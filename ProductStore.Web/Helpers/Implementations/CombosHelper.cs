using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductStore.Web.Data;
using ProductStore.Web.Helpers.Abstractions;

namespace ProductStore.Web.Helpers.Implementations
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _context;

        public CombosHelper(DataContext context)
        {
            _context = context;
        }

        public async Task<List<SelectListItem>> GetComboCategory()
        {
            return await _context.Category.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToListAsync();
        }
    }
}
