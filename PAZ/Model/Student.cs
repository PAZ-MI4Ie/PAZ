using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
    class Student
    {
        private int _user_id;

        public int User_id
        {
            get { return _user_id; }
            set { _user_id = value; }
        }


        private int _studentnumber;

        public int Studentnumber
        {
            get { return _studentnumber; }
            set { _studentnumber = value; }
        }


        private string _study;

        public string Study
        {
            get { return _study; }
            set { _study = value; }
        }
    }
}
