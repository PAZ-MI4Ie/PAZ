using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PAZMySQL;

namespace PAZ.Model
{
    public class Blocked_timeslot
    {
        public int Daytime_id { get; set; }
        private Daytime _daytime;
        public Daytime Daytime
        {
            get
            {
                if (this._daytime == null)
                {
                    this.Daytime = (new DaytimeMapper(MysqlDb.GetInstance())).Find(this.Daytime_id);
                }
                return this._daytime;
            }
            set
            {
                this._daytime = value;
            }
        }
        public User User { get; set; }
        public bool Hardblock { get; set; }

		public Blocked_timeslot()
		{
		}

		public Blocked_timeslot(Daytime dayTimeinput, bool blocktype)
		{
			this.Daytime = dayTimeinput;
			this.Hardblock = blocktype;
		}
    }
}