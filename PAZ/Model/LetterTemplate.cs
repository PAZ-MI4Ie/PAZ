using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
    public class LetterTemplate
    {
        public LetterTemplate(int id, string kenmerk, string contactpersonen, string telefoon, string email, string avans_adres, string avans_locatie, string begin_kern, string reis_informatie, string verdere_informatie, string afzenders, string bijlagen, string voettekst_links, string voettekst_center, string voettekst_rechts)
        {
            Id = id;
            Kenmerk = kenmerk;
            Contactpersonen = contactpersonen;
            Telefoon = telefoon;
            Email = email;
            AvansAdres = avans_adres;
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

        public int Id { get; set; }
        public string Kenmerk { get; set; }
        public string Contactpersonen { get; set; }
        public string Telefoon { get; set; }
        public string Email { get; set; }
        public string AvansAdres { get; set; }
        public string AvansLocatie { get; set; }
        public string BeginKern { get; set; }
        public string ReisInformatie { get; set; }
        public string VerdereInformatie { get; set; }
        public string Afzenders { get; set; }
        public string Bijlagen { get; set; }
        public string VoettekstLinks { get; set; }
        public string VoettekstCenter { get; set; }
        public string VoettekstRechts { get; set; }
    }
}
