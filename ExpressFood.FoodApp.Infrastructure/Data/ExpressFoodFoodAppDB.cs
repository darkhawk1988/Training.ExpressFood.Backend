using ExpressFood.FoodApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressFood.FoodApp.Infrastructure.Data
{
    public class ExpressFoodFoodAppDB : DbContext
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public ExpressFoodFoodAppDB(DbContextOptions<ExpressFoodFoodAppDB> options)
            :base(options)
        {
            
        }
    }
}
