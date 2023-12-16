using Showreel.Models;

namespace Showreel.Service
{
    public interface IPlaylistService
    {
        IEnumerable<Playlist> GetPagePlayList(string keySearch = "");
        Playlist AddPlayList(Playlist playlist);
        Buildingplaylist AddBuildingPlayList(Buildingplaylist buildingplaylist);
        Playlist GetPlayListById(int id);
        void DeletePlaylist(int id);
        IEnumerable<Video> GetVideoPlayList(int playListId);
        Playlist UpdatePlayList(Playlist playlist);
        void UpdateVideoPlayList(Video[] videos, int playListId);
    }
}
