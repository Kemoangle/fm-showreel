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
            var videoListToDelete = _context.VideoVideolists
                                            .Where(v => v.VideoListId == id)
                                            .ToList();
            _context.VideoVideolists.RemoveRange(videoListToDelete);

            var videoListDelete = _context.Videolists.Find(id);
            if (videoListDelete != null)
            {
                _context.Videolists.Remove(videoListDelete);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Videolist> GetAllVideoList()
        {
            return _context.Videolists.ToList();
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

        public void UpdateVideoList(Videolist videoList)
        {
            throw new NotImplementedException();
        }
    }
}

