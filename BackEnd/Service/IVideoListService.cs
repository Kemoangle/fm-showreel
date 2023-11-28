using Showreel.Models;

namespace Showreel.Service
{
    public interface IVideoListService
    {
        IEnumerable<Videolist> GetAllVideoList();
        IEnumerable<Videolist> GetPageVideoList(string keySearch = "");
        Videolist GetVideoListById(int id);
        void AddVideoList(Videolist videoList);
        void UpdateVideoList(Videolist videoList);
        void DeleteVideoList(int id);
    }
}
