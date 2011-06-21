using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PAZ.Control;

namespace PAZ.Model
{
    public class Expert : User
    {
        public string Company { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
		public string City { get; set; }
		public string Telephone { get; set; }
        
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