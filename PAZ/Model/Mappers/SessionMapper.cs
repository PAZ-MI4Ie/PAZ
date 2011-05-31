using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using PAZ.Model;

namespace PAZMySQL
{
    class SessionMapper : Mapper
    {
        public SessionMapper(MysqlDb db) : base(db)
        {
            
        }

        private Session ProcessRow(MySqlDataReader Reader)
        {
			return null;
        }

        public Session Find(int id)
        {
			return null;
        }

        public List<Session> FindAll()
        {
            List<Session> result = new List<Session>();
			
			Daytime daytime = new Daytime(1, new DateTime(2011, 5, 10), 1330);
			Classroom classroom = new Classroom(1, "OB002");
			User student1 = new User("Jan", "Piet");
			User student2 = new User("Piet", "Jan");
			Pair pair = new Pair(1, student1, student2, 12);
			result.Add(new Session(daytime, classroom, pair));

			daytime = new Daytime(2, new DateTime(2011, 5, 10), 1400);
			classroom = new Classroom(2, "OB002");
			student1 = new User("Boven", "Ibrahim");
			student2 = new User("Schipper", "Jeroen");
			pair = new Pair(2, student1, student2, 4);
			result.Add(new Session(daytime, classroom, pair));
			/*
				"10-5-2011",
				"14:00",
				"OB002",
				"Ibrahim Boven\nJeroen Schipper",
				"Freek Hogenboom\nSjaak Lauris",
				"Kees Prof 2 \n Piet Hogensluiter",
				4
			 * */
			return result;
        }
    }
}
