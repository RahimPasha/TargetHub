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

        public DbSet<Test> Tests { get; set; }
    }
}