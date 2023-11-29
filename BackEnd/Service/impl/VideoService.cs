using Google.Protobuf.WellKnownTypes;
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

        public IEnumerable<Video> GetPageVideos(string keySearch = "")
        {
            var query = _context.Videos.AsQueryable();
            if (!string.IsNullOrEmpty(keySearch))
            {
                query = query.Where(b => b.Title.Contains(keySearch));
            }
            return query.ToList();
        }

        public Video GetVideoById(int id)
        {
            return _context.Videos.FirstOrDefault(v => v.Id == id);
        }

        public void AddVideo(Video video)
        {
            _context.Videos.Add(video);
            _context.SaveChanges();
        }

        public void UpdateVideo(Video video)
        {
            _context.Update(video);
            _context.SaveChanges();
        }

        public void DeleteVideo(int id)
        {
            var videoCategoriesToDelete = _context.Videocategories
                                            .Where(vc => vc.VideoId == id)
                                            .ToList();

            _context.Videocategories.RemoveRange(videoCategoriesToDelete);
            var videoListToDelete = _context.VideoVideolists
                                            .Where(v => v.VideoId == id)
                                            .ToList();
            _context.VideoVideolists.RemoveRange(videoListToDelete);
            var videoDelete = _context.Videos.Find(id);
            if (videoDelete != null)
            {
                _context.Videos.Remove(videoDelete);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Video> GetAllVideos()
        {
            return _context.Videos.ToList();
        }

        public Video GetVideoByKeyNo(string keyNo)
        {
            return _context.Videos.FirstOrDefault(v => v.KeyNo == keyNo);
        }

        public IEnumerable<Video> GetVideoByList(int id)
        {
            var query = (from v in _context.Videos
                        join l in _context.VideoVideolists on v.Id equals l.VideoId
                        where l.VideoListId == id
                        select v).ToList();
            return query;
        }
    }
}
