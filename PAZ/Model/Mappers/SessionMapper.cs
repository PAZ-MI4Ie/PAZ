using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using PAZ.Model;

namespace PAZMySQL
{
    public class SessionMapper : Mapper
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

		// 6/6/2011 Mark Bos: Uiteindelijk zou deze functie alle sessies uit de database halen, maar voordat dat werkt, staat er nu dummy data in om te zorgen dat er iets in het overzicht staat.
		//De ProcessRow functie hierboven moet dus uitgewerkt worden, maar ik denk dat alleen Teun weet hoe dat moet.
        public List<Session> FindAll()
		{
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

			/* dummy data
			Daytime daytime = new Daytime(1, new DateTime(2011, 5, 10), 1);
			Classroom classroom = new Classroom(1, "OB002");
            Student student1 = new Student("Jan", "Piet", 0000000, "Management en Bestuur");
            Student student2 = new Student("Piet", "Jan", 0000000, "Management en Bestuur");
			Teacher teacher1 = new Teacher("Saris", "Ger");
			Teacher teacher2 = new Teacher("Hogenboom", "Keesjan");
			Expert expert1 = new Expert("Klein", "Aad");
			Expert expert2 = new Expert("Groot", "Ad");
			Pair pair = new Pair(1, student1, student2, 12);

			result.Add(new Session(daytime, classroom, pair, teacher1, teacher2, expert1, expert2));
			// end dummy data*/

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
