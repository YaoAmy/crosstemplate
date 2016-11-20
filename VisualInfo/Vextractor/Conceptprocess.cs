using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Collections;

namespace Vextractor
{
    class Conceptprocess
    {
        public static double GetCosineSimilarity(List<double> V1, List<double> V2)
        {
            double sim = 0.0d;
            int N = 0;
            N = ((V2.Count < V1.Count) ? V2.Count : V1.Count);
            double dot = 0.0d;
            double mag1 = 0.0d;
            double mag2 = 0.0d;
            for (int n = 0; n < N; n++)
            {
                dot += V1[n] * V2[n];
                mag1 += Math.Pow(V1[n], 2);
                mag2 += Math.Pow(V2[n], 2);
            }
            sim = dot / (Math.Sqrt(mag1) * Math.Sqrt(mag2));
            if (Double.IsNaN(sim))
                return 0;
            else
                return dot / (Math.Sqrt(mag1) * Math.Sqrt(mag2));
        }
        public static double getConceptscore(Dictionary<String, int> origin_concept, Dictionary<String, int> candidate_concept)
        {
            //向量的概念词应该根据origin_concept中的单词顺序排列；
            List<double> v1 = new List<double>();
            List<double> v2 = new List<double>();
            foreach (String key in origin_concept.Keys)
            {
                v1.Add(origin_concept[key]);
                if (candidate_concept.ContainsKey(key))
                {
                    v2.Add(candidate_concept[key]);
                }
                else {
                    v2.Add(0);
                }
            }
            return GetCosineSimilarity(v1, v2);
        }
        public static Dictionary<String, int> getConcept(String s)
        {
            Dictionary<String, int> concepts_dictionary = new Dictionary<String, int>();
            String[] wordlist = Regex.Split(s.Trim().ToLower().Replace("/", " "), "\\s+");   //该单词表中虽然含有许影响多无用词，但是概念查找中如果没有找到该词不放入词表中
            String sql = "select concept,count from concept_table where instance=@ins limit 10";
            MySqlConnection myCon = DBmysql.getmysqlcon();
            myCon.Open();
            MySqlCommand mySqlCommand = new MySqlCommand(sql, myCon);
            try
            {
                foreach (String instanceString in wordlist)
                {
                    mySqlCommand.Parameters.Clear();
                    mySqlCommand.Parameters.AddWithValue("@ins", instanceString);
                    MySqlDataReader reader = mySqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            String conceptString = reader.GetString(0);
                            int concept_count = reader.GetInt32(1);
                            //     Console.WriteLine("intstance: "+ instanceString + "  concept: " + conceptString + "      count:" + concept_count);
                            if (concepts_dictionary.ContainsKey(conceptString))
                            {
                                int temp_count = concepts_dictionary[conceptString];
                                concepts_dictionary[conceptString] = concept_count + temp_count;
                            }
                            else
                            {
                                concepts_dictionary.Add(conceptString, concept_count);
                            }
                        }
                    }
                    reader.Close();
                }

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
            return concepts_dictionary;
            //foreach (KeyValuePair<String,int> pair in concepts_dictionary)
            //{
            //    Console.WriteLine(pair.Key+"   "+pair.Value);
            //}
        }
    }
}
