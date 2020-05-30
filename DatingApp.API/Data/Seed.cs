using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DatingApp.API.Models;
using Newtonsoft.Json;

namespace DatingApp.API.Data {
    public class Seed {
        public static void SeedUsers (DataContext context) {
            if (!context.Users.Any ()) {
                var userData = File.ReadAllText ("Data/UserSeed.json");
                var users = JsonConvert.DeserializeObject<List<User>> (userData);
                foreach (var user in users) {
                    byte[] passwordhash, passwordSalt;
                    CreatePsswordHash ("password", out passwordhash, out passwordSalt);
                    user.PasswordHash = passwordhash;
                    user.PasswordSalt = passwordSalt;
                    user.Username = user.Username.ToLower ();
                    context.Users.Add (user);
                }

                context.SaveChanges ();
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