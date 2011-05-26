using System;
using System.Collections.Generic;

namespace PAZ
{
    public class Zitting
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

        private List<Object> _dataList; // Gebruikt voor pdf export loop

        public Zitting(string datum, string tijd, string lokaal, string leerlingen, string docenten, string deskundige, int aantalGasten)
        {
            _datum = datum;
            _tijd = tijd;
            _lokaal = lokaal;
            _leerlingen = leerlingen;
            _docenten = docenten;
            _deskundige = deskundige;
            _aantalGasten = aantalGasten;

            _dataList = new List<Object>();

            _dataList.Add(_datum);
            _dataList.Add(_tijd);
            _dataList.Add(_lokaal);
            _dataList.Add(_leerlingen);
            _dataList.Add(_docenten);
            _dataList.Add(_deskundige);
            _dataList.Add(_aantalGasten);
        }

        // Opmerking: Dit zou een property moeten zijn, maar dan wordt het automatisch in de datagrid geduwd en dat willen we niet
        public List<Object> GetDataList()
        {
            return _dataList;
        }
    }
}
