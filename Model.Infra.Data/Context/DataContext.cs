using Microsoft.EntityFrameworkCore;
using Model.Domain.Entities;
using Model.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Text;

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
