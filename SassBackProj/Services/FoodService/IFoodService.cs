using SassBackProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SassBackProj.Services.FoodService
{
   public interface IFoodService
    {
        public List<Food> GetAll();
        public List<Food> Create(Food food);
        public Food GetFoodById(int id);
    }
}
