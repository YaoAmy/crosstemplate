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
        public SelectCandidate(List<HtmlElement> leaf_nodes)
        {
            candscortor.setanchor(); 
            SetCandidate(leaf_nodes);
        }
        public void select(String sampletext) //对candidate打分；
        {
            Dictionary<String, int> sampleconcepts = Conceptprocess.getConcept(sampletext);
            List<HtmlElement> keylist = new List<HtmlElement>();
            keylist.AddRange(candidates.Keys);
            foreach (HtmlElement key in keylist)
            {
                
                double score = 0;  //当前节点评分
                double parent_score = -1;  //父节点评分；
                HtmlElement current = key;  //当前节点
                HtmlElement parent = current.Parent; //父节点
                String temp = Regex.Replace(key.InnerText.Trim(), "\\s+", " ");  //当前节点文本
                
                if (candscortor.lenscore(temp, candscortor.getavglen()) < 0.25) //过滤掉一些明显长度分过低的节点
                {
                    candidates.Remove(key);
                }
                else
                { //计算概念分一定要样本在前，待评节点在后，因为需要更具样本生成待评价节点概念向量
                    Dictionary<String, int> currentconcepts = Conceptprocess.getConcept(temp); 
                    score = candscortor.computeScore(temp)+ Conceptprocess.getConceptscore(sampleconcepts,currentconcepts);  //当前节点评分
                    String parent_temp = null; //如果存在父节点则找出父节点文本；
                    if (parent != null)
                    {
                        parent_temp = Regex.Replace(parent.InnerText.Trim(), "\\s+", " ");
                    }

                    if (parent_temp != null && parent_temp.Length > 0) //父节点评分
                    {
                        if (parent_temp.Equals(temp))
                        {
                            parent_score = score;
                        }
                        else {
                            Dictionary<String, int> parentconcepts = Conceptprocess.getConcept(parent_temp);
                            parent_score = candscortor.computeScore(parent_temp)+ Conceptprocess.getConceptscore(sampleconcepts, parentconcepts);
                        }
                    }

                    while (parent_score >= score)  //如果父节点的评分高于子节点则往上嵌套查找；
                    {
                        current = parent;  //当前节点为评分高的节点；
                        temp = parent_temp;
                        parent = current.Parent;
                        score = parent_score;
                        //跟新parent_score；初始时 parent_score = 0;parent_temp = null;                   
                        parent_score = -1;
                        parent_temp = null;
                        if (parent != null)
                        {
                            parent_temp = Regex.Replace(parent.InnerText.Trim(), "\\s+", " ");
                        }
                        if (parent_temp != null && parent_temp.Length > 0) //父节点评分
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
                    } //跳出while循环时为说明current记录的节点大于父节点的打分；
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
        }

        private void DictonarySort(Dictionary<HtmlElement, double> dic)
        {
            var dicSort = from objDic in dic orderby objDic.Value descending select objDic;
            foreach (KeyValuePair<HtmlElement, double> kvp in dicSort)
                Console.Write(kvp.Key + "：" + kvp.Value + "<br />");
        }
    }
}
