using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
    class Daytime
    {

        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private DateTime _date;

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        private int _timeslot;

        public int Timeslot
        {
            get { return _timeslot; }
            set { _timeslot = value; }
        }


    }
}
