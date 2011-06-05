using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PAZ.Model.Mappers;
using PAZMySQL;

namespace PAZ.Model
{
    class Session
    {
		public int Id;
        private Daytime _daytime;
        public int Daytime_id;
		public Daytime Daytime;
        private Classroom _classroom;
		public int Classroom_id;
        private Pair _pair;
		public int Pair_id;
		public Pair_attachment attachment;
		
		public string Datum
		{
			get
			{
				return Daytime.Date.Day.ToString()
				+ "-" + Daytime.Date.Month.ToString()
				+ "-" + Daytime.Date.Year.ToString();
			}
		}
		public int Timeslot
		{
			get { return Daytime.Timeslot; }
		}
		public string Lokaal
		{
			get { return _classroom.Room_number; }
		}
		public string Studenten
		{
			get
			{
                return _pair.Student1.Firstname + " " + _pair.Student1.Surname + "\r"
                    + _pair.Student2.Firstname + _pair.Student2.Surname;
			}
		}
		public string Docenten
		{
			get
			{
				return "";
			}
		}
		public string Deskundigen
		{
			get
			{
				return "";
			}
		}
		public int AantalGasten
		{
            get { return _pair.Number_of_guests; }
		}

		public Session() { }

		public Session(Daytime daytime, Classroom classroom, Pair pair)
		{
			Daytime = daytime;
			_classroom = classroom;
			_pair = pair;

            _dataList = new List<Object>();
            _dataList.Add(Datum);
            _dataList.Add(Timeslot);
            _dataList.Add(Lokaal);
            _dataList.Add(Studenten);
            _dataList.Add(Docenten); 
            _dataList.Add(Deskundigen);
            _dataList.Add(AantalGasten);
		}

        // TO DO: Zorgen dat deze gevuld wordt!
        private List<Object> _dataList; // Gebruikt voor pdf export loop

        // Opmerking: Dit zou een property moeten zijn, maar dan wordt het automatisch in de datagrid geduwd en dat willen we niet
        public List<Object> GetDataList()
        {
            return _dataList;
        }

        // Idem als bovenstaande opmerking
        public Pair GetPair()
        {
            return _pair;
        }
    }
}
