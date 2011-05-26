using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
    class Pair
    {


        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private User _student1;

        internal User Student1
        {
            get { return _student1; }
            set { _student1 = value; }
        }

        private User _student2;

        internal User Student2
        {
            get { return _student2; }
            set { _student2 = value; }
        }

        private int _number_of_guests;

        public int Number_of_guests
        {
            get { return _number_of_guests; }
            set { _number_of_guests = value; }
        }
    }
}
