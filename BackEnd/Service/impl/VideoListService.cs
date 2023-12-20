using Showreel.Models;
using Microsoft.EntityFrameworkCore;

namespace Showreel.Service.impl
{
    public class VideoListService : IVideoListService
    {
        private readonly ShowreelContext _context;

        public VideoListService(ShowreelContext context)
        {
            _context = context;
        }

        public Videolist AddVideoList(Videolist videoList)
        {
            _context.Videolists.Add(videoList);
            _context.SaveChanges();
            return videoList;
        }

        public void AddVideoVideoList(VideoVideolist videoVideolist)
        {
            _context.VideoVideolists.Add(videoVideolist);
            _context.SaveChanges();
        }

        public void DeleteVideoList(int id)
        {
            var videoListDelete = _context.Videolists.Find(id);
            if (videoListDelete != null)
            {
                _context.Videolists.Remove(videoListDelete);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Videolist> GetAllVideoList()
        {
            return _context.Videolists.OrderByDescending(x => x.Id).ToList();
        }

        public IEnumerable<Videolist> GetPageVideoList(string keySearch = "")
        {
            var query = _context.Videolists.AsQueryable();
            if (!string.IsNullOrEmpty(keySearch))
            {
                query = query.Where(b => b.Title.Contains(keySearch));
            }
            query = query.OrderByDescending(b => b.Id);
            return query.ToList();
        }

        public Videolist GetVideoListById(int id)
        {
            return _context.Videolists.FirstOrDefault(v => v.Id == id);
        }

        public IEnumerable<VideoVideolist> GetVideoVideolist(int id)
        {
            return _context.VideoVideolists.Where(v => v.VideoListId == id).ToList();
        }

        public Videolist UpdateVideoList(Videolist videoList)
        {
            _context.Videolists.Update(videoList);
            _context.SaveChanges();
            return videoList;
        }

        public void UpdateVideoVideoList(int videoListId, VideoVideolist[] videoVideolist)
        {
            var existingVideoVideolist = _context.VideoVideolists
                                            .Where(v => v.VideoListId == videoListId)
                                            .ToList();
            if (existingVideoVideolist.Any())
            {
                _context.VideoVideolists.RemoveRange(existingVideoVideolist);
            }

            foreach (var videolist in videoVideolist)
            {
                var newVideoVideolist = new VideoVideolist
                {
                    VideoId = videolist.VideoId,
                    VideoListId = videoListId,
                    LoopNum = videolist.LoopNum
                };
                _context.VideoVideolists.Add(newVideoVideolist);
            }

            _context.SaveChanges();
        }
    }
}

