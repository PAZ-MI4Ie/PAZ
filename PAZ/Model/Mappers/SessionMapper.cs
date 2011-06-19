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

        public void Save(Session session)
        {
            Boolean insert = true;
            if (session.Id != 0)
            {
                insert = false;
            }
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            if (insert)
            {
                command.CommandText = "INSERT INTO session (daytime_id, classroom_id, pair_id) VALUES " +
                "(?daytime_id, ?classroom_id, ?pair_id)";
            }
            else
            {
                command.CommandText = "UPDATE session SET daytime_id = ?daytime_id, classroom_id = ?classroom_id, pair_id = ?pair_id WHERE id = ?id";
            }

            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.String)).Value = session.Id;
            command.Parameters.Add(new MySqlParameter("?daytime_id", MySqlDbType.Int32)).Value = session.Daytime.Id;
            command.Parameters.Add(new MySqlParameter("?classroom_id", MySqlDbType.Int32)).Value = session.Classroom.Id;
            command.Parameters.Add(new MySqlParameter("?pair_id", MySqlDbType.String)).Value = session.Pair.ID;
            this._db.ExecuteCommand(command);
            this._db.CloseConnection();
        }

        public void Save(Planning planning)
        {
            this.Save(planning, false);
        }

        public void Save(Planning planning, bool deleteOld)
        {
            if (deleteOld)
            {
                this._db.OpenConnection();
                MySqlCommand command = this._db.CreateCommand();
                command.CommandText = "DELETE FROM session WHERE 1=1";
                this._db.ExecuteCommand(command);
                this._db.CloseConnection();
            }
            foreach (Session session in planning.Sessions)
            {
                this.Save(session);
            }
        }
    }
}
