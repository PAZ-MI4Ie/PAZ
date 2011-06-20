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

            List<Classroom> classrooms = PAZController.GetInstance().ClassroomMapper.FindAll();
            List<Daytime> daytimes = PAZController.GetInstance().DaytimeMapper.FindAll();
            Dictionary<Daytime, List<Classroom>> available = new Dictionary<Daytime,List<Classroom>>();
            Dictionary<Daytime, List<Expert>> availableExperts = new Dictionary<Daytime, List<Expert>>();

            List<Expert> allExperts = PAZController.GetInstance().ExpertMapper.FindAll();
            foreach (Daytime daytime in daytimes)
            {
                available.Add(daytime, new List<Classroom>(classrooms));//Clone :)
                List<Expert> expertsToAdd = new List<Expert>();
                foreach (Expert expert in allExperts)
                {
                    bool add = true;
                    foreach (Blocked_timeslot bs in expert.BlockedTimeslots)
                    {
                        if (bs.Hardblock && bs.Daytime_id == daytime.Id)
                        {
                            add = false;
                            break;
                        }
                    }
                    if (add)
                    {
                        expertsToAdd.Add(expert);
                    }
                }
                availableExperts.Add(daytime, expertsToAdd);
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

                if (!session.Pair.HasExpert())
                {
                    List<Expert> availableThisSlot = availableExperts.ElementAt(daytimeI).Value;
                    if (availableThisSlot.Count > 0)
                    {
                        session.Pair.Attachments.Add(availableThisSlot.ElementAt(0));
                        availableThisSlot.RemoveAt(0);
                        PAZController.GetInstance().PairMapper.Save(session.Pair);
                    }
                }

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