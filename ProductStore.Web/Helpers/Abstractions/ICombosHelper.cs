using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProductStore.Web.Helpers.Abstractions
{
    public interface ICombosHelper
    {
        public Task<List<SelectListItem>> GetComboCategory();
    }
}
