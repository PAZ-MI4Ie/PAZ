using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
    class Session
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        

        

        private Daytime _daytime;

        public Daytime Daytime
        {
            get { return _daytime; }
            set { _daytime = value; }
        }




        private Classroom _classroom;

        public Classroom Classroom
        {
            get { return _classroom; }
            set { _classroom = value; }
        }





        private Pair _pair;

        public Pair Pair
        {
            get { return _pair; }
            set { _pair = value; }
        }



    }
}
