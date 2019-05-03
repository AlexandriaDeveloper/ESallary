using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using API.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace API.Data {
    public class Seed {
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;
        public Seed (UserManager<User> userManager, DataContext context) {
            _userManager = userManager;
            _context = context;
        }
        public void SeedUsers () {
            if (!_userManager.Users.Any ()) {
                var userData = System.IO.File.ReadAllText ("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>> (userData);
                foreach (var user in users) {
                    user.UserName = user.UserName.ToLower ();
                    _userManager.CreateAsync (user, "123").Wait ();

                }
            }
        }

        public async void SeedDepartment () {
            if(!_context.Departments.Any())
            {
            _context.Departments.Add (new Department () { Name = "1-غير مسجل"});

            await _context.SaveChangesAsync ();
            }
        }
    }
}