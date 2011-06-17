using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
    public class Classroom : IComparable<Classroom>
    {
        public int Id { get; set; }
        public string Room_number { get; set; }

		public Classroom() { }

		public Classroom(int id, string room_number)
		{
			Id = id;
			Room_number = room_number;
		}

        public int CompareTo(Classroom right)
        {
            return Room_number.CompareTo(right.Room_number);
        }
    }
}
