using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
    public class Student : User
    {
        public int Studentnumber { get; set; }
        public string Study { get; set; }

        private static int tempHack = 0;

		public Student()
		{
			this.User_type = "student";
		}

        public Student(string surname, string firstname, int studentnumber, string study)
			: base(surname, firstname)
        {
            this.User_type = "student";

            this.Studentnumber = studentnumber;
            this.Study = study;
            this.Email = "teacher" + (++tempHack) + "@avans.nl";
        }
    }
}
