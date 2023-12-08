using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Showreel.Models;
using Showreel.Service;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayListController : ControllerBase
    {
        private readonly IPlaylistService playlistService;
        public PlayListController(IPlaylistService _playlistService)
        {
            playlistService = _playlistService;
        }
        [HttpGet]
        public ActionResult<Playlist> GetPagePlayList(string? keySearch = null, int page = 1, int pageSize = 10)
        {
            var query = playlistService.GetPagePlayList(keySearch);
            int startIndex = (page - 1) * pageSize;
            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var paginatedQuery = query.Skip(startIndex).Take(pageSize);
            if (!paginatedQuery.ToList().Any())
            {
                page = 1;
                totalPages = 1;
            }
            var response = new
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize,
                Playlist = paginatedQuery.ToList()
            };

            return Ok(response);
        }


        [HttpPost]
        public IActionResult CreatePlayList([FromBody] Playlist playlist)
        {
            var item = playlistService.AddPlayList(playlist);
            return Ok(item);
        }

        [HttpPost("CreatePlayListBuilding")]
        public IActionResult CreatePlayListBuilding([FromBody] Buildingplaylist buildingplaylist)
        {
            var item = playlistService.AddBuildingPlayList(buildingplaylist);
            return Ok(item);
        }

        [HttpGet("{id}")]
        public IActionResult GetAllVideoInList(int id)
        {
            var videoList = playlistService.GetVideoPlayList(id);
            return Ok(videoList);
        }

        [HttpPatch("{id}")]
        public IActionResult UpdatePlayList(int id, [FromBody] Playlist playlist)
        {
            var existingPlayList = playlistService.GetPlayListById(id);
            if (existingPlayList == null)
            {
                return NotFound("Playlist not found");
            }
            var response = playlistService.UpdatePlayList(playlist);

            return Ok(response);
        }

        [HttpPatch("UpdatePlayListVideo/{playListId}")]
        public IActionResult UpdatePlayListVideo(int playListId, [FromBody] Video[] videos)
        {
            var existingPlayList = playlistService.GetPlayListById(playListId);
            if (existingPlayList == null)
            {
                return NotFound("Playlist not found");
            }
            playlistService.UpdateVideoPlayList(videos,playListId);

            return Ok();
        }
    }
}