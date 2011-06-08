using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PAZ.Model.Mappers;
using PAZMySQL;

namespace PAZ.Model
{
    class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Surname { get; set; }
        public string Firstname { get; set; }
        public string Email { get; set; }
        public string User_type { get; set; }
        public string Status { get; set; }
        private List<Blocked_timeslot> _blockedTimeslots;
        public List<Blocked_timeslot> BlockedTimeslots
        {
            get
            {
                if (this._blockedTimeslots == null)
                {
                    this.BlockedTimeslots = new BlockedTimeslotMapper(MysqlDb.GetInstance()).FindByUserId(this.Id);
                }
                return this._blockedTimeslots;
            }
            set
            {
                this._blockedTimeslots = value;
            }
        }

		public User()
		{
			Status = "accepted";
			Username = " ";
		}

		public User(string surname, string firstname)
		{
			Surname = surname;
			Firstname = firstname;
			Status = "accepted";
			Username = surname;
		}
    }
}