using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
namespace Api_WithMyAdds.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeopleController : ControllerBase
    {
        Dictionary<string, int> people_age = new Dictionary<string, int>() {
            {"Jakub",32 },
            {"Jan",14 },
            {"Adam",98 },
        };

        [HttpGet]
        public ContentResult Get()
        {
            var c = new ContentResult();
            c.Content = JsonSerializer.Serialize(people_age, typeof(Dictionary<string, int>), new JsonSerializerOptions() { WriteIndented = true }) ;
            return  c;
        }

        [HttpPost] // not working
        [Route("/{name}/{age:int}")]
        public RedirectResult Post(string name, int age)
        {
            people_age.Add(name, age);
            return new RedirectResult("/people");
        }
    }
}
