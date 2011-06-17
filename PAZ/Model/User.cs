using System;
using System.Collections.Generic;
using PAZ.Control;
using PAZ.Model.Mappers;
using PAZMySQL;

namespace PAZ.Model
{
    public class User : IComparable<User>
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Surname { get; set; }
        public string Firstname { get; set; }
        public string Email { get; set; }
        public string User_type { get; set; }
        public string Status { get; set; }

        private bool? _wasChanged;
        public bool? WasChanged
        {
            get { return _wasChanged; }
            set 
            {
                if (_wasChanged == null)
                {
                    _wasChanged = value;
                    return;
                }

                if (value != _wasChanged)
                {
                    _wasChanged = value;
                    PAZController.GetInstance().UserMapper.Save(this);
                }

                
            }
        }

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

        public int CompareTo(User right)
        {
            return Firstname.CompareTo(right.Firstname);
        }
    }
}