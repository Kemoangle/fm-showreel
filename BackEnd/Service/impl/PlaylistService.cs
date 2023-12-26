using Showreel.Models;

namespace Showreel.Service.impl
{
    public class PlaylistService : IPlaylistService
    {
        private readonly ShowreelContext _context;

        public PlaylistService(ShowreelContext context)
        {
            _context = context;
        }

        public Buildingplaylist AddBuildingPlayList(Buildingplaylist buildingplaylist)
        {
            _context.Buildingplaylists.Add(buildingplaylist);
            _context.SaveChanges();
            return buildingplaylist;
        }

        public Playlist[] AddPlayList(Playlist[] playlist)
        {
            _context.Playlists.AddRange(playlist);
            _context.SaveChanges();
            return playlist;
        }


        public IEnumerable<Playlist> GetPagePlayList(string keySearch = "")
        {
            var query = _context.Playlists.Where(q => q.ParentId == 0 || q.ParentId == null).AsQueryable();
            if (!string.IsNullOrEmpty(keySearch))
            {
                query = query.Where(b => b.Title.Contains(keySearch) && (b.ParentId == 0 || b.ParentId != null));
            }
            query = query.OrderByDescending(b => b.Id);
            return query.ToList();
        }

        public Playlist GetPlayListById(int id)
        {
            var query = _context.Playlists.FirstOrDefault(p => p.Id == id);
            if (query == null)
                throw new KeyNotFoundException("Playlist not found");
            return query;
        }
        public IEnumerable<Video> GetVideoPlayList(int playListId)
        {
            var query = from pv in _context.Playlistvideos.Where(p => p.PlaylistId == playListId)
                        join v in _context.Videos on pv.VideoId equals v.Id
                        select v;
            return query.ToList();
        }

        public Playlist UpdatePlayList(Playlist playlist)
        {
            _context.Playlists.Update(playlist);
            _context.SaveChanges();
            return playlist;
        }

        public void DeletePlaylist(int id)
        {
            var playlistDelete = _context.Playlists.Find(id);
            if (playlistDelete != null)
            {
                _context.Playlists.Remove(playlistDelete);
                _context.SaveChanges();
            }
        }

        public void UpdateVideoPlayList(Video[] videos, int playListId)
        {
            var playlistvideoExist = _context.Playlistvideos
                                            .Where(v => v.PlaylistId == playListId)
                                            .ToList();
            if (playlistvideoExist.Any())
            {
                _context.Playlistvideos.RemoveRange(playlistvideoExist);
            }

            foreach (var video in videos)
            {
                var newPlayList = new Playlistvideo
                {
                    VideoId = video.Id,
                    PlaylistId = playListId
                };
                _context.Playlistvideos.Add(newPlayList);
            }
            _context.SaveChanges();
        }

        public IEnumerable<Playlist> GetPlayListByParent(string keySearch = "", int parentId = 0)
        {
            var query = _context.Playlists.Where(p => p.ParentId == parentId).AsQueryable();
            if (!string.IsNullOrEmpty(keySearch))
            {
                query = query.Where(b => b.Title.Contains(keySearch));
            }
            query = query.OrderByDescending(b => b.Id);
            return query.ToList();
        }
    }
}
