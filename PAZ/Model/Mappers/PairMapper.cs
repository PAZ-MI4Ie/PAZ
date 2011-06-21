using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PAZMySQL;
using MySql.Data.MySqlClient;
using PAZ.Control;

namespace PAZ.Model.Mappers
{
    public class PairMapper : Mapper
    {
        public PairMapper(MysqlDb db)
            : base(db)
        {

        }

        protected Pair ProcessRow(Pair pair, MySqlDataReader Reader)
        {
            pair.ID = Reader.GetInt32(0);
            pair.Number_of_guests = Reader.GetInt32(1);
            StudentMapper studentmapper = new StudentMapper(MysqlDb.GetInstance());
            pair.Student1_id = Reader.GetInt32(2);
            pair.Student2_id = Reader.GetInt32(3);
            return pair;
        }

        public void Save(Pair pair)
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            if (pair.ID != 0)
            {
                command.CommandText = "UPDATE pair SET number_of_guests=?number_of_guests, student1=?student1, student2=?student2 WHERE id = ?id";
                command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = pair.ID;
            }
            else
            {
                command.CommandText = "INSERT INTO pair (number_of_guests, student1, student2) VALUES " +
                "(?number_of_guests, ?student1, ?student2)";
            }
            command.Parameters.Add(new MySqlParameter("?number_of_guests", MySqlDbType.Int32)).Value = pair.Number_of_guests;
            command.Parameters.Add(new MySqlParameter("?student1", MySqlDbType.Int32)).Value = pair.Student1_id;
            command.Parameters.Add(new MySqlParameter("?student2", MySqlDbType.Int32)).Value = pair.Student2_id;
            this._db.ExecuteCommand(command);
            this._db.CloseConnection();
            if (pair.ID == 0)
            {
                this._db.OpenConnection();
                MySqlCommand command2 = this._db.CreateCommand();
                command2.CommandText = "SELECT LAST_INSERT_ID()";
                MySqlDataReader Reader = this._db.ExecuteCommand(command2);
                Reader.Read();
                this._db.CloseConnection();
                pair.ID = Reader.GetInt32(0);
            }

            this._db.OpenConnection();
            MySqlCommand command3 = this._db.CreateCommand();
            command3.CommandText = "DELETE FROM pair_attachment WHERE pair_id = ?pair_id";
            command3.Parameters.Add(new MySqlParameter("?pair_id", MySqlDbType.Int32)).Value = pair.ID;
            this._db.ExecuteCommand(command3);
            this._db.CloseConnection();
            List<int> had = new List<int>();
            foreach (User attachment in pair.Attachments)
            {
                if (!had.Contains(attachment.Id))
                {
                    this._db.OpenConnection();
                    MySqlCommand command4 = this._db.CreateCommand();
                    command4.CommandText = "INSERT INTO pair_attachment (user_id, pair_id) VALUES (?user_id, ?pair_id)";
                    command4.Parameters.Add(new MySqlParameter("?user_id", MySqlDbType.Int32)).Value = attachment.Id;
                    command4.Parameters.Add(new MySqlParameter("?pair_id", MySqlDbType.Int32)).Value = pair.ID;
                    this._db.ExecuteCommand(command4);
                    had.Add(attachment.Id);
                    this._db.CloseConnection();
                }
            }
        }

        public Pair Find(int id)
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, number_of_guests, student1, student2 FROM pair WHERE pair.id = ?id";
            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = id;
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            Reader.Read();//Only 1 row
            this._db.CloseConnection();
            return this.ProcessRow(new Pair(), Reader);
        }

        public List<Pair> FindAll()
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, number_of_guests, student1, student2 FROM pair";
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            List<Pair> result = new List<Pair>();
            while (Reader.Read())
            {
                result.Add(this.ProcessRow(new Pair(), Reader));
            }
            this._db.CloseConnection();
            return result;
        }

        public List<Pair> FindAllUnplanned()
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, number_of_guests, student1, student2 FROM pair WHERE id NOT IN ( SELECT pair_id FROM SESSION )";
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            List<Pair> result = new List<Pair>();
            while (Reader.Read())
            {
                result.Add(this.ProcessRow(new Pair(), Reader));
            }
            this._db.CloseConnection();
            return result;
        }

        public Dictionary<int, string> FindAttachments(int pairId)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT user_id, user_type FROM pair_attachment LEFT JOIN user ON user.id = user_id WHERE pair_id = ?id";
            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = pairId;
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            while (Reader.Read())
            {
                result.Add(Reader.GetInt32(0), Reader.GetString(1));
            }
            this._db.CloseConnection();
            return result;
        }

        public List<Pair> FindByAttachment(int attachmentId)
        {
            List<Pair> result = new List<Pair>();
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT pair_id FROM pair_attachment WHERE user_id = ?id";
            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = attachmentId;
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            
            while (Reader.Read())
            {
                result.Add(this.Find(Reader.GetInt32(0)));
            }
            this._db.CloseConnection();
            return result;
        }
    }
}