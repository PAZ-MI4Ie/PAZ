using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
    public class Teacher : User
    {
        public enum session_spread {ANY,CLOSE,FAR};

		public session_spread Session_spread { get; set; }
		public DateTime blockedTimeslot { get; set; }

		public Teacher()
		{
			this.User_type = "teacher";
		}

		public Teacher(string surname, string firstname)
			: base(surname, firstname)
		{
			this.User_type = "teacher";
		}
    }
}
