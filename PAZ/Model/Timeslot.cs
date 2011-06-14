using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
    public class Timeslot
    {
        public Timeslot(int id, string time)
		{
			Id = id;
            Time = time;
        }

        public int Id { get; private set; }
        public string Time { get; private set; }
    }
}
