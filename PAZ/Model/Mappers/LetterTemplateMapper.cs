using System;
using MySql.Data.MySqlClient;
using PAZMySQL;

namespace PAZ.Model.Mappers
{
    public class LetterTemplateMapper : Mapper
    {
        public LetterTemplateMapper(MysqlDb db)
            : base(db)
        {

        }

        public LetterTemplate Find(int id)
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();
            command.CommandText = "SELECT id, kenmerk, contactpersonen, telefoon, email, avans_adres, avans_locatie, begin_kern, reis_informatie, verdere_informatie, afzenders, bijlagen, voettekst_links, voettekst_center, voettekst_rechts FROM letter_template WHERE id = ?id";
            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = id;
            MySqlDataReader Reader = this._db.ExecuteCommand(command);
            Reader.Read();//Only 1 row
            this._db.CloseConnection();

            int index = 0;
            return new LetterTemplate(Reader.GetInt32(index++), Reader.GetString(index++), Reader.GetString(index++), Reader.GetString(index++), Reader.GetString(index++), Reader.GetString(index++), Reader.GetString(index++), Reader.GetString(index++), Reader.GetString(index++), Reader.GetString(index++), Reader.GetString(index++), Reader.GetString(index++), Reader.GetString(index++), Reader.GetString(index++), Reader.GetString(index++));
        }

        public void Save(LetterTemplate letterTemplate)
        {
            this._db.OpenConnection();
            MySqlCommand command = this._db.CreateCommand();

            command.CommandText = "UPDATE letter_template SET kenmerk=?kenmerk, contactpersonen=?contactpersonen, telefoon=?telefoon, email=?email, avans_adres=?avans_adres, avans_locatie=?avans_locatie, begin_kern=?begin_kern, reis_informatie=?reis_informatie, verdere_informatie=?verdere_informatie, afzenders=?afzenders, bijlagen=?bijlagen, voettekst_links=?voettekst_links, voettekst_center=?voettekst_center, voettekst_rechts=?voettekst_rechts WHERE id = ?id";

            command.Parameters.Add(new MySqlParameter("?id", MySqlDbType.Int32)).Value = letterTemplate.Id;
            command.Parameters.Add(new MySqlParameter("?kenmerk", MySqlDbType.String)).Value = letterTemplate.Kenmerk;
            command.Parameters.Add(new MySqlParameter("?contactpersonen", MySqlDbType.String)).Value = letterTemplate.Contactpersonen;
            command.Parameters.Add(new MySqlParameter("?telefoon", MySqlDbType.String)).Value = letterTemplate.Telefoon;
            command.Parameters.Add(new MySqlParameter("?email", MySqlDbType.String)).Value = letterTemplate.Email;
            command.Parameters.Add(new MySqlParameter("?avans_adres", MySqlDbType.String)).Value = letterTemplate.AvansAdres;
            command.Parameters.Add(new MySqlParameter("?avans_locatie", MySqlDbType.String)).Value = letterTemplate.AvansLocatie;
            command.Parameters.Add(new MySqlParameter("?begin_kern", MySqlDbType.Text)).Value = letterTemplate.BeginKern;
            command.Parameters.Add(new MySqlParameter("?reis_informatie", MySqlDbType.LongText)).Value = letterTemplate.ReisInformatie;
            command.Parameters.Add(new MySqlParameter("?verdere_informatie", MySqlDbType.Text)).Value = letterTemplate.VerdereInformatie;
            command.Parameters.Add(new MySqlParameter("?afzenders", MySqlDbType.String)).Value = letterTemplate.Afzenders;
            command.Parameters.Add(new MySqlParameter("?bijlagen", MySqlDbType.Text)).Value = letterTemplate.Bijlagen;
            command.Parameters.Add(new MySqlParameter("?voettekst_links", MySqlDbType.Text)).Value = letterTemplate.VoettekstLinks;
            command.Parameters.Add(new MySqlParameter("?voettekst_center", MySqlDbType.Text)).Value = letterTemplate.VoettekstCenter;
            command.Parameters.Add(new MySqlParameter("?voettekst_rechts", MySqlDbType.Text)).Value = letterTemplate.VoettekstRechts;

            this._db.ExecuteCommand(command);
            this._db.CloseConnection();
        }
    }
}
