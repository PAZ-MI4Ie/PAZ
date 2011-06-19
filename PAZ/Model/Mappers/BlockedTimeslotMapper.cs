using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PAZMySQL;
using MySql.Data.MySqlClient;

namespace PAZ.Model.Mappers
{
    class BlockedTimeslotMapper : Mapper
    {
        public BlockedTimeslotMapper(MysqlDb db) : base(db) { }

        protected Blocked_timeslot ProcessRow(MySqlDataReader Reader, Blocked_timeslot blockedTimeSlot)
        {
            blockedTimeSlot.Daytime_id = Reader.GetInt32(0);
            blockedTimeSlot.Hardblock = Reader.GetBoolean(1);
            return blockedTimeSlot;
        }

        public List<Blocked_timeslot> FindByUserId(int userId)
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT daytime_id, hardblock FROM blocked_timeslot WHERE user_id = ?id";
            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = userId;
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            List<Blocked_timeslot> result = new List<Blocked_timeslot>();
            while (Reader.Read())
            {
                result.Add(this.ProcessRow(Reader, new Blocked_timeslot()));
            }
            this._db.CloseConnection();
            return result;
        }

        public void Save(Blocked_timeslot slot)
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "INSERT INTO blocked_timeslot (user_id, daytime_id, hardblock) VALUES (?user_id, ?daytime_id, ?hardblock)";
            command.Parameters.Add(new MySqlParameter("?user_id", MySqlDbType.Int32)).Value = slot.User.Id;
            command.Parameters.Add(new MySqlParameter("?daytime_id", MySqlDbType.Int32)).Value = slot.Daytime.Id;
            if (slot.Hardblock)
            {
                command.Parameters.Add(new MySqlParameter("?hardblock", MySqlDbType.Int16)).Value = 1;
            }
            else
            {
                command.Parameters.Add(new MySqlParameter("?hardblock", MySqlDbType.Int16)).Value = 0;
            }
            this._db.ExecuteCommand(command);
            this._db.CloseConnection();
        }
    }
}