using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PAZMySQL;
using PAZ.Model.Mappers;

namespace PAZ.Model
{
    class Pair
    {
        public int ID { get; set; }
        private Student _student1;
        public int Student1_id { get; set; }
        public Student Student1
        {
            get
            {
                if (this._student1 == null)
                {
                    this.Student1 = (new StudentMapper(MysqlDb.GetInstance())).Find(this.Student1_id);
                }
                return this._student1;
            }
            set
            {
                this._student1 = value;
            }
        }
        private Student _student2;
        public int Student2_id { get; set; }
        public Student Student2
        {
            get
            {
                if (this._student2 == null)
                {
                    this.Student2 = (new StudentMapper(MysqlDb.GetInstance())).Find(this.Student2_id);
                }
                return this._student2;
            }
            set
            {
                this._student2 = value;
            }
        }
        public int Number_of_guests { get; set; }

        public List<User> Attachments
        {
            get
            {
                List<User> result = new List<User>();
                foreach (KeyValuePair<int, string> pair in (new PairMapper(MysqlDb.GetInstance())).FindAttachments(this.ID))
                {
                    if (pair.Value.Equals("teacher"))
                    {
                        result.Add((new TeacherMapper(MysqlDb.GetInstance())).Find(pair.Key));
                    }
                    else if (pair.Value.Equals("expert"))
                    {
                        result.Add((new ExpertMapper(MysqlDb.GetInstance())).Find(pair.Key));
                    }
                }
                return result;
            }
            set
            {

            }
        }

        public List<User> Participants
        {
            get
            {
                List<User> result = this.Attachments;
                result.Add(this.Student1);
                result.Add(this.Student2);
                return result;
            }
        }

		public Pair() { }

        public Pair(int id, Student student1, Student student2, int number_of_guests)
		{
			ID = id;
			Student1 = student1;
			Student2 = student2;
			Number_of_guests = number_of_guests;
		}
    }
}