using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PAZ.Model;
using PAZMySQL;
using Ini;

namespace PAZ.Control
{
    class Planner
    {
        public Planning Plan(List<Pair> pairs)
        {
            IniFile ini = PAZController.GetInstance().IniReader;
            int max_tries = Int32.Parse(ini["AUTOPLANNERSETTINGS"]["max_tries"]);
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
                int daytimeI = r.Next(0, available.Count);
                int tries = 0;
                while (session.Pair.ScoreAt(available.ElementAt(daytimeI).Key) == -1 && available.ElementAt(daytimeI).Value.Count < 1 && tries < max_tries)
                {
                    daytimeI = r.Next(0, available.Count);
                    ++tries;
                }
                session.Daytime = available.ElementAt(daytimeI).Key;

                int classRoomI = r.Next(0, available.ElementAt(daytimeI).Value.Count);
                session.Classroom = available.ElementAt(daytimeI).Value.ElementAt(classRoomI);
                available[session.Daytime].RemoveAt(classRoomI);
                result.Sessions.Add(session);
            }
            if (result.GetScore() == -1)
            {
                return this.Plan(pairs);
            }
            return result;
        }
    }
}