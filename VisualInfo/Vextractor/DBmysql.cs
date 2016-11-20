using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vextractor
{
    class DBmysql
    {
        public static MySqlConnection getmysqlcon()
        {
            string M_str_sqlcon = "server=localhost;user id=root;password=;database=htmlinfo"; //根据自己的设置
            MySqlConnection myCon = new MySqlConnection(M_str_sqlcon);
            return myCon;
        }


    }
}
