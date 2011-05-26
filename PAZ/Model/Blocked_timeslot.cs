using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
    class Blocked_timeslot
    {
        private Daytime _id;

        internal Daytime Id
        {
            get { return _id; }
            set { _id = value; }
        }


        private User _user;

        internal User User
        {
            get { return _user; }
            set { _user = value; }
        }


        private int _hardblock;

        public int Hardblock
        {
            get { return _hardblock; }
            set { _hardblock = value; }
        }

    }
}
