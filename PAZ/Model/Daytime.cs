using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
    class Daytime
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Timeslot { get; set; }

		public Daytime(int id, DateTime date, int timeslot)
		{
			Id = id;
			Date = date;
			Timeslot = timeslot;
		}
    }
}
