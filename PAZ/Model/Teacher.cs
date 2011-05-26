using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
    class Teacher
    {

        private int _user_id;

        public int User_id
        {
            get { return _user_id; }
            set { _user_id = value; }
        }


        public enum _session_spread {ANY,LOSE,FAR};

        private _session_spread session_spread;

        public _session_spread Session_spread
        {
            get { return session_spread; }
            set { session_spread = value; }
        }




    }
}
