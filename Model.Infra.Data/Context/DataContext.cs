using Model.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Model.Infra.Data.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions <DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
