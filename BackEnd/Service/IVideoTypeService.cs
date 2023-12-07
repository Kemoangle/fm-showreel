using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Showreel.Models;

namespace BackEnd.Service
{
    public interface IVideoTypeService
    {
        IEnumerable<VideoType> GetAllVideoType();
        VideoType GetVideoTypeById(int id);
        IEnumerable<VideoType> GetPageVideoType(string keySearch = "");
        void AddVideoType(VideoType videoType);
        void UpdateVideoType(VideoType videoType);
        IEnumerable<VideoType> GetVideoTypeByCategory(int categoryId);
    }
}