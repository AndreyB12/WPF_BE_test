using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_BE_test
{
    class Context : DbContext
    {
        public Context() : base("name = ConnectionString")
        {
            Database.SetInitializer<Context>(new DropCreateDatabaseIfModelChanges<Context>());

        }

        public DbSet<NotSortedItem> NotSorted { get; set; }
        public DbSet<SortedItem> Sorted { get; set; }

    }
}
