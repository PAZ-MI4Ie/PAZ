using System.Collections.Generic;
using MySql.Data.MySqlClient;
using PAZMySQL;

namespace PAZ.Model.Mappers
{
    public class TimeslotMapper : Mapper
    {
        public TimeslotMapper(MysqlDb db)
            : base(db)
        {

        }

        public Timeslot Find(int id)
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, time FROM timeslot WHERE id = ?id";
            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = id;
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            Reader.Read();//Only 1 row
            this._db.CloseConnection();

            int index = -1;
            return new Timeslot(Reader.GetInt32(++index), Reader.GetString(++index));
        }

        public List<Timeslot> FindAll()
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, time FROM timeslot";
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            List<Timeslot> result = new List<Timeslot>();
            while (Reader.Read())
            {
                int index = -1;
                result.Add(new Timeslot(Reader.GetInt32(++index), Reader.GetString(++index)));
            }
            this._db.CloseConnection();
            return result;
        }

        public void Save(Timeslot timeslot)
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();

            command.CommandText = "UPDATE timeslot SET time=?time WHERE id = ?id";

            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = timeslot.Id;
            command.Parameters.Add(new MySqlParameter("?time", MySqlDbType.String)).Value = timeslot.Time;

            this._db.ExecuteCommand(command);
            this._db.CloseConnection();
        }
    }
}
