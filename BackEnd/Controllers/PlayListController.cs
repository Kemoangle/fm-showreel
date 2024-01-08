using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Showreel.Models;
using Showreel.Models.ViewModels;
using Showreel.Service;

namespace BackEnd.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class PlayListController : ControllerBase
    {
        private readonly ShowreelContext _context;

        private readonly IPlaylistService playlistService;
        public PlayListController(IPlaylistService _playlistService, ShowreelContext context)
        {
            playlistService = _playlistService;
            _context = context;
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
        [HttpDelete("{id}")]
        public IActionResult DeletePlaylist(int id)
        {
            playlistService.DeletePlaylist(id);
            return Ok();
        }

        [HttpPost]
        public IActionResult CreatePlayList([FromBody] PlaylistPost[] playlists)
        {
            foreach (var pl in playlists)
            {
                var playlist = _context.Playlists.Where(x => x.Title == pl.Title).FirstOrDefault();
                if (playlist != null)
                {
                    ModelState.AddModelError("Playlist", "The name already exists");
                    return BadRequest(ModelState);
                }
            }
            var item = playlistService.AddPlayList(playlists);
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
            playlistService.UpdateVideoPlayList(videos, playListId);

            return Ok();
        }

        [HttpGet("GetPlayListByParent/{id}")]
        public ActionResult<Playlist> GetPlayListByParent(int id, string? keySearch = null, int page = 1, int pageSize = 10, string? buildingId = null)
        {
            var query = playlistService.GetPlayListByParent(keySearch: keySearch, parentId: id, buildingId: buildingId);
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
                BuildingId = buildingId,
                CurrentPage = page,
                PageSize = pageSize,
                Playlist = paginatedQuery.ToList()
            };

            return Ok(response);
        }
    }
}