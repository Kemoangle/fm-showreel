using Showreel.Models;

namespace Showreel.Service
{
    public interface IVideoService
    {
        IEnumerable<Video> GetAllVideos();
        Video GetVideoById(int id);
        void AddVideo(Video building);
        void UpdateVideo(Video building);
        void DeleteVideo(int id);
    }
}
