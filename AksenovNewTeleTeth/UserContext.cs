using System;
using System.Data.Entity;
using AksenovNewTeleTeth.Models;

namespace AksenovNewTeleTeth
{
    class UserContext : DbContext
    {
        public UserContext()
            : base("DbConnection")
        { }

        public DbSet<PointObject> PointObjects { get; set; }
        public DbSet<MainObject> MainObjects { get; set; }
    }
}
