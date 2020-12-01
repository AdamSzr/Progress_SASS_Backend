using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SassBackProj.Models;
using SassBackProj.Services.FoodService;

namespace SassBackProj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IFoodService service;

        public FoodController(IFoodService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return  Ok(service.GetAll());
        }

        [HttpPost]
        public IActionResult Create(Food food)
        {
            return Ok(service.Create(food));
        }

        [Route("{id:int}")]
        [HttpGet]
        public IActionResult Get(int id)
        {
            return Ok(service.GetFoodById(id));
        }
    }
}
