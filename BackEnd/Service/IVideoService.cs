using Showreel.Models;

namespace Showreel.Service
{
    public interface IVideoService
    {
        IEnumerable<Video> GetAllVideos();
        Video GetVideoById(int id);
        void AddVideo(Video video);
        void UpdateVideo(Video video);
        void DeleteVideo(int id);
    }
}
