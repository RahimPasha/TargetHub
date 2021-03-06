namespace TargetHubApi.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TargetHubApi.Infrastructure.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(TargetHubApi.Infrastructure.ApplicationDbContext context)
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

            context.Servers.AddOrUpdate(
                new Server
                {
                    Id = 1,
                    Name = "A",
                    Identifier = "123"
                }
            );

            context.ServerRequestTypes.AddOrUpdate(
                new ServerRequestType
                {
                    Id = 1,
                    Type = "Register"
                }
            );
        }
    }
}
