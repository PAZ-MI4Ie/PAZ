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
        public string City { get; set; }

		public Expert()
		{
			this.User_type = "expert";
		}

		public Expert(string surname, string firstname)
			: base(surname, firstname)
		{
			this.User_type = "expert";

            // Placeholders voor brief aan experts
            this.Company = "<bedrijf>";
            this.Address = "<adres>";
            this.Postcode = "<pc / ";
            this.City = "woonplaats>";
		}
    }
}