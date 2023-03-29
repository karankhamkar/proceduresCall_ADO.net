using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CableOperatorApp
{
    public class UserStore
    {
        public static List<User> GetUsers() 
        {
            List<User> users = new List<User>();
            users.Add(new User("Rayaba", "Pass123"));
            users.Add(new User("user123", "pass123"));
            return users;
        }

    }
}
