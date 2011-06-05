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

		public Session ProcessRow(Session session, MySqlDataReader Reader)
		{
			return this.ProcessRow(session, Reader, 0);
		}

		public Session ProcessRow(Session session, MySqlDataReader Reader, int offset)
		{
			session.Id = Reader.GetInt32(0 + offset);
            session.Daytime_id = Reader.GetInt32(1 + offset);
            session.Classroom_id = Reader.GetInt32(2 + offset);
            session.Pair_id = Reader.GetInt32(3 + offset);
			return session;
		}

        public List<Session> FindAll()
		{
			/*
            List<Session> result = new List<Session>();
			
			Daytime daytime = new Daytime(1, new DateTime(2011, 5, 10), 1330);
			Classroom classroom = new Classroom(1, "OB002");
			User student1 = new Student("Jan", "Piet");
			User student2 = new Student("Piet", "Jan");
			Pair pair = new Pair(1, student1, student2, 12);
			result.Add(new Session(daytime, classroom, pair));

			daytime = new Daytime(2, new DateTime(2011, 5, 10), 1400);
			classroom = new Classroom(2, "OB002");
			student1 = new Student("Boven", "Ibrahim");
			student2 = new Student("Schipper", "Jeroen");
			pair = new Pair(2, student1, student2, 4);
			result.Add(new Session(daytime, classroom, pair));
			return result;
			 * */
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, daytime_id, classroom_id, pair_id FROM session";
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            List<Session> result = new List<Session>();
            while (Reader.Read())
            {
                result.Add(this.ProcessRow(new Session(), Reader));
            }
            this._db.CloseConnection();

			// dummy data
			Daytime daytime = new Daytime(1, new DateTime(2011, 5, 10), 1330);
			Classroom classroom = new Classroom(1, "OB002");
			User student1 = new Student("Jan", "Piet");
			User student2 = new Student("Piet", "Jan");
			Pair pair = new Pair(1, student1, student2, 12);
			Teacher teacher1 = new Teacher();
			Pair_attachment teacher1_attachment = new Pair_attachment(teacher1 ,pair);
			result.Add(new Session(daytime, classroom, pair));
			// end dummy data

            return result;
		}

        public Session Find(int id)
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, daytime_id, classroom_id, pair_id FROM session WHERE id = ?id";
            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = id;
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            Reader.Read();//Only 1 row
            this._db.CloseConnection();
            return this.ProcessRow(new Session(), Reader);
        }
    }
}
