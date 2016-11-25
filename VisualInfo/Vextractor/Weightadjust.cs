using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vextractor
{
    class Weightadjust
    {//;
        String sqlparent= "select parent,count(*) from (select parent from scores where site=@site and mark=@mark limit @samplenum)as temp group by parent";  //设置取出不同模式的sql;
        String sqlcurrent= "select current,count(*) from (select current from scores where site=@site and mark=@mark limit @samplenum)as temp group by current";
        String sqlfirstchild= "select firstchild, count(*) from (select firstchild from scores where site = @site and mark= @mark limit @samplenum) as temp group by firstchild";
        Dictionary<String, double> parentweight =new Dictionary<String, double>();
        Dictionary<String, double> currentweight = new Dictionary<String, double>();
        Dictionary<String, double> firstchildweight = new Dictionary<String, double>();
        public double parentscore(String parent2) //传入需要比较的父节点模式
        {
            if (parentweight.ContainsKey(parent2))
            {
                return parentweight[parent2];
            }
            else
            {
                return 0;
            }
        }
        public double currentscore(String current2) //传入需要比较的当前节点模式
        {
            if (currentweight.ContainsKey(current2))
            {
                return currentweight[current2];
            }
            else
            {
                return 0;
            }
        }
        
        public double firstchildscore(String child2) //传入需要比较的父节点模式
        {
            if (firstchildweight.ContainsKey(child2))
            {
                return firstchildweight[child2];
            }
            else
            {
                return 0;
            }
        }
        public void show()
        {
            foreach (String key in parentweight.Keys)
            {
                Console.WriteLine("parentpattern  :"+ key +" 系数：  "+ parentweight[key]);
            }
            foreach (String key in currentweight.Keys)
            {
                Console.WriteLine("curentpattern  :" + key + " 系数：  " + currentweight[key]);
            }
            foreach (String key in firstchildweight.Keys)
            {
                Console.WriteLine("childpattern  :" + key + " 系数：  " + firstchildweight[key]);
            }
        }
        private void setsqlparent(String sql) 
        {
            sqlparent = sql;
        }
        private void setsqlcurrent(String sql)
        {
            sqlcurrent = sql;
        }
        private void setsqlfirstchild(String sql)
        {
            sqlfirstchild = sql;
        }
        public void computeWeight(String site,String mark,int smplenum) //计算指定网站和类别的权重
        {
            parentweight.Clear();
            currentweight.Clear();
            firstchildweight.Clear();
            

        }
        private void transformToweight(Dictionary<String, double> weight, int size) //传入的是所有抽取模式一起出现的总条数
        {//该函数用于将weight中模式出现的绝对次数转变为相对系数
            if (weight.Count>0) {
                List<String> keylist = new List<String>();
                keylist.AddRange(weight.Keys);
                foreach (String key in keylist)
                {
                   weight[key] =weight[key] / size; 
                }
            }

        }
        public void computeParentweight(String site, String mark,int samplenum)
        {
            parentweight.Clear(); //(可改权重矩阵)
            MySqlConnection myCon = DBmysql.getmysqlcon();
            myCon.Open();
            MySqlCommand mySqlCommand = new MySqlCommand(sqlparent, myCon); //处理父节点模板(可改权重矩阵)
            try
            {
                int size = 0;  //所有次数相加取
                mySqlCommand.Parameters.Clear();
                mySqlCommand.Parameters.AddWithValue("@site", site);
                mySqlCommand.Parameters.AddWithValue("@mark", mark);
                mySqlCommand.Parameters.AddWithValue("@samplenum", samplenum);
                MySqlDataReader reader = mySqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    
                    if (reader.HasRows)
                    {
                        String pattern = reader.GetString(0);
                        int count = reader.GetInt32(1);
                        size = size + count;
                        parentweight.Add(pattern,count);  //此时还没有进行权重系数的装换(可改权重矩阵)                      
                      //  Console.WriteLine("取出的模式及其数目：  "+pattern+count);

                    }
                }
                reader.Close();
                transformToweight(parentweight,size); //转换成权重系数(可改权重矩阵)
            }
            catch (Exception e)
            {
                Console.WriteLine("查询失败了！");
                Console.WriteLine(e.ToString());
            }
            finally
            {
                mySqlCommand.Dispose();
                myCon.Close();
            }
        }
        public void computeCurrentweight(String site,String mark,int samplenum)
        {
            currentweight.Clear();
            MySqlConnection myCon = DBmysql.getmysqlcon();
            myCon.Open();
            MySqlCommand mySqlCommand = new MySqlCommand(sqlcurrent, myCon); //处理父节点模板(可改权重矩阵)
            try
            {
                int size = 0;  //所有次数相加取
                mySqlCommand.Parameters.Clear();
                mySqlCommand.Parameters.AddWithValue("@site", site);
                mySqlCommand.Parameters.AddWithValue("@mark", mark);
                mySqlCommand.Parameters.AddWithValue("@samplenum", samplenum);
                MySqlDataReader reader = mySqlCommand.ExecuteReader();
                while (reader.Read())
                {

                    if (reader.HasRows)
                    {
                        String pattern = reader.GetString(0);
                        int count = reader.GetInt32(1);
                        size = size + count;
                        currentweight.Add(pattern, count);  //此时还没有进行权重系数的装换(可改权重矩阵)                      
                    }
                }
                reader.Close();
                transformToweight(currentweight, size); //转换成权重系数(可改权重矩阵)
            }
            catch (Exception e)
            {
                Console.WriteLine("查询失败了！");
                Console.WriteLine(e.ToString());
            }
            finally
            {
                mySqlCommand.Dispose();
                myCon.Close();
            }
        }
        public void computeFirstchildweight(String site,String mark,int samplenum)
        {
            firstchildweight.Clear();
            MySqlConnection myCon = DBmysql.getmysqlcon();
            myCon.Open();
            MySqlCommand mySqlCommand = new MySqlCommand(sqlfirstchild, myCon); //处理父节点模板(可改权重矩阵)
            try
            {
                int size = 0;  //所有次数相加取
                mySqlCommand.Parameters.Clear();
                mySqlCommand.Parameters.AddWithValue("@site", site);
                mySqlCommand.Parameters.AddWithValue("@mark", mark);
                mySqlCommand.Parameters.AddWithValue("@samplenum", samplenum);
                MySqlDataReader reader = mySqlCommand.ExecuteReader();
                while (reader.Read())
                {

                    if (reader.HasRows)
                    {
                        String pattern = reader.GetString(0);
                        int count = reader.GetInt32(1);
                        size = size + count;
                        firstchildweight.Add(pattern, count);  //此时还没有进行权重系数的装换(可改权重矩阵)                      
                    }
                }
                reader.Close();
                transformToweight(firstchildweight, size); //转换成权重系数(可改权重矩阵)
            }
            catch (Exception e)
            {
                Console.WriteLine("查询失败了！");
                Console.WriteLine(e.ToString());
            }
            finally
            {
                mySqlCommand.Dispose();
                myCon.Close();
            }
        }
    }
}
