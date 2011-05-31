using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
    class Student : User
    {
        public int Studentnumber { get; set; }
        public string Study { get; set; }

        public Student()
        {
            this.User_type = "student";
        }
    }
}
