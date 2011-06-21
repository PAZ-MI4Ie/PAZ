using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAZ.Model
{
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
                int tempscore = session.Pair.ScoreAt(session.Daytime);
                if (tempscore == -1) {
                    return tempscore;
                } else {
                    score += tempscore;
                }
            }
            return score;
        }
    }
}