using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PAZ.Model.Mappers;
using PAZMySQL;

namespace PAZ.Model
{
    public class Session
    {
        private const int MAX_TEACHERS = 2;
        private const int MAX_EXPERTS = 2;

        public int Id { get; set; }
        private Daytime _daytime;
        public int Daytime_id { get; set; }
        public Daytime Daytime
        {
            get
            {
                if (this._daytime == null)
                {
                    this.Daytime = (new DaytimeMapper(MysqlDb.GetInstance())).Find(this.Daytime_id);
                }
                return this._daytime;
            }
            set
            {
                this._daytime = value;
                this.Daytime_id = this._daytime.Id;
            }
        }
        private Classroom _classroom;
		public int Classroom_id {get; set;}
        public Classroom Classroom
        {
            get
            {
                if (this._classroom == null)
                {
                    this.Classroom = (new ClassroomMapper(MysqlDb.GetInstance())).Find(this.Classroom_id);
                }
                return this._classroom;
            }
            set
            {
                this._classroom = value;
                this.Classroom_id = this._classroom.Id;
            }
        }
        private Pair _pair;
		public int Pair_id {get; set;}
        public Pair Pair {
            get {
                if (this._pair == null) {
                    this.Pair = (new PairMapper(MysqlDb.GetInstance())).Find(this.Pair_id);
                }
                return this._pair;
            }
            set {
                this._pair = value;
                this.Pair_id = this._pair.ID;
            }
        }

        public Dictionary<int, Teacher> Teachers
        {
            get
            {
                Dictionary<int, Teacher> result = new Dictionary<int, Teacher>();
                foreach (User user in Pair.Attachments)
                {
                    if (user is Teacher)
                        result.Add(user.Id, (Teacher) user);
                }

                return result;
            }
            set
            {
            }
        }

        public Dictionary<int, Expert> Experts
        {
            get
            {
                Dictionary<int, Expert> result = new Dictionary<int, Expert>();
                foreach (User user in Pair.Attachments)
                {
                    if (user is Expert)
                        result.Add(user.Id, (Expert) user);
                }

                return result;
            }
            set
            {
            }
        }
    }
}