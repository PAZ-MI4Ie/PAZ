using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PAZ.Model;

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
                    foreach (Blocked_timeslot blockedTimeSlot in user.BlockedTimeslots)
                    {
                        if (session.Daytime_id == blockedTimeSlot.Daytime_id)
                        {
                            if (!blockedTimeSlot.Hardblock)
                            {
                                ++score;
                                break;
                            }
                            else
                            {
                                return -1;
                            }
                        }
                        else
                        {
                            score += 2;
                            break;
                        }
                    }
                }
            }
            return score;
        }

        public Planning Plan()
        {
            return null;
        }
    }
}