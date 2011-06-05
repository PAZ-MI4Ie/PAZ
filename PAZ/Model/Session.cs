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
        private Classroom _classroom;
		public int Classroom_id;
        private Pair _pair;
		public int Pair_id;

        // 5/6/2011 Yorg: Dit is eigenlijk wel lelijk gedaan zo, maar leraren en experts staan eigenlijk vrij vast, 
        // je wilt in ieder geval niet er je hoofd over breken om meer dan 2 te ondersteunen(YAGNI), maar als je een goede manier weet om dit anders in te vullen, ga er voor! :)
        private Teacher _teacher1;
        private Teacher _teacher2;

        // Idem als bij teachers boven
        private Expert _expert1;
        private Expert _expert2;

		public string Datum
		{
			get
			{
				return _daytime.Date.Day.ToString()
                + "-" + _daytime.Date.Month.ToString()
                + "-" + _daytime.Date.Year.ToString();
			}
		}
		public int Timeslot
		{
            get { return _daytime.Timeslot; }
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
                return _teacher1.Firstname + " " + _teacher1.Surname + "\r"
                    + _teacher2.Firstname + _teacher2.Surname;
			}
		}
		public string Deskundigen
		{
            get
            {
                return _expert1.Firstname + " " + _expert1.Surname + "\r"
                    + _expert2.Firstname + _expert2.Surname;
            }
		}
		public int AantalGasten
		{
            get { return _pair.Number_of_guests; }
		}

		public Session() { }

		public Session(Daytime daytime, Classroom classroom, Pair pair, Teacher teacher1, Teacher teacher2, Expert expert1, Expert expert2)
		{
			_daytime = daytime;
			_classroom = classroom;
			_pair = pair;

            _teacher1 = teacher1;
            _teacher2 = teacher2;

            _expert1 = expert1;
            _expert2 = expert2;

            _dataList = new List<Object>();
            _dataList.Add(Datum);
            _dataList.Add(Timeslot);
            _dataList.Add(Lokaal);
            _dataList.Add(Studenten);
            _dataList.Add(Docenten); 
            _dataList.Add(Deskundigen);
            _dataList.Add(AantalGasten);
		}

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
