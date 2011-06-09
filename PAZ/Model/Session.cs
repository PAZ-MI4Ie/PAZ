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

		public Session() { }

		public Session(Daytime daytime, Classroom classroom, Pair pair, Teacher teacher1, Teacher teacher2, Expert expert1, Expert expert2)
		{
			_daytime = daytime;
			_classroom = classroom;
			_pair = pair;
            this._temp_experts = new Expert[2];
            this._temp_experts[0] = expert1;
            this._temp_experts[1] = expert2;
            this._temp_teachers = new Teacher[2];
            this._temp_teachers[0] = teacher1;
            this._temp_teachers[1] = teacher2;
		}

        //TEMP CODE:
        private Expert[] _temp_experts;
        private Teacher[] _temp_teachers;

        public Expert[] GetExperts()
        {
            return _temp_experts;
        }

        public Teacher[] GetTeachers()
        {
            return _temp_teachers;
        }
    }
}