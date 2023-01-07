using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace sistgre
{
    class cnxsql
    {
        public string conectar()
        {
            MySqlConnection cnx = new MySqlConnection("server = 127.0.0.1; uid=root;pwd=muerete66;database=factura");
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
            MySqlConnection cnx = new MySqlConnection("server = 127.0.0.1; uid=root;pwd=muerete66;database=factura");
            try
            {
                cnx.Open();
                MySqlCommand comand = new MySqlCommand(sql, cnx);
                comand.ExecuteNonQuery();
                return "";

            }
            catch (MySqlException ex)
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
            MySqlDataAdapter ad;
            DataTable dt = new DataTable();
            MySqlConnection cnx = new MySqlConnection("server = 127.0.0.1; uid=root;pwd=muerete66;database=factura");
            try
            {
                cnx.Open();

                MySqlCommand cmd;
                cmd = cnx.CreateCommand();
                cmd.CommandText = sql;
                ad = new MySqlDataAdapter(cmd);
                ad.Fill(dt);
            }

            catch (MySqlException ex)


            {



            }
            cnx.Close();
            return dt;
        }

    }
}

