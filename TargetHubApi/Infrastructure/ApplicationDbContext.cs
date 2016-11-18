using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TargetHubApi.Models;

namespace TargetHubApi.Infrastructure
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext() : base("cnnStr") { }

        public DbSet<Server> Servers { get; set; }
        public DbSet<ServerRequest> ServerRequests { get; set; }
        public DbSet<ServerRequestType> ServerRequestTypes { get; set; }
        public DbSet<Target> Targets { get; set; }
        public DbSet<TargetRequest> TargetRequests { get; set; }
        public DbSet<TargetRequestType> TargetRequestTypes { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<ContentRequest> ContentRequests { get; set; }
        public DbSet<ContentRequestType> ContentRequestTypes { get; set; }
    }
}