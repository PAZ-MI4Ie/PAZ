using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PAZ.Model.Mappers;
using PAZMySQL;

namespace PAZ.Model
{
    class Session
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
                return this._daytime;
            }
            set
            {
                this._daytime = value;
            }
        }
        private Classroom _classroom;
		public int Classroom_id {get; set;}
        public Classroom Classroom
        {
            get
            {
                return this._classroom;
            }
            set
            {
                this._classroom = value;
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
            }
        }

		public Session() { }

		public Session(Daytime daytime, Classroom classroom, Pair pair, Teacher teacher1, Teacher teacher2, Expert expert1, Expert expert2)
		{
			_daytime = daytime;
			_classroom = classroom;
			_pair = pair;
		}

        public Expert[] GetExperts()
        {
            return new Expert[0];
        }

        public Teacher[] GetTeachers()
        {
            return new Teacher[0];
        }
    }
}