using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace DatingApp.API.Data {
    public class Seed {
        public static void SeedUsers (UserManager<User> userManager, RoleManager<Role> roleManager) {
            if (!userManager.Users.Any ()) {
                var userData = File.ReadAllText ("Data/UserSeed.json");
                var users = JsonConvert.DeserializeObject<List<User>> (userData);

                var roles = new List<Role> {
                    new Role { Name = "Member" },
                    new Role { Name = "Admin" },
                    new Role { Name = "Moderator" },
                    new Role { Name = "VIP" },
                };

                foreach (var role in roles) {
                    roleManager.CreateAsync (role).Wait ();
                }

                foreach (var user in users) {
                    userManager.CreateAsync (user, "password").Wait ();
                    userManager.AddToRoleAsync (user, "Member").Wait ();
                    // byte[] passwordhash, passwordSalt;
                    // CreatePsswordHash ("password", out passwordhash, out passwordSalt);
                    // // user.PasswordHash = passwordhash;
                    // // user.PasswordSalt = passwordSalt;
                    // user.UserName = user.UserName.ToLower ();
                    // context.Users.Add (user);
                }

                // context.SaveChanges ();

                var adminUser = new User {
                    UserName = "Admin"
                };

                var result = userManager.CreateAsync (adminUser, "password").Result;

                if (result.Succeeded) {
                    var admin = userManager.FindByNameAsync ("Admin").Result;
                    userManager.AddToRolesAsync (admin, new [] { "Admin", "Moderatore" }).Wait ();
                }

            }
        }

        public static void CreatePsswordHash (string password, out byte[] passwordHash, out byte[] passwordSalt) {
            using (var hmac = new System.Security.Cryptography.HMACSHA512 ()) {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash (Encoding.UTF8.GetBytes (password));
            }
        }
    }
}