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
    }
}