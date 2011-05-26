using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
    class User
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _username;

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        private string _surname;

        public string Surname
        {
            get { return _surname; }
            set { _surname = value; }
        }

        private String _firstname;

        public String Firstname
        {
            get { return _firstname; }
            set { _firstname = value; }
        }

        private string _email;

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        private string _user_type;

        public string User_type
        {
            get { return _user_type; }
            set { _user_type = value; }
        }

        private string _status;

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }
    }
}
