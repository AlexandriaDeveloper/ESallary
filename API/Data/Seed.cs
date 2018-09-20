using System.Collections.Generic;
using System.Linq;
using API.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace API.Data {
    public class Seed {
        private readonly UserManager<User> _userManager;
        public Seed (UserManager<User> userManager) {
            _userManager = userManager;

        }
        public void SeedUsers () {
            if (!_userManager.Users.Any ()) {
                    var userData= System.IO.File.ReadAllText("Data/UserSeedData.json");
                    var users = JsonConvert.DeserializeObject<List<User>>(userData);
                    foreach (var user in users)
                    {
                        user.UserName= user.UserName.ToLower();
                       _userManager.CreateAsync(user,"password").Wait();
                    
                    }
            }
        }
    }
}