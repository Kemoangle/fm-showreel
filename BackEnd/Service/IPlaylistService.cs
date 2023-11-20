using Showreel.Models;

namespace Showreel.Service
{
    public interface IPlaylistService
    {
        IList<Video> sortingVideo(List<Video> videos);
    }
}
