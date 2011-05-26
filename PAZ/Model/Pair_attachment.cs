using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
    class Pair_attachment
    {

        private User _user;

        internal User User
        {
            get { return _user; }
            set { _user = value; }
        }


        private Pair _pair;

        internal Pair Pair
        {
            get { return _pair; }
            set { _pair = value; }
        }


    }
}
