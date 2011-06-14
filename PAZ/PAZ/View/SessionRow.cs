using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PAZ.Model;

namespace PAZ.View
{
    public class SessionRow
    {
        public SessionRow(Session session)
        {
            if (session.Daytime != null)
            {
                this.Datum = session.Daytime.Date.Day.ToString()
                    + "-" + session.Daytime.Date.Month.ToString()
                    + "-" + session.Daytime.Date.Year.ToString();
                this.Timeslot = session.Daytime.Timeslot;
            }
            else
            {
                this.Datum = "Onbekend";
                this.Timeslot = 0;
            }
            if (session.Classroom != null)
            {
                this.Lokaal = session.Classroom.Room_number;
            }
            else
            {
                this.Lokaal = "";
            }
            this.Studenten = session.Pair.Student1.Firstname + " " + session.Pair.Student1.Surname + "\r"
                    + session.Pair.Student2.Firstname + " " + session.Pair.Student2.Surname;

            string returnStringTeachers = String.Empty;
            string returnStringExperts = String.Empty;
           
            if (session.Pair.Attachments != null)
            {
                foreach (User user in session.Pair.Attachments)
                {
                    if (user is Teacher)
                    {
                        Teacher teacher = (Teacher)user;
                        returnStringTeachers += teacher.Firstname + " " + teacher.Surname + "\r";
                    }
                    else if (user is Expert)
                    {
                        Expert expert = (Expert)user;
                        returnStringExperts += expert.Firstname + " " + expert.Surname + "\r";
                    }
                }
            }
            
            this.Docenten = returnStringTeachers;
            this.Deskundigen = returnStringExperts;
            this.AantalGasten = session.Pair.Number_of_guests;

            _dataList = new List<Object>();
            _dataList.Add(Datum);
            _dataList.Add(Timeslot);
            _dataList.Add(Lokaal);
            _dataList.Add(Studenten);
            _dataList.Add(Docenten);
            _dataList.Add(Deskundigen);
            _dataList.Add(AantalGasten);
            this._session = session;
        }
        private List<Object> _dataList; // Gebruikt voor pdf export loop
        private Session _session;

        public List<Object> GetDataList()
        {
            return this._dataList;
        }

        public Session GetSessionModel()
        {
            return this._session;
        }

        public string Datum { get; set; }
        public int Timeslot { get; set; }
        public string Lokaal { get; set; }
        public string Studenten { get; set; }
        public string Docenten { get; set; }
        public string Deskundigen { get; set; }
        public int AantalGasten { get; set; }
    }
}