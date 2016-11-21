using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vextractor
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
             Application.Run(new Form1());

            //     String[] symbolsource = { "@", "%", "#", "¥", "$", "℃", "GB", "&", "gb", "mm", "-", "_" };
            // String temp = "Canon EOS Rebel T5 DSLR Camera with EF-S 18-55mm IS II & 75-300mm Zoom Lens and 32GB Accessory Bu";
            //String temp2 = "VTech CS6919 DECT 6.0 Expandable Cordless Phone with Caller ID and Handset Speakerphone, Silver/Black";
            //String temp3 = "Redeemable for talk time, text messaging, unlimited IM, picture and video messages";
            //    String temp4 = "Straight Talk Apple iPhone 5S 16GB 4G LTE Refurbished Prepaid Smartphone w/ Bonus $45 Service Plan";
            //double t1=Conceptprocess.getConceptscore(Conceptprocess.getConcept(temp), Conceptprocess.getConcept(temp4));
            // double t2=Conceptprocess.getConceptscore(Conceptprocess.getConcept(temp), Conceptprocess.getConcept(temp3));
            // Console.Write("test: "+t1);
            //   Candidatescore cs = new Candidatescore();
            //    cs.setCandidate();
            //     cs.computeScores();
           // String tem = " fdfdf  f4 out of stars 5 fdf";
           // Console.Write(Regex.Replace(tem,@"\d* out of stars \d*",""));
            //if (m.Success) { Console.WriteLine(m.Value); }
            //else
            //{ Console.WriteLine("0"); }
            
            //List<double> V1 = new List<double>();
            //V1.Add(0);
            //V1.Add(0);
            //V1.Add(0);
            //V1.Add(0);
            //V1.Add(0);
            //V1.Add(0);
            //List<double> V2 = new List<double>();
            //V2.Add(0.444444444444444);
            //V2.Add(0.333333333333333);
            //V2.Add(0.333333333333333);
            //V2.Add(0.111111111111111);
            //V2.Add(0.555555555555556);
            //V2.Add(0.333333333333333);
            //// Console.WriteLine( cs.GetCosineSimilarity(v1,v2));
            //// Console.WriteLine("!!!!!!!!!!!!!!!!!!!!");
            //Dictionary<String, double> dic = new Dictionary<String, double>();
            //dic.Add("4", 0.8);
            //dic.Add("1",3400.23);
            //dic.Add("3", 1.45);
            //dic.Add("2",1.5);
            
            
            //var dicSort = from objDic in dic orderby objDic.Value descending select objDic;
            //foreach (KeyValuePair<String, double> kvp in dicSort)
            //    Console.WriteLine(kvp.Key + "：" + kvp.Value );
        }
    }
}
