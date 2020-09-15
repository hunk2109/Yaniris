using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

namespace sistgre
{
    class cnxsql
    {
        public string conectar()
        {
            SQLiteConnection cnx = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
            try
            {
                cnx.Open();
                return "conxion exitosa!";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            finally
            {
                cnx.Close();
            }



        }

        public string consultasinreaultado(string sql)
        {
            SQLiteConnection cnx = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
            try
            {
                cnx.Open();
                SQLiteCommand comand = new SQLiteCommand(sql, cnx);
                comand.ExecuteNonQuery();
                return "";

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                cnx.Close();
            }
        }
        public DataTable cosnsultaconresultado(string sql)
        {
            SQLiteDataAdapter ad;
            DataTable dt = new DataTable();
            SQLiteConnection cnx = new SQLiteConnection("Data Source=C:\\bdd\\factura.s3db; Version=3;");
            try
            {
                cnx.Open();

                SQLiteCommand cmd;
                cmd = cnx.CreateCommand();
                cmd.CommandText = sql;
                ad = new SQLiteDataAdapter(cmd);
                ad.Fill(dt);
            }

            catch (SQLiteException ex)


            {



            }
            cnx.Close();
            return dt;
        }

    }
}

