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
			

			return session;
		}

        public Session Find(int id)
        {
			return null;
        }

        public List<Session> FindAll()
		{
			this._db.OpenConnection();
			MySqlCommand command = this._db.CreateCommand();
			command.CommandText = "SELECT id, username, firstname, surname, email, user_type, status, studentnumber, study FROM user, student WHERE user.id = student.user_id";
			MySqlDataReader Reader = this._db.ExecuteCommand(command);
			List<Session> result = new List<Session>();
			while (Reader.Read())
			{
				result.Add(this.ProcessRow(new Session(), Reader));
			}
			this._db.CloseConnection();
			return result;
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
		}
    }
}
