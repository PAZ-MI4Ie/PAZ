using System;
using PAZ.Model.Mappers;
using PAZMySQL;
using PAZ.Control;

namespace PAZ.Model
{
    public class Daytime
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Timeslot { get; set; }

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
			this.Id = id;
			this.Date = date;
			this.Timeslot = timeslot;
		}

        public string GetStarttime()
        {
            return PAZController.GetInstance().TimeslotMapper.Find(this.Timeslot).Time.Split(new char[] { '-' })[0];
        }
    }
}
