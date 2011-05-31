using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PAZMySQL;

namespace PAZ.Model
{
    class Pair
    {
        public int ID { get; set; }
        private User _student1;
        public int Student1_id { get; set; }
        public User Student1 {
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
        private User _student2;
        public int Student2_id { get; set; }
        public User Student2 {
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

        public Pair()
        {

        }

		public Pair(int id, User student1, User student2, int number_of_guests)
		{
			ID = id;
			Student1 = student1;
			Student2 = student2;
			Number_of_guests = number_of_guests;
		}
    }
}
