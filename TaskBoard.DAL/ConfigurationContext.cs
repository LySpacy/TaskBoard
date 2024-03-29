using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.DAL.Configuration;

namespace TaskBoard.DAL
{
    public static class ConfigurationContext
    {
        public static void InitializeConfiguration(this ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ProjectConfiguration());
            builder.ApplyConfiguration(new SprintConfiguration());
            builder.ApplyConfiguration(new TaskConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new FileConfiguration());
        }
    }
}
