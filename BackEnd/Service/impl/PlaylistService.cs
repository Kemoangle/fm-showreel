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
        
    }
}
