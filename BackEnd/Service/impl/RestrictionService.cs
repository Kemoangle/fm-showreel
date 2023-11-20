using Microsoft.EntityFrameworkCore;
using Showreel.Models;

namespace Showreel.Service.impl
{
    public class RestrictionService : IRestrictionService
    {
        private readonly ShowreelContext _context;

        public RestrictionService(ShowreelContext context)
        {
            _context = context;
        }

        public IEnumerable<Restriction> GetAllRestrictions()
        {
            return _context.Restrictions.ToList();
        }

        public Restriction GetRestrictionById(int id)
        {
            return _context.Restrictions.FirstOrDefault(b => b.Id == id);
        }

        public void AddRestriction(Restriction restriction)
        {
            _context.Restrictions.Add(restriction);
            _context.SaveChanges();
        }

        public void UpdateRestriction(Restriction restriction)
        {
            _context.Entry(restriction).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteRestriction(int id)
        {
            var restrictionToDelete = _context.Restrictions.Find(id);
            if (restrictionToDelete != null)
            {
                _context.Restrictions.Remove(restrictionToDelete);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Restriction> GetRestrictionByBuildingId(int id)
        {
            var query = from br in _context.Buildingrestrictions.Where(b => b.BuildingId == id)
                        join r in _context.Restrictions on br.RestrictionId equals r.Id
                        select new Restriction
                        {
                            Id = r.Id,
                            RestrictionName = r.RestrictionName
                        };

            return query;
        }
    }
}
