using static System.Collections.Specialized.BitVector32;

namespace ProductStore.Web.Data.Entities
{
    public class RoleCategory
    {
        public required Guid ProductStoreRoleId { get; set; }
        public required Guid CategoryId { get; set; }
        public ProductStoreRole ProductStoreRole { get; set; }
        public Category Category { get; set; }
    }
}
