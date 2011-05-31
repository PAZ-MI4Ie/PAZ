using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
    class Session
    {
        public int Id { get; set; }
        public Daytime Daytime { get; set; }
        public Classroom Classroom { get; set; }
        public Pair Pair { get; set; }

		public Session() { }

		public Session(Daytime daytime, Classroom classroom, Pair pair)
		{
			Daytime = daytime;
			Classroom = classroom;
			Pair = pair;
		}
    }
}
