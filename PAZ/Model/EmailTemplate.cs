using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
    public class EmailTemplate
    {
        public EmailTemplate(int id, string displayname, string inleiding, string informatie, string afsluiting, string afzenders)
        {
            Id = id;
            Displayname = displayname;
            Inleiding = inleiding;
            Informatie = informatie;
            Afsluiting = afsluiting;
            Afzenders = afzenders;
        }

        public int Id { get; private set; }
        public string Displayname { get; private set; }
        public string Inleiding { get; private set; }
        public string Informatie { get; private set; }
        public string Afsluiting { get; private set; }
        public string Afzenders { get; private set; }
    }
}
