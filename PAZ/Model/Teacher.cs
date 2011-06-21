using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PAZ.Control;

namespace PAZ.Model
{
    public class Teacher : User
    {
		public enum session_spread { ANY, CLOSE, FAR };

		public session_spread Session_spread { get; set; }

        private List<Pair> _pairs;
        public List<Pair> Pairs
        {
            get
            {
                if (this._pairs == null)
                {
                    PAZController.GetInstance().PairMapper.FindByAttachment(this.Id);
                }
                return this._pairs;
            }

            set
            {
                this._pairs = value;
            }
        }

        private static int tempHack = 0;

		public Teacher()
		{
			this.User_type = "teacher";
		}

		public Teacher(string surname, string firstname)
			: base(surname, firstname)
		{
			this.User_type = "teacher";
            this.Email = "teacher" + (++tempHack) + "@avans.nl";
		}
    }
}
