using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
    class Teacher : User
    {
        public enum session_spread {ANY,CLOSE,FAR};

        public session_spread Session_spread { get; set; }

		public Teacher() : base()
		{

		}
    }
}
