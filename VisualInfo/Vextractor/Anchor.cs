using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Vextractor
{
    class Anchor
    {
        List<String> source = new List<String>();
        String[] symbolsource = {"@","%","#", "¥", "$", "℃","GB","gb","mm","-","_","&", "mah" };
        HashSet<String> symbol =new HashSet<String>();      //特殊符号
        List<String> snconbine =new List<String>();  //numS、记录数字符号和字母的组合，数字全部用0替代。只要含有的就可以算
        List<double> vecofconbine = new List<double>();  //将组合的均值向量放入其中
        double intnum = 0;   //记录是否含有纯数字
        double length = 0;      //长度过大或过小都应该适当减少得分。
        public HashSet<String> getsymbol()
        {
            return symbol;
        }
        public List<double> getvecofconbine()
        {
            return vecofconbine;
        }
        public double getintnum()
        {
            return intnum;
        }
        public double getlength()
        {
            return length;
        }

        public void sourcesample() {
            //source.Add("VTech CS6919 DECT 6.0 Expandable Cordless Phone with Caller ID and Handset Speakerphone, Silver/Black");
            //source.Add("Straight Talk Apple iPhone 5S 16GB 4G LTE Refurbished Prepaid Smartphone w/ Bonus $45 Service Plan");
            //source.Add("AT&T EL51203 DECT 6.0 Phone with Caller ID/Call Waiting, 2 Cordless Handsets, Silver");
            //source.Add("T-Mobile Samsung Galaxy S4 Prepaid Smartphone");
            //source.Add("VTech CS6919 DECT 6.0 Expandable Cordless Phone with Caller ID and Handset Speakerphone, Silver/Black");
            //source.Add("FIGO Atrium 5.5 - Dual Micro SIM Unlocked 16GB Smartphone - US & International GSM 4G");
            //source.Add("iPhone 7 Back Skin with Stick-up edges,HYAIZLZ(TM)Tire lines Carbon Fiber");
            //source.Add("iPhone 7 Case, AiSpeed Premium TPU [Soft Series] Flexible Soft Transparent Clear Back");
            //source.Add("Techniroux 20000mah Portable Charger with Inbuilt Flash Lights, Led Display and 3 USB");
            // source.Add("Techniroux 20000mah Portable Charger with Inbuilt Flash Lights, Led Display and 3 USB");
            source.Add("$399.00 Straight Talk Apple iPhone 6 Plus 16GB 4G LTE Prepaid Smartphone ");
            source.Add("Straight Talk Apple iPhone 5S 16GB 4G LTE Refurbished Prepaid Smartphone w/ Bonus $45 Service Plan");
            source.Add("$45.00 (Email Delivery) Straight Talk Monthly Plan Unlimited 30 Access Days $45 ");
            source.Add("FIGO Atrium 5.5 - Dual Micro SIM Unlocked 16GB Smartphone - US & International GSM 4G");
            source.Add("$34.99 List price $37.68 Save $2.69 AT&T EL51203 DECT 6.0 Phone with Caller ID/Call Waiting, 2 Cordless Handsets, Silver ");
            source.Add("$368.99 Techniroux 20000mah Portable Charger with Inbuilt Flash Lights, Led Display and 3 USB");
            source.Add("$749.00 Straight Talk Apple iPhone 6S Plus 16GB 4G LTE Prepaid Smartphone ");
        }
        public void computeAnchor()
        {
            symbol.Clear();
            snconbine.Clear();
            vecofconbine.Clear();
            intnum = 0;   
            length = 0;
            //重新计算之前需要清零；
            double size = source.Count;
            foreach (String s in source) {
                length = length + Regex.Replace(s, @"\s+", "").Length;
                compute(s);            
            }
            length = length/size;
       //     symbol.Add("$");      //特殊符号
            intnum = intnum/size;
            //统计每一类的向量  //   顺序为  { "a0","0a","0a0","0s","as","0as" }
            double na0 = 0;
            double n0a = 0;
            double n0a0 = 0;
            double n0s = 0;
            double nas = 0;
            double n0as = 0;
            foreach (String s in snconbine)
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
            vecofconbine.Add(na0/size);
            vecofconbine.Add(n0a/size);
            vecofconbine.Add(n0a0/size);
            vecofconbine.Add(n0s/size);
            vecofconbine.Add(nas/size);
            vecofconbine.Add(n0as /size);
        }
        private  void compute(String stemp) //对所有设定好的单个样本进行统计提取，重复执行时对所有样本进行统计
        {
            String temp = stemp;
            //组合形式，全大写字母（行业标准）、s0、0s、0s0、（数字，字母，符号组合）
            //提取全大写字母S标识，字母数字型、符号数字型（单位）、
            temp = Regex.Replace(temp, "&amp;", " ");
            temp = Regex.Replace(temp, @"\d+", "0");
            temp = Regex.Replace(temp, @"0.0", "0");
            temp = Regex.Replace(temp, ",", " ");
            temp = Regex.Replace(temp, "/", " ");
            intnum = intnum + Regex.Matches(temp, "0").Count;
            //  Console.WriteLine(temp);
            //组合形式，全大写字母（行业标准）、a0、0a、0a0、（数字，字母，符号组合）
            //提取全大写字母S标识，字母数字型@"0[a-zA-z]+",@"[a-zA-z]+0",@"0[a-zA-z]+0"、符号数字型字母组合（单位）
            //数字符号型：0s;字母符号型：as；字母数字符号型0as
             //snconbine得到所有样本的统计情况
            if (Regex.IsMatch(temp, @"0[a-zA-z]+")) {
                //Console.WriteLine("0a");
                snconbine.Add("0a");
            } 
            if (Regex.IsMatch(temp, @"[a-zA-z]+0")) {
                //Console.WriteLine("a0"); 
                snconbine.Add("a0");
            } 
            if (Regex.IsMatch(temp, @"0[a-zA-z]+")) {
                //Console.WriteLine("0a0"); 
                snconbine.Add("0a0");
            } 
            String[] joins = {"-","_" };
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
         //   Console.WriteLine(temp);
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
                            temp = Regex.Replace(temp, s, "");
                        }
                    }
                }
            }
            temp = Regex.Replace(temp, @"0[a-zA-z]+", "");
            temp = Regex.Replace(temp, @"[a-zA-z]+0", "");
            temp = Regex.Replace(temp, @"0[a-zA-z]+", "");
            temp = Regex.Replace(temp, "0", "");
            List<String> words = new List<String>(Regex.Split(temp, @"\s+"));
            List<String> rm = new List<String>();
            for (int i = 0; i < words.Count; i++)
            { }
            foreach (String word in words)
            {
                if (word.Equals(word.ToUpper()))
                {
                 //   Console.WriteLine("S");
                    rm.Add(word);
                }
                if (word.Equals("0"))
                {
                    rm.Add(word);
                }
            }
            foreach (String rmword in rm)
            {
                //  Console.WriteLine("删除的"+rmword);
                words.Remove(rmword);
            }
            //foreach (String word in words)
            //{
            //    Console.WriteLine(word);
            //}

        }

    }
}
