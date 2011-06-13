using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
    public class Daytime
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Timeslot { get; set; }
        public string Time
        {
            get
            {
                // Deze tijden komen uit de Planning Zittingen.xslx op BB, het invoeren hiervan zou nog anders moeten als deze tijden instelbaar zijn...
                // TO DO: fix this shit :)
                switch (Timeslot)
                {
                    case 1:
                        return "09.00";
                    
                    case 2:
                        return "11.00";

                    case 3:
                        return "13.30";

                    case 4:
                        return "15.50";

                    default:
                        return "error";
                }
            }
        }

        public Daytime()
        {
        }

		public Daytime(int id, DateTime date, int timeslot)
		{
			Id = id;
			Date = date;
			Timeslot = timeslot;
		}
    }
}
