using Showreel.Models;
using Showreel.Models.ViewModels;

namespace Showreel.Service
{
    public interface IPlaylistService
    {
        IEnumerable<Playlist> GetPagePlayList(string keySearch = "");
        PlaylistPost[] AddPlayList(PlaylistPost[] playlist);
        Buildingplaylist AddBuildingPlayList(Buildingplaylist buildingplaylist);
        Playlist GetPlayListById(int id);
        void DeletePlaylist(int id);
        IEnumerable<Video> GetVideoPlayList(int playListId);
        Playlist UpdatePlayList(Playlist playlist);
        void UpdateVideoPlayList(Video[] videos, int playListId);
        IEnumerable<PlaylistWithBuilding> GetPlayListByParent(string keySearch = "", int parentId = 0, string buildingId = "");
    }
}
