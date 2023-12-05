using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Showreel.Models;

namespace BackEnd.Service.impl
{
    public class VideoTypeService : IVideoTypeService
    {
        private readonly ShowreelContext _context;

        public VideoTypeService(ShowreelContext context)
        {
            _context = context;   
        }

        public void AddVideoType(VideoType videoType)
        {
            _context.VideoTypes.Add(videoType);
            _context.SaveChanges();
        }

        public IEnumerable<VideoType> GetAllVideoType()
        {
            return _context.VideoTypes.ToList();
        }

        public IEnumerable<VideoType> GetPageVideoType(string keySearch = "")
        {
            var query = _context.VideoTypes.AsQueryable();
            if (!string.IsNullOrEmpty(keySearch))
            {
                query = query.Where(c => c.Name.Contains(keySearch));
            }
            query = query.OrderByDescending(b => b.Id);
            return query.ToList();
        }

        public VideoType GetVideoTypeById(int id)
        {
            return _context.VideoTypes.FirstOrDefault(v => v.Id == id);
        }

        public void UpdateVideoType(VideoType videoType)
        {
            _context.Update(videoType);
            _context.SaveChanges();
        }
    }
}