using System;
using PAZ.Model.Mappers;
using PAZMySQL;

namespace PAZ.Model
{
    public class Daytime
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Timeslot { get; set; }
        public string Starttime
        {
            get
            {
                // Lang en lelijk D:
                return new TimeslotMapper(MysqlDb.GetInstance()).Find(this.Timeslot).Time.Split(new char[] { '-' })[0];
            }
        }

        public string StartEndTime
        {
            get
            {
                return new TimeslotMapper(MysqlDb.GetInstance()).Find(this.Timeslot).Time;
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
