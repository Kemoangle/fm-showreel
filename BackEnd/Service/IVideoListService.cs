using Showreel.Models;

namespace Showreel.Service
{
    public interface IVideoListService
    {
        IEnumerable<Videolist> GetAllVideoList();
        IEnumerable<Videolist> GetPageVideoList(string keySearch = "");
        Videolist GetVideoListById(int id);
        IEnumerable<VideoVideolist> GetVideoVideolist(int id);
        Videolist AddVideoList(Videolist videoList);
        void AddVideoVideoList(VideoVideolist videoVideolist);
        Videolist UpdateVideoList(Videolist videoList);
        void DeleteVideoList(int id);

        void UpdateVideoVideoList(int videoListId,VideoVideolist[] videoVideolist);
    }
}
