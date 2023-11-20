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
        public IList<Video> sortingVideo(List<Video> videos)
        {
            var sortedVideo = videos.OrderBy(v => v.Category).ToList();
            for(int i = 0; i < sortedVideo.Count(); i++)
            {
                if (sortedVideo[i].CategoryId == sortedVideo[i+1].CategoryId)
                {

                }
            }
            throw new NotImplementedException();
        }
    }
}
