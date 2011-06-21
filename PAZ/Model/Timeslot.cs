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
			this.Id = id;
            this.Time = time;
        }

        public int Id { get; set; }
        public string Time { get; set; }
    }
}
