using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using BookStoreWebApp.Models;
using BookStoreWebApp.Supporters.Constants;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace BookStoreWebApp.Supporters.DataGenerator
{
    public class ApplicationDataGenerator
    {
        const string DEFAULT = "default1";
        public static async Task Initialize(IServiceProvider serviceProvider, string adminName, string adminPassword)
        {
            using (var context = new eBookStore3Context(serviceProvider.GetRequiredService<DbContextOptions<eBookStore3Context>>()))
            {
                await CreateListOfRoleAsync(serviceProvider, new List<String> { RoleName.ADMIN, RoleName.USER });

                var adminID = await EnsureUser(serviceProvider, adminPassword, adminName);
                await EnsureRole(serviceProvider, adminID, RoleName.ADMIN);
            };
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider, string testUserPw, string userName)
        {
            var userManager = serviceProvider.GetService<UserManager<User>>();

            var user = await userManager.FindByNameAsync(userName);
            if(user == null)
            {
                user = new User()
                {
                    Mail = userName,
                    DisplayName = DEFAULT,
                    Address = DEFAULT,
                    Phone = DEFAULT,
                    Avatar = DEFAULT,
                };
                await userManager.CreateAsync(user, testUserPw);
            }

            if(user == null)
            {
                throw new Exception("The password provided is not strong enough");
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider, string uid, string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<Role>>();

            if(roleManager == null)
            {
                throw new Exception("Null rolemanager");
            }

            IdentityResult IR;
            if(!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new Role() { Name = role });
            }

            var userManager = serviceProvider.GetService<UserManager<User>>();

            var user = await userManager.FindByIdAsync(uid);

            if(user == null)
            {
                throw new Exception("The password of user is not strong enough");
            }

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;

        }

        private static async Task<List<IdentityResult>> CreateListOfRoleAsync(IServiceProvider serviceProvider, List<string> roles)
        {
            var roleManager = serviceProvider.GetService<RoleManager<Role>>();

            if(roleManager == null)
            {
                throw new Exception("Null rolemanager");
            }
            List<IdentityResult> identityResults = new List<IdentityResult>() ;
            foreach(var role in roles)
            {
                if(!await roleManager.RoleExistsAsync(role))
                {
                    var identityResult = await roleManager.CreateAsync(new Role() { Name = role });
                    identityResults.Add(identityResult);
                }
            }

            return identityResults;
        }

    }
}
