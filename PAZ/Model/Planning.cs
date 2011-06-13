using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
    /* 
     * TODO: alles
     */
    public class Planning
    {
        private List<Session> _sessions;
        public List<Session> Sessions
        {
            get
            {
                if (this._sessions == null)
                {
                    this.Sessions = new List<Session>();
                }
                return this._sessions;
            }
            set
            {
                this._sessions = value;
            }
        }

        public int GetScore()
        {
            int score = 0;
            foreach (Session session in this.Sessions)
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
    }
}