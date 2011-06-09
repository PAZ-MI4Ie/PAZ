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
        private int CheckScore(Planning planning)
        {
            int score = 0;
            foreach (Session session in planning.Sessions)
            {
                foreach (User user in session.Pair.Participants)
                {
                    bool countedScore = false;
                    foreach (Blocked_timeslot blockedTimeSlot in user.BlockedTimeslots)
                    {
                        if (session.Daytime_id == blockedTimeSlot.Daytime_id)
                        {
                            if (!blockedTimeSlot.Hardblock)
                            {
                                ++score;
                                countedScore = true;
                                break;
                            }
                            else
                            {
                                return -1;
                            }
                        }
                    }
                    if (!countedScore)
                    {
                        score += 2;
                    }
                }
            }
            return score;
        }

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
            Console.WriteLine(this.CheckScore(result));
            return result;
        }
    }
}