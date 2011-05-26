using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
    class Admin
    {
        private User _id;

        internal User Id
        {
            get { return _id; }
            set { _id = value; }
        }


        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }


    }
}
