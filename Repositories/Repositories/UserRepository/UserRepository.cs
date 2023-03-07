using BookStoreWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly eBookStore5Context context;

        public UserRepository(eBookStore5Context context)
        {
            this.context = context;
        }

        public User GetUserByUsername(string userName)
        {
            return context.Users.Where(u => u.Mail == userName).FirstOrDefault();
        }
    }
}
