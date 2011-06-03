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
        public int Id { get; set; }
        private Daytime _daytime;
        public int Daytime_id { get; set; }
        public Daytime Daytime { get; set; }
        private Classroom _classroom;
        public int Classroom_id { get; set; }
        public Classroom Classroom { get; set; }
        private Pair _pair;
        
        public int Pair_id { get; set; }
        public Pair Pair {
            get
            {
                if (this._pair == null)
                {
                    this.Pair = (new PairMapper(MysqlDb.GetInstance())).Find(this.Pair_id);
                }
                return this._pair;
            }
            set
            {
                this._pair = value;
            }
        }
        public string Student1
        {
            get { return Pair.Student1.Firstname + Pair.Student1.Surname; }
        }

		public Session() { }

		public Session(Daytime daytime, Classroom classroom, Pair pair)
		{
			Daytime = daytime;
			Classroom = classroom;
			Pair = pair;
		}

        // TO DO: Zorgen dat deze gevuld wordt!
        private List<Object> _dataList = new List<Object>(); // Gebruikt voor pdf export loop

        // Opmerking: Dit zou een property moeten zijn, maar dan wordt het automatisch in de datagrid geduwd en dat willen we niet
        public List<Object> GetDataList()
        {
            return _dataList;
        }
    }
}
