using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PAZ.Model;
using PAZMySQL;

namespace PAZ.Control
{
    class Planner
    {
        public Planning Plan(List<Pair> pairs)
        {
            Random r = new Random();
            Planning result = new Planning();

            List<Classroom> classrooms = new ClassroomMapper(MysqlDb.GetInstance()).FindAll();
            List<Daytime> daytimes = new DaytimeMapper(MysqlDb.GetInstance()).FindAll();
            Dictionary<Daytime, List<Classroom>> available = new Dictionary<Daytime,List<Classroom>>();
            foreach (Daytime daytime in daytimes)
            {
                available.Add(daytime, new List<Classroom>(classrooms));//Clone :)
            }

            foreach (Pair pair in pairs)
            {
                Session session = new Session();
                session.Pair = pair;
                int daytimeI = r.Next(0, available.Count - 1);
                while (session.Pair.ScoreAt(available.ElementAt(daytimeI).Key) == -1 && available.ElementAt(daytimeI).Value.Count < 1)
                {
                    daytimeI = r.Next(0, available.Count - 1);
                }
                session.Daytime = available.ElementAt(daytimeI).Key;

                int classRoomI = r.Next(0, available.ElementAt(daytimeI).Value.Count-1);
                session.Classroom = available.ElementAt(daytimeI).Value.ElementAt(classRoomI);
                available[session.Daytime].RemoveAt(classRoomI);
                result.Sessions.Add(session);
                Console.WriteLine(pair.Student1.Firstname + " en " + pair.Student2.Firstname + " at " + session.Daytime.Date + " slot " + session.Daytime.Timeslot + " in " + session.Classroom.Room_number);
            }
            Console.WriteLine(result.GetScore());
            return result;
        }
    }
}