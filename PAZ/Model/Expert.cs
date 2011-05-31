using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
    class Expert : User
    {
        public string Company { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }

		public Expert() : base()
		{

		}
    }
}
