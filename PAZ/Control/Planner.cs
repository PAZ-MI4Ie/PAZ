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
            Planning result = new Planning();

            List<Classroom> classrooms = new ClassroomMapper(MysqlDb.GetInstance()).FindAll();
            List<Daytime> daytimes = new DaytimeMapper(MysqlDb.GetInstance()).FindAll();
            int classroomI = 0;
            int daytimeI = 0;
            foreach (Pair pair in pairs)
            {
                Session session = new Session();
                session.Classroom = classrooms[classroomI];
                session.Daytime = daytimes[daytimeI];
                session.Pair = pair;
                result.Sessions.Add(session);
                Console.WriteLine(pair.Student1.Firstname + " en " + pair.Student2.Firstname + " at " + session.Daytime.Date + " slot " + session.Daytime.Timeslot + " in " + session.Classroom.Room_number);
                ++classroomI;
                if (classroomI == classrooms.Count)
                {
                    classroomI = 0;
                    ++daytimeI;
                }
            }
            Console.WriteLine(result.GetScore());
            return result;
        }
    }
}