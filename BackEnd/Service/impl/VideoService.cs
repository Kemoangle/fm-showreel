using Microsoft.EntityFrameworkCore;
using Showreel.Models;

namespace Showreel.Service.impl
{
    public class VideoService : IVideoService
    {
        private readonly ShowreelContext _context;

        public VideoService(ShowreelContext context)
        {
            _context = context;
        }

        public IEnumerable<Video> GetAllVideos()
        {
            return _context.Videos.ToList();
        }

        public Video GetVideoById(int id)
        {
            return _context.Videos.FirstOrDefault(b => b.Id == id);
        }

        public void AddVideo(Video video)
        {
            _context.Videos.Add(video);
            _context.SaveChanges();
        }

        public void UpdateVideo(Video video)
        {
            _context.Entry(video).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteVideo(int id)
        {
            var videoDelete = _context.Videos.Find(id);
            if (videoDelete != null)
            {
                _context.Videos.Remove(videoDelete);
                _context.SaveChanges();
            }
        }
    }
}
