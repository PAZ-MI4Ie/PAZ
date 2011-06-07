using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
    class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Surname { get; set; }
        public string Firstname { get; set; }
        public string Email { get; set; }
        public string User_type { get; set; }
        public string Status { get; set; }

		public User()
		{
		}

		public User(string surname, string firstname)
		{
			Surname = surname;
			Firstname = firstname;
		}
    }
}
