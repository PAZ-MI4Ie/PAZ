using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
    class Pair_attachment
    {
        public User user { get; set; }
        public Pair pair { get; set; }

		public Pair_attachment(User user, Pair pair)
		{
			this.user = user;
			this.pair = pair;
		}
    }
}
