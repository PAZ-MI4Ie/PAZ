using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
    class Pair
    {
        public int ID { get; set; }
        public User Student1 { get; set; }
        public User Student2 { get; set; }
        public int Number_of_guests { get; set; }

		public Pair() { }

		public Pair(int id, User student1, User student2, int number_of_guests)
		{
			ID = id;
			Student1 = student1;
			Student2 = student2;
			Number_of_guests = number_of_guests;
		}
    }
}
