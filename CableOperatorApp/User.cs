using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CableOperatorApp
{
    public class User
    {
        #region Properties
        public string UserName { get; set; }
        public string Password { get; set; }
        #endregion

        #region Constructor

        public User(string name, string pass)
        {
            this.UserName = name;
            this.Password = pass;
        }


        #endregion

        #region Methods
        public static bool IsPasswordValid(string input)
        {
            bool result = false;
            if (string.IsNullOrEmpty(input) == false && 6 <= input.Length && input.Length <= 8)
            {
                result = true;
            }
            return result;
        }

        public static bool IsUserNameValid(string input)
        {
            bool result = false;
            if (string.IsNullOrEmpty(input) == false && 6 <= input.Length && input.Length <= 8)
            {
                result = true;
            }
            return result;
        }

        #endregion
    }
}
