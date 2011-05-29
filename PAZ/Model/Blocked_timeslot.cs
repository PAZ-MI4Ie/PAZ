using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
    class Blocked_timeslot
    {
        public Daytime Id { get; set; }
        public User User { get; set; }
        public int Hardblock { get; set; }
    }
}
