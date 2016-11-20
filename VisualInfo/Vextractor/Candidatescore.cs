using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
/*
public void setanchor()  //执行该类前一定需要先设定锚点样本
setCandidate()          //两种设置候选节点的方法,从外部分传入候选节点可以不用设置候选，单一定要设定锚点样本
setCandidate(List<HtmlElement> leaf_nodes)
*/
namespace Vextractor
{
    class Candidatescore   //计算候选节点的得分
    {
         Dictionary<String, double> candidate = new Dictionary<String, double>(); //候选节点以String形式用candidate,HtmlElement形式用candidates;
        Dictionary<HtmlElement, double> candidates = new Dictionary<HtmlElement, double>();
        String[] symbolsource = { "@", "%", "#", "¥", "$", "℃", "GB", "gb", "mm", "-", "_", "&", "mah" };
        double avgnum; //数字，根据样本得出的指标
        double avglen; //平均长度
        List<double> vecofconbine;//组合向量
        public double getavglen()
        {
            return avglen;
        }
        private String Read(string path)
        {
            String temp = "";
            StreamReader sr = new StreamReader(path, Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                temp = temp + line;
            }
            return temp;
        }
        public void setCandidate()  //设置待计算的候选节点
        {
            candidate.Clear();
            String cans = Read("D:\\candidate.txt");
            String[] carray = Regex.Split(cans,"######"); ;
            int count = 0;
            foreach (String s in carray)
            {
                String stemp = Regex.Replace(s,"&amp;"," ").Trim();
                  Console.WriteLine("第"+ count++ +"个："+createNode(stemp).InnerText);
                
                candidate.Add(createNode(stemp).InnerText.Trim(),0);
            }           
        }
       public void setCandidate(List<HtmlElement> leaf_nodes)
        {
            candidates.Clear();   //非String型的候选用candidates作为容器
            foreach (HtmlElement ele in leaf_nodes)
            {
                candidates.Add(ele, 0);
            }

        }
        private HtmlNode createNode(String s)   //根据纯文本得到封装的节点
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(s);          
            return doc.DocumentNode.LastChild;
        }
        public void setanchor()  //设置样本锚点的指标，暂时写死
        {
            Anchor anchor = new Anchor();
            anchor.sourcesample(); //设置锚文本计算样本，目前写死
            anchor.computeAnchor();//计算提取样本
            avgnum = anchor.getintnum();
            avglen = anchor.getlength();
            //  HashSet<String> symbol = new HashSet<String>();      //特殊符号
            //  HashSet<String> snconbine = anchor.getsnconbine();
             vecofconbine = anchor.getvecofconbine();
        }
        public double computeScore(String s) //计算单个候选节点的评分
        {
            return numscore(s, avgnum) + lenscore(s, avglen) + conbinescore(s, vecofconbine);
        }
        public void computeScores()   //计算所有候选节点的评价
        {
           
            //  Console.WriteLine("样本数字"+avgnum);
            //   Console.WriteLine("样本长度"+avglen);
            //   for (int i=0;i<6;i++)
            //{
            //     Console.WriteLine(vecofconbine[i]);
            //}
            List<String> keylist = new List<String>();
            keylist.AddRange(candidate.Keys);
            foreach (String key in keylist)
            {
                candidate[key]= numscore(key, avgnum) + lenscore(key, avglen) + conbinescore(key,vecofconbine);
                Console.WriteLine("数字分： "+numscore(key, avgnum));
                Console.WriteLine("长度分： " + lenscore(key, avglen));
                Console.WriteLine("组合分： " + conbinescore(key, vecofconbine));
                Console.WriteLine("总分： " + candidate[key]);
            }

        }
        private double numscore(String temp,double avgnum)
        {
            temp = Regex.Replace(temp, @"\d+", "0");
            temp = Regex.Replace(temp, @"0.0", "0");
            double tnum = Regex.Matches(temp, "0").Count;
            //Console.WriteLine("候选数字： " + tnum);
            return Math.Min(tnum,avgnum)/Math.Max(tnum,avgnum);
        }
        public double lenscore(String temp,double avglen)
        {
            
            temp = Regex.Replace(temp, ",", " ");
            temp = Regex.Replace(temp, "/", " ");
            double tlen = Regex.Replace(temp, @"\s+", "").Length;
          //  Console.WriteLine("候选长度： " + tlen);
            return Math.Min(tlen,avglen) / Math.Max(tlen,avglen);
        }
        private List<String> getconbine(String temp)  //计算文本中含有的文本组合
        {
            List<String> snconbine = new List<String>();
            
            temp = Regex.Replace(temp, @"\d+", "0");
            temp = Regex.Replace(temp, @"0.0", "0");
            temp = Regex.Replace(temp, ",", " ");
            temp = Regex.Replace(temp, "/", " ");
            if (Regex.IsMatch(temp, @"0[a-zA-z]+"))
            {
                //Console.WriteLine("0a");
                snconbine.Add("0a");
            }
            if (Regex.IsMatch(temp, @"[a-zA-z]+0"))
            {
                //Console.WriteLine("a0"); 
                snconbine.Add("a0");
            }
            if (Regex.IsMatch(temp, @"0[a-zA-z]+"))
            {
                //Console.WriteLine("0a0"); 
                snconbine.Add("0a0");
            }
            String[] joins = { "-", "_" };
            foreach (String s in joins)
            {
                String[] t2 = null;
                if (temp.Contains(s))
                {
                    t2 = Regex.Split(temp, s);
                    if (t2.Length > 0) t2[0].TrimEnd();
                    int i = 1;
                    for (; i < t2.Length - 1; i++)
                    {
                        t2[i] = t2[i].Trim();

                    }
                    t2[i] = t2[i].TrimEnd();
                    temp = String.Join(s, t2);
                }
            }
            String[] t3 = Regex.Split(temp, " ");
            if (t3 != null && t3.Length > 0)
            {
                foreach (String s in t3)
                {
                    foreach (String symbol in symbolsource)
                    {
                        if (s.Contains(symbol))
                        {
                            if (Regex.IsMatch(s, @"[a-zA-z]+"))
                            {                                //含有字母；

                                if (Regex.IsMatch(s, @"\d+"))
                                {
                                    // Console.WriteLine("0as");
                                    snconbine.Add("0as");
                                }
                                else
                                {
                                    //  Console.WriteLine("as");
                                    snconbine.Add("as");
                                }
                            }
                            else
                            {
                                if (Regex.IsMatch(s, @"\d+"))
                                {
                                    //  Console.WriteLine("0s");
                                    snconbine.Add("0s");
                                }


                            }
                            temp = Regex.Replace(temp,s, "");
                        }
                    }
                }
            }
            return snconbine;
        }
        private double conbinescore(String temp, List<double> vec2)
        {

            List<double> v1 = toVector(getconbine(temp));
            List<double> v2 = vec2;
           // Console.Write("v1的向量");
            //for (int i= 0;i< 6;i++)
            //{
                
            //    Console.Write(v1[i]);
            //}
           // Console.Write("v2的向量");
            //for (int i= 0; i < 6; i++)
            //{              
            //    Console.Write(v2[i]);
            //}
        return  GetCosineSimilarity(v1, v2);

        }
        private List<double> toVector(List<String> set)
        { //   顺序为  { "a0","0a","0a0","0s","as","0as" }
            List<double> v = new List<double>();
            double na0 = 0;
            double n0a = 0;
            double n0a0 = 0;
            double n0s = 0;
            double nas = 0;
            double n0as = 0;
            foreach (String s in set)
            {
                if (s.Equals("a0"))
                    na0++;
                //2
                if (s.Equals("0a"))
                    n0a++;
                //3
                if (s.Equals("0a0"))
                    n0a0++;
                //4
                if (s.Equals("0s"))
                    n0s++;
                //5
                if (s.Equals("as"))
                    nas++;
                //6
                if (s.Equals("0as"))
                    n0as++;
            }
            v.Add(na0);
            v.Add(n0a);
            v.Add(n0a0);
            v.Add(n0s);
            v.Add(nas);
            v.Add(n0as);
            return v; 
        }
        public double GetCosineSimilarity(List<double> V1, List<double> V2)
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


        private void DictonarySort(Dictionary<string, int> dic)
        {
            var dicSort = from objDic in dic orderby objDic.Value descending select objDic;
            foreach (KeyValuePair<string, int> kvp in dicSort)
                Console.Write(kvp.Key + "：" + kvp.Value + "<br />");
        }
    }
}
