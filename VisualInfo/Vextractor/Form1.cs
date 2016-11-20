using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Data.Entity.Validation;
using mshtml;
using HtmlAgilityPack;
namespace Vextractor
{
    public partial class Form1 : Form
    {
        int count = 0;
        int id = 777;

        htmlinfoEntities htmlpage = new htmlinfoEntities();
        private Dictionary<HtmlElement, String> adictionary = new Dictionary<HtmlElement, String>();//存放分割完的所有节点上
                                                                                                    //    private Dictionary<HtmlElement, String> tdictionary = new Dictionary<HtmlElement, String>();//存放所有的纯文本类文本
        public Form1()
        {
            InitializeComponent();
            this.webBrowser1.ScriptErrorsSuppressed = true;

        }



        private void extractv_Click(object sender, EventArgs e)
        {
            // this.webBrowser1.DocumentText = htmlpage.detailinfo.Find(1).html;
            //  Regex.Match("a.b?c。d？", "[?.。？]");          
            // this.webBrowser1.Navigate("http://ieeexplore.ieee.org/search/searchresult.jsp?newsearch=true&queryText=information%20extraction&fname=&lname=&title=&volume=&issue=&spage=");
            this.webBrowser1.Navigate("http://www.tesco.com/direct/samsung-ue40ku6020-smart-4k-ultra-hd-40-inch-led-tv-with-freeview-hd/356-2551.prd?skuId=356-2551");
            Docfinished.Start();
            //  nextpage();
            // Console.Out.WriteLine(id);
        }
        private void nextpage()
        {
            id++;
            urltable ut = htmlpage.urltable.Find(id);
            if (id >= 2516)
            {
                Application.Exit();
            }
            if (ut != null)
            {
                count = 0;//解析下一页时，count必须清零
                this.webBrowser1.Navigate(ut.url);
                Console.Out.WriteLine("解析的" + id + "对应url：" + ut.url);
                Docfinished.Start();
            }
            else
            {
                nextpage();
            }

        }
        //文档加载完之后，对页面进行提取
        private void Docfinished_Tick(object sender, EventArgs e)
        {
            count++;
            //判断关键的html元素是否被加载

            if (this.webBrowser1.ReadyState == WebBrowserReadyState.Complete || (count > 30 && count < 35 && this.webBrowser1.ReadyState == WebBrowserReadyState.Interactive))
            {
                Docfinished.Stop();
                //在每一次处理不同url前需要清空原来存放的节点数据
                adictionary.Clear();
                int i = 0;
                //对于不同的网站meta和title信息可能需要重新抽取
                string title = null;
                string meta = null;
                int vimglink = 0;  //图片 
                int vnlink = 0;  //链接
                int nstext = 0; //短文本
                int nltext = 0;   //长文本
                int sc = 0;  //长文本块字符串
                int c = 0;   //短文本块字符串
                int nparagraph = 0;//段落数
                int len = 0;  //文本长度
                int nlink = 0;            //链接数目
                int nimg = 0;             //图片数目
                int nt = 0; //规整的数据结构：表格、列表
                            /*  //写入文件测试
                              StreamReader sr = new StreamReader(webBrowser1.DocumentStream);
                              StreamWriter sw = new StreamWriter(@"D:\test\test.html");
                              sw.Write(sr.ReadToEnd());
                              sw.Flush();
                              sw.Close();
                              sr.Close();
                              return;*/
                            //抽取京东页面的title和meta
                foreach (HtmlElement el in this.webBrowser1.Document.GetElementsByTagName("head"))
                {
                    //获取title信息
                    foreach (HtmlElement t in el.GetElementsByTagName("title"))
                    {
                        title = t.InnerText;
                        //Console.Out.WriteLine("title :" + title.InnerText);
                        break;
                    }
                    //获取meta标签信息中的描述信息

                    foreach (HtmlElement m in el.GetElementsByTagName("meta"))
                    {
                        if (m.GetAttribute("name").Equals("keywords"))
                        {
                            meta = m.GetAttribute("content");
                            //  Console.Out.WriteLine("meta :" + meta.GetAttribute("content"));
                            break;
                        }
                        if (m.GetAttribute("name").Equals("description"))
                        {
                            meta = m.GetAttribute("content");
                            //  Console.Out.WriteLine("meta :" + meta.GetAttribute("content"));
                            break;
                        }
                    }
                } //对head信息的提取
                //Console.Out.WriteLine(this.webBrowser1.Document.Body.InnerText);
                //检测body节点中有哪些节点需要分割，打印无需分割的节点
                //Console.Out.WriteLine("打印出孩子节点的数量 :"+this.webBrowser1.Document.Body.OuterHtml);
                //Console.Out.WriteLine("需要分割??? :"+ needsplit(this.webBrowser1.Document.Body));
                Vextra(this.webBrowser1.Document.Body);   //该函数是递归的                                
                foreach (HtmlElement ele in adictionary.Keys)
                {
                    i++;
                    String[] type = judgetype(ele);
                    // Console.Out.WriteLine("第 :" + i+"个无需分割节点为~~~~~~~~~~~~~~~~~~~~~~类型为"+ type);
                    Console.Out.WriteLine("第" + i + "个元素   " + type[0] + "   " + adictionary[ele]);    //打印节点内部信息     
                    if (type[0].Equals("imglink"))
                    {
                        vimglink++;
                    }
                    else
                        if (type[0].Equals("alink")) vnlink++;
                    else
                        if (type[0].Equals("stext"))
                    {
                        nstext++; //短文本
                        sc = sc + int.Parse(type[1]);

                    }
                    else
                    {
                        nltext++;
                        c = c + int.Parse(type[1]);
                    }
                }
                /*
                   Console.Out.WriteLine("图片：" + vimglink);
                   Console.Out.WriteLine("链接：" + vnlink);
                   Console.Out.WriteLine("短文本：" + nstext);
                   Console.Out.WriteLine("长文本：" + nltext);
                if (nstext > 0)
                {
                    Console.Out.WriteLine("短文本长度" + sc);
                    Console.Out.WriteLine("短文本平均长度：" + (float)sc / (float)nstext);
                }
                else
                {
                    Console.Out.WriteLine("短文本平均长度：" + 0);
                }
                if (nltext > 0)
                {
                    Console.Out.WriteLine("长文本长度" + c);
                    Console.Out.WriteLine("长文本平均长度：" + (float)c / (float)nltext);
                }
                else {
                    Console.Out.WriteLine("长文本平均长度：" + 0);
                } */





                /*

                            //上述为对视觉信息的处理              
                            nparagraph= this.webBrowser1.Document.Body.GetElementsByTagName("p").Count; 
                            len=Regex.Replace(this.webBrowser1.Document.Body.InnerText, "\\s*", "").Length;
                            nlink = this.webBrowser1.Document.Body.GetElementsByTagName("a").Count;
                            nimg = this.webBrowser1.Document.Body.GetElementsByTagName("img").Count;
                            nt= this.webBrowser1.Document.Body.GetElementsByTagName("table").Count+ this.webBrowser1.Document.Body.GetElementsByTagName("ul").Count+this.webBrowser1.Document.Body.GetElementsByTagName("ol").Count;
                    */
                /*
                Console.Out.WriteLine("nspecical：" + nparagraph);
                Console.Out.WriteLine("nspecical：" + len);
                Console.Out.WriteLine("nspecical：" + nlink);
                Console.Out.WriteLine("nspecical：" + nimg);
                Console.Out.WriteLine("nspecical：" + nt); */
                //上述为对html网页元素的分析

                //所有操作处理完之后进行数据库存储
                /*  
                   if ((vimglink+ vnlink+ nstext+ nltext)<3 || title==null) //认为分块不成功，或者页面没有正确加载，舍弃
                   {
                       //添加直接解析下一页代码
                       nextpage();
                       MessageBox.Show("网络问题，未能正确解析！！！！！！！！！！！！！");
                       Application.Exit();
                       return;
                   }
                   urltable ut = htmlpage.urltable.Find(id);
                   detailinfo detail = new detailinfo();
                   //文档的url（修）
                   detail.url = ut.url;
                   detail.site = ut.site;
                   //文档的类别，根据抽取的内容来定（修）
                   detail.mark = ut.mark;
                   detail.title = title;
                   detail.meta = meta;
                   detail.vimglink = vimglink;
                   detail.vnlink = vnlink;
                   detail.nstext = nstext;
                   detail.nltext = nltext;
                   detail.nlink = nlink;
                   detail.nparagraph = nparagraph;
                   detail.len = len;
                   detail.nimg = nimg;
                   detail.nt = nt;
                   detail.html = this.webBrowser1.DocumentText;  */
                //  Console.Out.WriteLine("url" + ut.url);
                //  Console.Out.WriteLine("site：" + ut.site);
                // Console.Out.WriteLine("mark：" + ut.mark);
                // Console.Out.WriteLine("title：" + title);
                //  Console.Out.WriteLine("meta：" + meta);
                // Console.Out.WriteLine("vimglink：" + vimglink);
                // Console.Out.WriteLine("vnlink：" + vnlink);
                // Console.Out.WriteLine("nstext：" + nstext);
                // Console.Out.WriteLine("nltext：" + nltext);
                // Console.Out.WriteLine("nlink：" + nlink);
                //Console.Out.WriteLine("nparagraph：" + nparagraph);
                // Console.Out.WriteLine("len：" + len);
                // Console.Out.WriteLine("nimg：" + nimg);
                // Console.Out.WriteLine("nt：" + nt);

                /*
                try   //写回到数据库中
                {
             
                   htmlpage.detailinfo.Add(detail);
                    htmlpage.SaveChanges();
                }
                catch(DbEntityValidationException dbEx) {
                    MessageBox.Show(dbEx.Message);
                }*/

                //        nextpage();
                return;
                // htmlpage.detailinfo.Find(1).html;

            }//文档加载完成后的处理   


            if (count >= 35) //count >=35仍然没有加载完成则直接取下一进行抽取
            {
                Docfinished.Stop();
                Console.Out.WriteLine("此处记得添加解析不成功是的下一页代码");
                nextpage();
                return;

            }
        }

        public void Vextra(HtmlElement e)
        {
            //innertext中含有该节点中（包含子节点的子节点中）的所有文本
            //利用视觉信息提取相关值   
            if (e.Children.Count == 1)
            {
                //如果有只有一个孩子，剥离上层父节
                HtmlElement a = null;
                foreach (HtmlElement ch in e.Children)
                {
                    a = ch;
                }
                e = a;
            }
            if (e.Children.Count > 0 && needsplit(e)) //如果当前节点不含有任何文本则删除该节点
            {

                foreach (HtmlElement ch in e.Children)
                {
                    Vextra(ch);
                }
            }
            else {
                if (e.InnerText != null)
                {  //&& !temp.Contains("window._") && !temp.Contains("function()")
                    string temp = e.InnerText;
                    if (e.TagName != "SCRIPT" && e.TagName != "STYLE" && e.TagName != "NOSCRIPT")
                    {
                        // 处理掉子节点中的script和style代码,将待删除字符串从大到小的顺序排列，再删除
                        ArrayList script = new ArrayList();
                        foreach (HtmlElement ec in e.GetElementsByTagName("script"))
                        {
                            if (ec.InnerText != null)
                            {

                                String delt = ec.InnerText.Trim(); //必须除掉空前后格，否则可能出现不匹配                                      
                                if (script.Count > 0 && script[0].ToString().Length < delt.Length)
                                {
                                    script.Insert(0, delt);
                                }
                                else { script.Add(delt); }
                            }
                        }
                        foreach (HtmlElement ec in e.GetElementsByTagName("style"))
                        {
                            if (ec.InnerText != null)
                            {
                                String delt = ec.InnerText.Trim();
                                if (script.Count > 0 && script[0].ToString().Length < delt.Length)
                                {
                                    script.Insert(0, delt);
                                }
                                else { script.Add(delt); }
                            }
                        }
                        foreach (HtmlElement ec in e.GetElementsByTagName("noscript"))
                        {
                            if (ec.InnerText != null)
                            {
                                String delt = ec.InnerText.Trim();
                                if (script.Count > 0 && script[0].ToString().Length < delt.Length)
                                {
                                    script.Insert(0, delt);
                                }
                                else { script.Add(delt); }
                            }
                        }
                        foreach (String del in script)
                        {
                            temp = temp.Replace(del, "");
                            // Console.Out.WriteLine("找到script" + del);
                        }
                        temp = Regex.Replace(temp, "\\s{2,}", "\n");
                        if (Regex.Replace(temp, "\\s*", "").Length > 30) //假设一个英文单词的长度为5少于6个单词的节点丢弃（认为分割粒度太细，可能只是突出表示的子标签）
                        {
                            //  test++;
                            // Console.Out.WriteLine("第"+test+"个元素的标签是"+e.TagName);                    
                            adictionary.Add(e, temp.Trim());
                        }
                    }
                }
            }
        }
        public bool needsplit(HtmlElement e) //输入节点e一定有孩子，根据节点的背景、字体等信息决定是否需要划分
        {
            HashSet<int> setsplitNum = new HashSet<int>();
            ArrayList listbc = new ArrayList(); //背景色
            ArrayList listfc = new ArrayList(); //字体色
            ArrayList listfs = new ArrayList(); //字体大小
            ArrayList listfst = new ArrayList();//字体信息
            foreach (HtmlElement ch in e.Children) //对e中的所有直接孩子节点提取背景色等信息
            {
                mshtml.IHTMLElement2 ch2 = (mshtml.IHTMLElement2)ch.DomElement;
                IHTMLCurrentStyle chstyle = ch2.currentStyle;
                listbc.Add(chstyle.backgroundColor);
                listfc.Add(chstyle.color);
                listfs.Add(chstyle.fontSize);
                listfst.Add(chstyle.fontStyle);
            }
            //如果有孩子节点的上述样式有所不同则需要分割  
            foreach (Object value in listbc)
            {
                if (listbc[0] != value) //如果有一个是不一样的则需要分割
                    return true;
            }
            foreach (Object value in listfc)
            {
                if (listfc[0] != value) //如果有一个是不一样的则需要分割               
                    return true;
            }
            foreach (Object value in listfs)
            {
                if (listfs[0] != value) //如果有一个是不一样的则需要分割
                    return true;
            }
            foreach (Object value in listfst)
            {
                if (listfst[0] != value) //如果有一个是不一样的则需要分割
                    return true;
            }
            return false;
        }
        //对提取出的node进行分类
        //链接标签（展示链接i、词链接a）:如果节点中的图片和链接的比例达到0.6则认为是展示链接，否则认为是普通链接节点（如果含有多个展示节点则更可能是一个电商网站，设此用于对区分不同类型网页有所倾斜）
        //文本节点t：如果不是链接节点则认为是文本节点
        private String[] judgetype(HtmlElement e)
        {
            String[] temp = new String[2];
            String alen = "";  //节点中所有链接文本
            String tlen = ""; //节点所有文本
            int counta = 0;//含有的a链接的数目
            HashSet<string> ac = new HashSet<string>();
            int counti = 0;//含有的img标签的数目
            foreach (HtmlElement alink in e.GetElementsByTagName("a"))
            {
                //alen的文本可能包裹在子节点中，所以需要加一步判断
                HtmlElementCollection aelc = alink.Children;
                if (aelc.Count > 0)
                {
                    foreach (HtmlElement ael in aelc)
                    {
                        alen = alen + "\n" + ael.InnerText;
                    }

                }
                else {
                    alen = alen + "\n" + alink.InnerText;
                }
                ac.Add(alink.GetAttribute("href"));
            }
            counta = ac.Count;
            tlen = e.InnerText;
            //  Console.Out.WriteLine("linklinklink:\n"+ alen);
            //  Console.Out.WriteLine("ttttttttttt:\n" + tlen);
            tlen = Regex.Replace(tlen, "\\s*", "");
            alen = Regex.Replace(alen, "\\s*", "");
            // Console.Out.WriteLine("链接占是" + ((float)alen.Length));
            // Console.Out.WriteLine("全文本是" + ((float)tlen.Length));

            // Console.Out.WriteLine("链接占全文本比值是"+((float)alen.Length / (float)tlen.Length));
            if (((float)alen.Length / (float)tlen.Length) > 0.7)   //自己设的阈值,大于该阈值时认为是链接节点
            {
                counti = e.GetElementsByTagName("img").Count;
                //Console.Out.WriteLine("链接有！！！！！！" + counta);
                //Console.Out.WriteLine("图片有！！！！！！" + counti);
                //Console.Out.WriteLine("比例！！！！！！！" + ((float)counti / (float)counta));
                // Console.Out.WriteLine(e.OuterHtml);            
                if (((float)counti / (float)counta) > 0.45) //一张图片很可能对应两个链接
                {
                    temp[0] = "imglink";
                    temp[1] = null;
                    return temp;
                }
                else {
                    temp[0] = "alink";
                    temp[1] = null;
                    return temp;
                }
            }
            else {
                //根据段落中的包含的语句数目确定为长文本还是短文本
                // tlen.Length-tlen.Replace(".","").Length- tlen.Replace("?", "")
                // Console.Out.WriteLine("wenbneduan zhong 匹配的字符啊啊啊啊 啊 啊 啊数目"+ Regex.Matches(tlen, "[?.。？\"]").Count);
                if (Regex.Matches(tlen, "[?.。？\"]").Count < 5)
                {
                    temp[0] = "stext";
                    temp[1] = tlen.Length.ToString(); ;
                    return temp;
                }
                else
                {
                    temp[0] = "ltext";
                    temp[1] = tlen.Length.ToString();
                    return temp;
                }
            }
        }

        private void splitNum(ArrayList list, HashSet<int> setsplitNum)
        {
            HashSet<Object> set = new HashSet<Object>();
            ArrayList repeatElements = new ArrayList();
            foreach (Object value in list)
            {
                if (set.Contains(value))
                {
                    // 重复元素
                    repeatElements.Add(value);
                }
                else {
                    set.Add(value);
                }
            }
            foreach (Object value in list)
            {
                if (!repeatElements.Contains(value))
                {
                    setsplitNum.Add(list.IndexOf(value)); //将于所有其他孩子节点的某一属性不同的值是需要分割的节点
                }
            }
        }

        //________________________________________________________________________________________________________
        //第二部分实验 (模块1：根据模板抽取信息)
        private void divide_Click(object sender, EventArgs e)  //得到模板节点
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            String txt = this.Domtext.Text;
            doc.LoadHtml(txt);
            HtmlNode node = doc.DocumentNode.FirstChild;
            HtmlAttributeCollection attrcollection = node.Attributes;
            String tag = node.Name;



            HtmlAttributeCollection attrs = node.Attributes;
            foreach (var item in attrs)
            {
                Console.WriteLine(item.Name + " : " + item.Value);    //提取节点的属性集合
            }
            this.Domtext.Text = tag + " " + node.HasChildNodes;
            Console.WriteLine("文本信息：  " + node.InnerText);
            /* foreach (HtmlNode link in doc.DocumentNode.ChildNodes)
            {
                // this.Domtext.Text=link.InnerText;
                // Console.Out.WriteLine(link.InnerText); 

             } */
        }

        HtmlAttributeCollection attrs;//指定抽取的节点属性
        String tag;//指定抽取的节点标签
        int extractid= 133808; //设置记录抽取id;对应urltable中；
        int endid= 133817;//设置抽取结束的id；
        int count2 = 0;
        String site;//记录来源
        String extracturl;//记录正在抽取的url
        String mark;
        //根据文本框中信息抽取指定节点
        private void extract_info_Click(object sender, EventArgs e)
        {
            Template tm = new Template();  //获取模板
            tm.createTem(this.Domtext.Text);  //将模板放在Domtext中；
            attrs = tm.getattr();   //设置抽取属性；
            tag = tm.gettag();   //设置抽取标签；

            // tm.Extract("http://www.walmart.com/ip/VTech-CS6919-15-DECT-6.0-Expandable-Cordless-Phone-with-Caller-ID-and-Handset-Speakerphone-Red/45074431");
            //  this.webBrowser1.Navigate("http://www.walmart.com/ip/VTech-CS6919-15-DECT-6.0-Expandable-Cordless-Phone-with-Caller-ID-and-Handset-Speakerphone-Red/45074431");
            nextextraturl(extractid);

          //  partition_timer.Start();
        }
        private void nextextraturl(int id)
        {
            urltable ut = htmlpage.urltable.Find(id);
            if (id >endid)   //大于不包括需要抽取的节点时程序退出
            {
                Application.Exit();
            }
            if (ut != null)
            {
                count2 = 0;//解析下一页时，count2必须清零
                site = ut.site;
                extracturl = ut.url;
                mark = ut.mark;
                this.webBrowser1.Navigate(extracturl);

              //  Console.Out.WriteLine("解析的" + id + "对应url：" + ut.url);
                extract_timer.Start(); //抽取timer启动；
            }
            else
            {
                extractid++;  //如果该id对应的urltable元组不存在则记录抽取的extractid自增，抽取下一条；
                nextextraturl(extractid);
            }
        }
        private void extract_timer_Tick(object sender, EventArgs e)
        {
            if (count2 >= 1000) //count >=35仍然没有加载完成则直接取下一进行抽取
            {
                extract_timer.Stop();
                Console.Out.WriteLine("此处记得添加解析不成功是的下一页代码");
                extractid++;
                nextextraturl(extractid);
                return;

            }
            if (this.webBrowser1.ReadyState == WebBrowserReadyState.Complete || this.webBrowser1.ReadyState == WebBrowserReadyState.Interactive)
            {
                extract_timer.Stop();              
                //string strPath = @"D:\testtest.txt";
                //string value = this.webBrowser1.Document.Body.OuterHtml;

                //if (!Directory.Exists(Path.GetDirectoryName(strPath)))
                //{
                //    Directory.CreateDirectory(Path.GetDirectoryName(strPath));
                //}
                //       File.AppendAllText(strPath, value, Encoding.Default);
                foreach (HtmlElement el in this.webBrowser1.Document.Body.GetElementsByTagName(tag))
                {
                    bool isRight = true;
                   // Console.Out.WriteLine("tag"+ tag +"attr:  "+ attrs[0].Value);
                    foreach (var item in attrs)
                    {
                        // Console.Out.WriteLine("name   " + item.Name);
                        //  Console.Out.WriteLine("value   " + item.Value);
                        String attr = item.Name;
                        if (item.Name.Equals("classname") || item.Name.Equals("class"))
                        {
                            attr = "className";
                        }
                        //    Console.Out.WriteLine(el.GetAttribute(attr));

                        if (!item.Value.Equals(el.GetAttribute(attr)))
                        {
                            isRight = false;  //如果有一个节点属性不能匹配则结束循环不在匹配后续属性
                            break;
                        }
                    }
                    if (isRight)  //如果所有节点属性都匹配成功则进行抽取
                    {
                         
                        if (el.InnerText!=null&& el.InnerText.Trim().Length>0)
                        {                           
                            //int urlid = extractid;
                            //String info =Regex.Replace(el.InnerText.Trim(),"\\s+"," ");                           
                            ////site、extracturl已经在提取url是设置；
                            //String doc = this.webBrowser1.DocumentText;
                            //Console.Out.WriteLine("urlid:    "+urlid + "抽取信息： " + info + " 网站： " + site + "来源url " + extracturl + "分类： " + mark);
                            original orig = new original();
                            orig.info= Regex.Replace(el.InnerText.Trim(), "\\s+", " ");
                            orig.mark = mark;
                            orig.site = site;
                            orig.url = extracturl;
                            orig.urlid = extractid;
                            orig.doc= this.webBrowser1.DocumentText;
                            htmlpage.original.Add(orig);
                            htmlpage.SaveChanges();
                        }
                        
                    }
                }
                extractid++;
                nextextraturl(extractid);
            }
            // public void extract
        }


        //______________________________________________________________________________________
        //                       分块部分
        SelectCandidate selector = new SelectCandidate();
        //int originalid; //设置待匹配的概念节点的id
        //private String getSample(int id) //从original表格中得到样本用于概念计算
        //{
        //    return;
        //}
        private void Scoretor_Click(object sender, EventArgs e)
        {
            this.webBrowser1.Navigate("https://www.amazon.com/Unlocked-Android-MTK6572-Smartphone-598-0~1203-0MHz/dp/B016NX0C8M/ref=sr_1_3?ie=UTF8&qid=1463985735&sr=8-3&keywords=phone");
            partition_timer.Start();

        }
        private void partition_timer_Tick(object sender, EventArgs e)
        {
           
            if (this.webBrowser1.ReadyState == WebBrowserReadyState.Complete || this.webBrowser1.ReadyState == WebBrowserReadyState.Interactive)
            {
                partition_timer.Stop();
                Console.WriteLine(this.webBrowser1.Document.All.Count);
                Console.WriteLine(this.webBrowser1.Document.Body.All.Count);
                int i = 0;
                List<HtmlElement> candidateslist = new List<HtmlElement>();
                foreach (HtmlElement ele in this.webBrowser1.Document.All)
                {
                    if (ele.Children.Count == 0&&ele.TagName!= "SCRIPT" && ele.TagName != "NOSCRIPT" && ele.TagName != "STYLE" && ele.InnerText != null)
                    {
                        
                        //Console.WriteLine(ele.TagName);
                        //Console.WriteLine(i+"   "+ele.InnerText);
                        if (Regex.Replace(ele.InnerText,"\\s+","").Length>20) //长度太小的直接不要,不需要做去脚本处理的原因为本身为叶子节点
                        {
                            candidateslist.Add(ele);
                            i++;
                        }
                        
                    }
                   
                }
                Console.WriteLine("总共选出的节点数目"+i); //总共的后选节点数目；
                selector.SetCandidate(candidateslist); //重新设置候选节点，每次调用会清除之前的候选节点
                selector.select("FIGO Atrium 5.5 - Dual Micro SIM Unlocked 16GB Smartphone - US & International GSM 4G");  //根据sampleString来计算概念分。


            }

        }

       
    }
}

