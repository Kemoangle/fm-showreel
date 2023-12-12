using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Showreel.Models;
using Newtonsoft;
using Newtonsoft.Json;
using Showreel.Service;

namespace BackEnd.Service.impl
{
    public class RestrcitionService : IRestrictionService
    {
        private readonly ShowreelContext context;
        private readonly IVideoCategoryService categoryService;
        public RestrcitionService(ShowreelContext _context, IVideoCategoryService _categoryService)
        {
            context = _context;
            categoryService = _categoryService;
        }

        public void AddRestriction(Restriction restriction)
        {
            context.Restrictions.Add(restriction);
            context.SaveChanges();
        }

        public void DeleteRestriction(int id)
        {
            var restrictionDelete = context.Restrictions.Find(id);
            if (restrictionDelete != null)
            {
                context.Restrictions.Remove(restrictionDelete);
                context.SaveChanges();
            }
        }

        public IEnumerable<Restriction> GetBuildingRestriction(int buildingId)
        {   
            var query = context.Restrictions.Where(r => r.BuildingId == buildingId);
            return query.ToList();
        }

        public Restriction GetRestrictionById(int id)
        {
            return context.Restrictions.FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<Category> GetRestrictionExcepts(int restrictionId)
        {
            var restriction = context.Restrictions.FirstOrDefault(r => r.Id == restrictionId);

            if (restriction == null || restriction.ArrCategory == null)
            {
                return Enumerable.Empty<Category>();
            }
            int[]? arrCategoryId = JsonConvert.DeserializeObject<int[]>(restriction.ArrCategory);

            if (arrCategoryId == null || arrCategoryId.Length == 0)
            {
                return Enumerable.Empty<Category>();
            }
            
            return context.Categories.Where(c => arrCategoryId.Contains(c.Id));
        }

        public void UpdateRestriction(Restriction restriction)
        {
            context.Restrictions.Update(restriction);
            context.SaveChanges();
        }
    }
}