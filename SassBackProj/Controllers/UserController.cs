using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SassBackProj.Models;
using System.Text.Json;

namespace SassBackProj.Controllers
{
    public static class UserData
    {
        static public List<Models.User> Users = new List<Models.User>()
        {
            new Models.User(){id=0,nick="muminek",passw="tajne"},
            new Models.User(){id=1,nick="Kubus",passw="Puchatek"},
            new Models.User(){id=2,nick="Tabaluga",passw="nieluga"},
            new Models.User(){id=3,nick="prosiaczek",passw="haslo"},
        };
    }

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {


        [HttpGet]
        public IActionResult Get()
        {
            string json_data = JsonSerializer.Serialize(UserData.Users, typeof(List<User>), new JsonSerializerOptions() { WriteIndented = true });
            return this.Ok(json_data);
        }

        [Route("{id:int}")]
        [HttpGet]
        public IActionResult Get(int id)
        {
            var user = UserData.Users.FirstOrDefault((user) => user.id == id);
            if (user != null)
                return Ok(JsonSerializer.Serialize<User>(user, new JsonSerializerOptions() { WriteIndented = true }));

            return NotFound($"Not found user with ID:{id}");
        }

        [HttpPut]
        public IActionResult UpdateUser(User user)
        {
            var u = UserData.Users.First((x) => x.id == user.id);
            UserData.Users.Remove(u);
            UserData.Users.Add(user);

            return Ok(user);
        }


        [HttpPost]
        public IActionResult Create(User user)
        {
            UserData.Users.Add(user);

            return Ok(JsonSerializer.Serialize<User>(user, new JsonSerializerOptions() { WriteIndented = true }));
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            //var user = UserData.Users.FirstOrDefault((user) => user.id == id);
            if(! UserData.Users.Any(user=> user.id==id))
                return NotFound($"Not found user with ID:{id}");

            UserData.Users.RemoveAll(u => u.id == id);

            return Ok("Deleted succesfully");
        }

    }
}
