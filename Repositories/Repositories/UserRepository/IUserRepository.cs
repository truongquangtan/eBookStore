using BookStoreWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.UserRepository
{
    public interface IUserRepository
    {
        public User GetUserByUsername(string userName);
    }
}
