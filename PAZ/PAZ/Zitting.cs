using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ
{
    class Zitting
    {
        private string _datum;

        public string Datum
        {
            get { return _datum; }
            set { _datum = value; }
        }
        private string _tijd;

        public string Tijd
        {
            get { return _tijd; }
            set { _tijd = value; }
        }
        private string _lokaal;

        public string Lokaal
        {
            get { return _lokaal; }
            set { _lokaal = value; }
        }
        private string _leerlingen;

        public string Leerlingen
        {
            get { return _leerlingen; }
            set { _leerlingen = value; }
        }
        private string _docenten;

        public string Docenten
        {
            get { return _docenten; }
            set { _docenten = value; }
        }
        private string _deskundige;

        public string Deskundige
        {
            get { return _deskundige; }
            set { _deskundige = value; }
        }
        private int _aantalGasten;

        public int AantalGasten
        {
            get { return _aantalGasten; }
            set { _aantalGasten = value; }
        }
    }
}
