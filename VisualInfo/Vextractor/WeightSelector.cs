using mshtml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vextractor
{
    class WeightSelector
    {
        Weightadjust weightadjuster = new Weightadjust(); //设置存放权重系数
        Dictionary<HtmlElement, double> candidates = new Dictionary<HtmlElement, double>();//存放待评价节点
        public void SetCandidate(List<HtmlElement> leaf_nodes)
        {
            candidates.Clear();   //非String型的候选用candidates作为容器
            foreach (HtmlElement ele in leaf_nodes)
            {
                candidates.Add(ele, 0);
            }

        }
        public WeightSelector(String site,String mark,int samplenum) //初始化权重系数矩阵
        {
            weightadjuster.computeParentweight(site, mark,samplenum);
            weightadjuster.computeCurrentweight(site, mark,samplenum);
            weightadjuster.computeFirstchildweight(site, mark,samplenum);
            Console.WriteLine("打印权重系数   "+ samplenum);
            weightadjuster.show();
        }
        private String[] getTagandAttr(HtmlElement ele)
        {
            String[] temp = new String[4];
            IHTMLElement e = (mshtml.IHTMLElement)ele.DomElement;
            HtmlElement parent = ele.Parent;
            HtmlElement firstchild = ele.FirstChild;
            if (parent != null)
            {
                temp[0] = parent.TagName + " " + "class=" + parent.GetAttribute("className");
            }
            else {
                temp[0] = "0";
            }
            temp[1] = ele.TagName + " " + "class=" + ele.GetAttribute("className");
            if (firstchild != null)
            {
                temp[2] = firstchild.TagName + " " + "class=" + firstchild.GetAttribute("className");
            }
            else {
                temp[2] = "0";
            }
            Match m = Regex.Match(ele.OuterHtml, @"\<.*?\>");
            if (m.Success)
            {
                temp[3] = m.Value;
            }
            else {
                temp[0] = "0";
            }
            //   Console.WriteLine(Regex.Match(ele.OuterHtml, @"\<.*?\>"));           
            return temp;
        }
        public double Score(HtmlElement ele) //对传入的HtmlElement计算得分
        {
            String[] tag_attr = getTagandAttr(ele);
            String parent = tag_attr[0];  //提取父子、当前模式
            String current = tag_attr[1];
            String child= tag_attr[2];
            return weightadjuster.parentscore(parent) +weightadjuster.currentscore(current) +weightadjuster.firstchildscore(child);
        }
        public double score(String parent,String current,String child) //对传入的HtmlElement pattern计算得分
        {
            return weightadjuster.parentscore(parent) + weightadjuster.currentscore(current) + weightadjuster.firstchildscore(child);
        }
        public IEnumerable<KeyValuePair<HtmlElement, double>> select(int num) //传入的是提取的节点数目
        {
            DateTime beforDT = System.DateTime.Now;
            Console.Write("开始计算评分");
            int test = 0;
            List<HtmlElement> keylist = new List<HtmlElement>();
            keylist.AddRange(candidates.Keys); //candidates需要调用SetCandidate先设置；
            foreach (HtmlElement key in keylist)
            {
                test++;
                Console.WriteLine("正在计算第" + test + "个");
                double score = 0;  //当前节点评分
                double parent_score = -1;  //父节点评分；
                HtmlElement current = key;  //当前节点
                HtmlElement parent = current.Parent; //父节点             

                //if (weightadjuster.currentscore(current.TagName + " " + "class=" + current.GetAttribute("className")) <= 0)  //过滤掉当前节点得分为0的节点
                //{
                //    candidates.Remove(key);
                //}
              //  else
             //   {
                    score = Score(current);
                    if (parent != null)
                    {
                        //父节点字符串需要进行处理
                        parent_score = Score(parent);
                    }
                    while (parent_score >= score)  //如果父节点的评分高于子节点则往上嵌套查找；
                    {
                        current = parent;
                        parent = current.Parent;
                        score = parent_score;
                        parent_score = -1;
                        if (parent != null)
                        {
                            parent_score = Score(parent);
                        }
                    }
                    //跳出while循环时为说明current记录的节点大于父节点的打分；
                    Console.WriteLine("总分" + score);
                    if (candidates.ContainsKey(current)) //记录节点的得分
                    {
                        candidates[current] = score;
                    }
                    else {
                        candidates.Add(current, score);
                        candidates.Remove(key);
                    }
               // }
            }
            var dicSort = (from objDic in candidates where objDic.Value==candidates.Values.Max() select objDic);
            DateTime afterDT = System.DateTime.Now;
            TimeSpan ts = afterDT.Subtract(beforDT);
            Console.WriteLine("DateTime总共花费{0}ms.", ts.TotalMilliseconds);
            return dicSort;
        }
    }
}
