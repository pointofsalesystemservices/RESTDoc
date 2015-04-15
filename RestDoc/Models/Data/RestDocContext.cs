namespace RestDoc.Models.Data
{
    using System.Data.Entity;

    public class RestDocContext : DbContext
    {


        public DbSet<RestApi> RestApis { get; set; }
        public DbSet<ApiPathGroup> ApiPathGroups { get; set; }
        public DbSet<ApiBody> ApiBodies { get; set; }
        public DbSet<ApiPath> ApiPaths { get; set; }
        public DbSet<ApiVerb> ApiVerbs { get; set; }
        public DbSet<ApiParameter> ApiParameters { get; set; }

    }
}