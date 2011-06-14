using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
    static class login
    {

        private static string username = "test";

        public static bool checkUsername(string name)
        {
            return (username == name);
        }
    }
}
