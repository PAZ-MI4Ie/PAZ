using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
    class Expert
    {

        private User _id;

        internal User Id
        {
            get { return _id; }
            set { _id = value; }
        }


        private string _company;

        public string Company
        {
            get { return _company; }
            set { _company = value; }
        }


        private string _address;

        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }


        private string _postcode;

        public string Postcode
        {
            get { return _postcode; }
            set { _postcode = value; }
        }

    }
}
