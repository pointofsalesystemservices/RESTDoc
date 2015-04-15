namespace RestDoc.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models.Data;

    internal sealed class Configuration : DbMigrationsConfiguration<RestDoc.Models.Data.RestDocContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(RestDoc.Models.Data.RestDocContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //


            if (!context.RestApis.Any())
            {
                var newApi = new RestApi
                {
                    Name = "Demo",
                    PathGroups =
                    {
                        new ApiPathGroup
                        {
                            Description = "Demo Path Group",
                            BasePath = "/api/demo",
                            ApiPaths =
                            {
                                new ApiPath
                                {
                                    Description = "Demo Path",
                                    Path = "demo",
                                    ApiVerbs =
                                    {
                                        new ApiVerb
                                        {
                                            ResponseBody = new ApiBody
                                            {
                                                Description = "Foo",
                                                Example = "bar"
                                            },
                                            Verb = "GET",
                                            Parameters =
                                            {
                                                new ApiParameter
                                                {
                                                    DataType = "int",
                                                    DefaultValue = "0",
                                                    Name = "demoParameter",
                                                    Required = true
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                };

                context.RestApis.Add(newApi);
                context.SaveChanges();
            }
        }
    }
}
