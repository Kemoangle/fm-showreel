using Showreel.Models;
using Showreel.Models.ViewModels;
namespace Showreel.Service.impl
{
    public class PlaylistService : IPlaylistService
    {
        private readonly ShowreelContext _context;
        public PlaylistService(ShowreelContext context)
        {
            _context = context;
        }

        public Buildingplaylist AddBuildingPlayList(Buildingplaylist buildingPlaylist)
        {
            _context.Buildingplaylists.Add(buildingPlaylist);
            _context.SaveChanges();
            return buildingPlaylist;
        }

        public PlaylistPost[] AddPlayList(PlaylistPost[] playlist)
        {
            _context.Playlists.AddRange(playlist);
            _context.SaveChanges();

            foreach (var pl in playlist)
            {
                List<Buildingplaylist> list = new List<Buildingplaylist>();

                foreach (int buildingId in pl.buildingsId)
                {
                    list.Add(new Buildingplaylist
                    {
                        BuildingId = buildingId,
                        PlaylistId = pl.Id
                    });
                }
                _context.Buildingplaylists.AddRange(list);

            }
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
                var subPlaylistDelete = _context.Playlists.Where(p => p.ParentId == id);
                _context.Playlists.RemoveRange(subPlaylistDelete);
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

        public IEnumerable<PlaylistWithBuilding> GetPlayListByParent(string keySearch = "", int parentId = 0, string buildingId = "")
        {
            var query = _context.Playlists.Select(x => new PlaylistWithBuilding
            {
                Id = x.Id,
                Title = x.Title,
                Creator = x.Creator,
                JsonListVideo = x.JsonListVideo,
                JsonPlaylist = x.JsonPlaylist,
                ParentId = x.ParentId,
                Status = x.Status,
                EndDate = x.EndDate,
                StartDate = x.StartDate,
                Buildings = _context.Buildings
               .Join(_context.Buildingplaylists, building => building.Id, buildingPlaylist => buildingPlaylist.BuildingId,
               (building, buildingPlaylist) => new
               {
                   building,
                   buildingPlaylist
               }).Where(v => v.buildingPlaylist.PlaylistId == x.Id)
               .Select(v => new BuildingDetail { Id = v.building.Id, BuildingName = v.building.BuildingName }).ToArray()
            }).AsQueryable();

            if (!string.IsNullOrEmpty(keySearch))
            {
                query = query.Where(b => b.Title.Contains(keySearch));
            }

            if (buildingId != null)
            {
                string[] listBuilding = buildingId.Split(", ");
                query = query.Where(b => b.Buildings.Any(x => listBuilding.Contains(x.Id.ToString())));
            }
            query = query.OrderByDescending(b => b.Id);
            return query.ToList();
        }
    }
}
