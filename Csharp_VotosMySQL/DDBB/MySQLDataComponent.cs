using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp_VotosMySQL.DDBB
{
    //USAR ESTO EN LOS EJER DE SQL
    class MySQLDataComponent 
    {
        public static void ExecuteNonQuery(String SQL, String cnstr)
        {
            //Hay que instalar el paquete MySqlClient
            MySqlConnection con = new MySqlConnection(cnstr);
            con.Open();
            MySqlCommand cmd = new MySqlCommand(SQL, con);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }

        public static DataTable LoadData(String SQL, String cnstr)
        {

            MySqlConnection con = new MySqlConnection(cnstr);
            con.Open();
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(SQL, con);
            //Vuelca datos a DTB
            da.Fill(dt);
            
            da.Dispose();
            con.Close();
                
            return dt;
        }
    }
}
