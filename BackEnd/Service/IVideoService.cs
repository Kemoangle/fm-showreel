using Showreel.Models;

namespace Showreel.Service
{
    public interface IVideoService
    {
        IEnumerable<Video> GetAllVideos();
        IEnumerable<Video> GetPageVideos(string keySearch ="");
        IEnumerable<Video> GetVideoByList(int id);

        Video GetVideoById(int id);
        void AddVideo(Video video);
        void UpdateVideo(Video video);
        void DeleteVideo(int id);
        Video GetVideoByKeyNo(string keyNo);
    }
}
