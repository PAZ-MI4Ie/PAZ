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
		public Classroom Classroom;
        private Pair _pair;
		public int Pair_id;
		public Pair Pair;
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
			get { return Classroom.Room_number; }
		}
		public string Studenten
		{
			get
			{
				return Pair.Student1.Firstname + " " + Pair.Student1.Surname + "\r\n"
					+ Pair.Student2.Firstname + Pair.Student2.Surname;
			}
		}
		public string Docenten
		{
			get
			{
				return "";
			}
		}
		public string Deskundige
		{
			get
			{
				return "";
			}
		}
		public int AantalGasten
		{
			get { return Pair.Number_of_guests; }
		}

		private List<Object> _dataList; // Gebruikt voor pdf export loop

		public Session() { }

		public Session(Daytime daytime, Classroom classroom, Pair pair)
		{
			Daytime = daytime;
			Classroom = classroom;
			Pair = pair;
		}
    }
}
