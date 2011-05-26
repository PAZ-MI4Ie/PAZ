using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
    class Classroom
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _room_number;

        public string Room_number
        {
            get { return _room_number; }
            set { _room_number = value; }
        }


    }
}
