namespace ProductStore.Web.Data.Entities
{
    public class RolePermission
    {
        public required Guid ProductStoreRoleId { get; set; }
        public required Guid PermissionId { get; set; }
        public ProductStoreRole ProductStoreRole { get; set; }
        public Permission Permission { get; set; }
    }
}
