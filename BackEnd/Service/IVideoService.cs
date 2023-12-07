using Showreel.Models;

namespace Showreel.Service
{
    public interface IVideoService
    {
        IEnumerable<Video> GetAllVideos();
        IEnumerable<Video> GetPageVideos(string keySearch ="");
        IEnumerable<Video> GetVideoByList(int id);

        Video GetVideoById(int id);
        Video AddVideo(Video video);
        Video UpdateVideo(Video video);
        void DeleteVideo(int id);
        Video GetVideoByKeyNo(string keyNo);
    }
}
