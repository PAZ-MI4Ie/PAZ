using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
    class Pair
    {
        public int ID { get; set; }
        public User Student1 { get; set; }
        public User Student2 { get; set; }
        public int Number_of_guests { get; set; }
    }
}
