using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vextractor
{
    class SelectCandidate
    {
        Dictionary<HtmlElement, double> candidates = new Dictionary<HtmlElement, double>();
        Candidatescore candscortor = new Candidatescore();
         
        public void SetCandidate(List<HtmlElement> leaf_nodes)
        {
            candidates.Clear();   //非String型的候选用candidates作为容器
            foreach (HtmlElement ele in leaf_nodes)
            {
                candidates.Add(ele, 0);
            }
            
        }
        public SelectCandidate()  //如果一开始没有设置候选节点，则调用需要后来调用setCandidate方法
        {
            candscortor.setanchor();
        }
        public SelectCandidate(List<HtmlElement> leaf_nodes)
        {
            candscortor.setanchor(); 
            SetCandidate(leaf_nodes);
        }
        public void select(String sampletext) //对candidate打分；
        {
            DateTime beforDT = System.DateTime.Now;

            //耗时巨大的代码  

           
            Console.Write("开始计算评分");
            int test = 0;
            Dictionary<String, int> sampleconcepts = Conceptprocess.getConcept(sampletext);
            List<HtmlElement> keylist = new List<HtmlElement>();
            keylist.AddRange(candidates.Keys);
            foreach (HtmlElement key in keylist)
            {
                test++;
                Console.WriteLine("正在计算第"+test+"个");
                double score = 0;  //当前节点评分
                double parent_score = -1;  //父节点评分；
                HtmlElement current = key;  //当前节点
                HtmlElement parent = current.Parent; //父节点
                String temp = Regex.Replace(key.InnerText.Trim(), "\\s+", " ");  //当前节点文本
                double lenscore = candscortor.lenscore(temp, candscortor.getavglen());
                Console.Write("节点：  "+ temp);
                Console.WriteLine("长度得分"+lenscore);
                
                if (lenscore < 0.4) //过滤掉一些明显长度分过低的节点
                {
                    
                    candidates.Remove(key);
                }
                else
                { //计算概念分一定要样本在前，待评节点在后，因为需要更具样本生成待评价节点概念向量
                    Dictionary<String, int> currentconcepts = Conceptprocess.getConcept(temp);
                  //  Console.WriteLine("String:  " + temp);
                 //   Console.WriteLine("概念词个数:  "+ currentconcepts.Count);
                    score = candscortor.computeScore(temp)+ Conceptprocess.getConceptscore(sampleconcepts,currentconcepts);  //当前节点评分
                //    Console.WriteLine("组合分1:  " + candscortor.computeScore(temp));
                //    Console.WriteLine("概念分1:  " + Conceptprocess.getConceptscore(sampleconcepts, currentconcepts));
               //     Console.WriteLine("111111得分:  " + score);
                    String parent_temp = null; //如果存在父节点则找出父节点文本；
                    if (parent != null)
                    {
                        //父节点字符串需要进行处理
                           parent_temp = Regex.Replace(HtmlElementprocess.getElementString(parent), "\\s+", " ");
                    }

                    if (parent_temp != null && parent_temp.Length > 0) //父节点评分
                    {
                        if (candscortor.lenscore(parent_temp, candscortor.getavglen()) > 0.4)
                        {
                            if (parent_temp.Equals(temp))
                            {
                                parent_score = score;
                            }
                            else {
                                //   Console.WriteLine("Stringfu:  " + parent_temp);
                                Dictionary<String, int> parentconcepts = Conceptprocess.getConcept(parent_temp);
                                //   Console.WriteLine("概念词个数:  " + parentconcepts.Count);
                                parent_score = candscortor.computeScore(parent_temp) + Conceptprocess.getConceptscore(sampleconcepts, parentconcepts);
                                //    Console.WriteLine("组合分2:  " + candscortor.computeScore(temp));
                                //    Console.WriteLine("概念分2:  " + Conceptprocess.getConceptscore(sampleconcepts, parentconcepts));
                                //    Console.WriteLine("2222222得分:  " + parent_score);
                            }
                        }
                        else {
                            parent_score = -1;  //如果父节点的长度变得太大则也不要
                        }


                        
                    }
                   // Console.WriteLine("父节点得分:  " + parent_score + "子节点得分:  " + score);
                    while (parent_score >= score)  //如果父节点的评分高于子节点则往上嵌套查找；
                    {
                       // Console.WriteLine("父节点得分:  " + parent_score+ "子节点得分:  "+ score);
                        current = parent;  //当前节点为评分高的节点；
                        temp = parent_temp;
                        parent = current.Parent;
                        score = parent_score;
                        //跟新parent_score；初始时 parent_score = 0;parent_temp = null;                   
                        parent_score = -1;
                        parent_temp = null;
                        if (parent != null)
                        {
                            parent_temp = Regex.Replace(HtmlElementprocess.getElementString(parent), "\\s+", " ");
                        }
                        if (parent_temp != null && parent_temp.Length > 0) //父节点评分
                        {
                            if (candscortor.lenscore(parent_temp, candscortor.getavglen()) > 0.4)
                            {
                                if (parent_temp.Equals(temp))
                                {
                                    parent_score = score;
                                }
                                else {
                                    Dictionary<String, int> parentconcepts = Conceptprocess.getConcept(parent_temp);
                                    parent_score = candscortor.computeScore(parent_temp) + Conceptprocess.getConceptscore(sampleconcepts, parentconcepts);
                                }
                            }
                            else {
                                parent_score = -1;  //如果父节点的长度变得太大则也不要
                            }
                        }
                        
                    } //跳出while循环时为说明current记录的节点大于父节点的打分；
                    Console.WriteLine("总分" + score);
                    if (candidates.ContainsKey(current))
                    {
                        candidates[current] = score;
                    }
                    else {
                        candidates.Add(current, score);
                        candidates.Remove(key);
                    }                  
                }               
            }
            DictonarySort(candidates);
            DateTime afterDT = System.DateTime.Now;
            TimeSpan ts = afterDT.Subtract(beforDT);
            Console.WriteLine("DateTime总共花费{0}ms.", ts.TotalMilliseconds);
        }

        private void DictonarySort(Dictionary<HtmlElement, double> dic)
        {
            var dicSort = from objDic in dic orderby objDic.Value descending select objDic;
            foreach (KeyValuePair<HtmlElement, double> kvp in dicSort)
                Console.Write(kvp.Key + "：" + kvp.Value + "<br />");
        }
       
    }
}
