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
        private const int MAX_TEACHERS = 2;
        private const int MAX_EXPERTS = 2;

		public int Id;
        private Daytime _daytime;
        public int Daytime_id;
        private Classroom _classroom;
		public int Classroom_id;
        private Pair _pair;
		public int Pair_id;

        private Teacher[] _teachers = new Teacher[MAX_TEACHERS];
        private Expert[] _experts = new Expert[MAX_EXPERTS];

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
					+ _pair.Student2.Firstname + " " + _pair.Student2.Surname;
			}
		}
		public string Docenten
		{
			get
			{
                string returnString = String.Empty;
                for (int i = 0; i < _teachers.Length; ++i)
                {
                    // Mogelijk overbodig bij teachers, omdat het aantal precies vast staat op 2.
                    if (_teachers[i] == null)
                        break;

                    returnString += _teachers[i].Firstname + " " + _teachers[i].Surname + "\r";
                }

                return returnString;
			}
		}
		public string Deskundigen
		{
            get
            {
                string returnString = String.Empty;
                for (int i = 0; i < _experts.Length; ++i)
                {
                    // Er kunnen 1 of 2 experts zijn(en misschien ooit wel meer dan 2) maar het kan dus zijn dat er null waardes zijn bij experts.
                    if (_experts[i] == null)
                        break;

                    returnString += _experts[i].Firstname + " " + _experts[i].Surname + "\r";
                }

                return returnString;
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

            _teachers[0] = teacher1;
            _teachers[1] = teacher2;

            _experts[0] = expert1;
            _experts[1] = expert2;

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

        // Idem
        public Teacher[] GetTeachers()
        {
            return _teachers;
        }

        // Idem
        public Expert[] GetExperts()
        {
            return _experts;
        }
       
        // Idem
        public Daytime GetDaytime()
        {
            return _daytime;
        }
    }
}
