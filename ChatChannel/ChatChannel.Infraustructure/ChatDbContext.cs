using ChatChannel.Domain.Model.Entity;
using ChatChannel.Infraustructure.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatChannel.Infraustructure
{
    public class ChatDbContext : DbContext
    {
        public DbSet<User> Users { get; private set; }

        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
        {


        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
        }

    }
}
