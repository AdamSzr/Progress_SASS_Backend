using SassBackProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SassBackProj.Services.FoodService
{
    public class FoodService : IFoodService
    {
        static List<Food> meals = new List<Food>()
        {
            new Food(){id=0,name="kawa",description="zborzowa" },
            new Food(){id=1,name="kawa",description="INKA" },
            new Food(){id=2,name="kawa",description="prawdziwa" },
            new Food(){id=3,name="kawa",description="Anatol" },
        };

        public List<Food> Create(Food food)
        {
            meals.Add(food);
            return meals;
        }

        public List<Food> GetAll()
        {
            return meals;
        }

        public Food GetFoodById(int id)
        {
            return meals.FirstOrDefault((item) => item.id == id);
        }
    }
}
