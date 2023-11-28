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

        public void AddVideoList(Videolist videoList)
        {
            throw new NotImplementedException();
        }

        public void DeleteVideoList(int id)
        {
            throw new NotImplementedException();
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
            return query.ToList();
        }

        public Videolist GetVideoListById(int id)
        {
            return _context.Videolists.FirstOrDefault(v => v.Id == id);
        }

        public void UpdateVideoList(Videolist videoList)
        {
            throw new NotImplementedException();
        }
    }
}

