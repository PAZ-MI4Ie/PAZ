using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
    public class LetterTemplate
    {
        public LetterTemplate(int id, string kenmerk, string contactpersonen, string telefoon, string email, string avans_locatie, string begin_kern, string reis_informatie, string verdere_informatie, string afzenders, string bijlagen, string voettekst_links, string voettekst_center, string voettekst_rechts)
        {
            Id = id;
            Kenmerk = kenmerk;
            Contactpersonen = contactpersonen;
            Telefoon = telefoon;
            Email = email;
            AvansLocatie = avans_locatie;
            BeginKern = begin_kern;
            ReisInformatie = reis_informatie;
            VerdereInformatie = verdere_informatie;
            Afzenders = afzenders;
            Bijlagen = bijlagen;
            VoettekstLinks = voettekst_links;
            VoettekstCenter = voettekst_center;
            VoettekstRechts = voettekst_rechts;
        }

        public int Id { get; private set; }
        public string Kenmerk { get; private set; }
        public string Contactpersonen { get; private set; }
        public string Telefoon { get; private set; }
        public string Email { get; private set; }
        public string AvansLocatie { get; private set; }
        public string BeginKern { get; private set; }
        public string ReisInformatie { get; private set; }
        public string VerdereInformatie { get; private set; }
        public string Afzenders { get; private set; }
        public string Bijlagen { get; private set; }
        public string VoettekstLinks { get; private set; }
        public string VoettekstCenter { get; private set; }
        public string VoettekstRechts { get; private set; }
    }
}
