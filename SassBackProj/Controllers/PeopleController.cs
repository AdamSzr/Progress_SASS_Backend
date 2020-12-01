using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace SassBackProj.Controllers
{
    public static class StaticPeopleData
    {
        public static Dictionary<string, int> people_age = new Dictionary<string, int>() {
            {"Jakub",32 },
            {"Jan",14 },
            {"Adam",98 },
        };
    }

    [ApiController]
    [Route("api/[controller]")]
    public class PeopleController : ControllerBase //Don't create a web API controller by deriving from the Controller class. Controller derives from ControllerBase and adds support for views, so it's for handling web pages, not web API requests. There's an exception to this rule: if you plan to use the same controller for both views and web APIs, derive it from Controller.
    {


        [HttpGet]
        public ContentResult Get()
        {
            var c = new ContentResult();
            c.Content = JsonSerializer.Serialize(StaticPeopleData.people_age, typeof(Dictionary<string, int>), new JsonSerializerOptions() { WriteIndented = true });
            return c;
        }


        [Route("{id:int}")]
        [HttpGet]
        public ContentResult GetById(int id)
        {
            var c = new ContentResult();
            c.Content = JsonSerializer.Serialize(StaticPeopleData.people_age.ElementAt(id), typeof(KeyValuePair<string, int>), new JsonSerializerOptions() { WriteIndented = true });
            return c;
        }

        [Route("{age:int}")]
        [HttpDelete] // not working
        public RedirectResult Post(int age)
        {
            var x = StaticPeopleData.people_age.FirstOrDefault((x) => x.Value == age);
            if (x.Key != null)
                StaticPeopleData.people_age.Remove(x.Key);

            return new RedirectResult("/people");

        }


        [Route("{name}/{age:int}")]
        [HttpPost] // not working
        public RedirectResult Post(string name, int age)
        {
            StaticPeopleData.people_age.Add(name, age);
            return new RedirectResult("/people");
        }
    }
}
