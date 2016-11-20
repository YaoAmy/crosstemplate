using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using mshtml;
namespace VisualInfo
{
    public partial class Form1 : Form
    {
      //  private bool completed=false;//是否加载完毕状态位。
    //    private int count=0; //用来记录网页的加载次数，每次导航到一个新的页面时count=0
     //   private int countcheck =100;//记录3秒钟前的count;
        public Form1()
        {
            InitializeComponent();
            //当触发DocumentCompleted 事件后执行函数go
            this.webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(Count);
            this.webBrowser1.ScriptErrorsSuppressed = true;
        }
    
        private void button1_Click(object sender, EventArgs e)
        {
            String[] temp = new String[] { "http://www.amazon.com/gp/product/B00U3FPN4U?ref_=gb1h_img_m-5_9102_c5f99c1d&smid=ATVPDKIKX0DER",
            "http://www.walmart.com/cp/1231521",
            "http://www.ebay.com/itm/APPLE-IPHONE-6S-64GB-GOLD-UNLOCKED-BRAND-NEW-MKQQ2X-A-A1688-AUSLUCK-/181871187903",
            "http://www.cbsnews.com/news/iowa-michigan-state-big-10-final-top-25-ncaa-football-roundup/",
            "http://edition.cnn.com/2015/12/06/us/san-bernardino-shooting-what-we-know/index.html",
            "http://abcnews.go.com/International/stabbing-london-tube-station-treated-terrorist-incident-police/story?id=35600650"};
           
           
                System.Console.WriteLine("现在处理的是"+ temp[2]);
            this.webBrowser1.Navigate(temp[2]);


        }
        /*       
private void Savehtml(String html) {
    //获得字节数组  uu
    byte[] data = new UTF8Encoding();
    using (FileStream fsWrite = new FileStream(@"D:\1.html", FileMode.Append))
    {
        fsWrite.Write(data, 0, data.Length);
        fsWrite.Flush();
        fsWrite.Close();
    };
}*/
        private void Count(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

            if (e.Url.ToString() == this.webBrowser1.Url.ToString() && this.webBrowser1.ReadyState == WebBrowserReadyState.Complete)
            {
                //认为加载完成
                // test();
                // ExtractVinfo();
                IHTMLWindow7 win = (IHTMLWindow7)this.webBrowser1.Document.Window.DomWindow;
                foreach (HtmlElement temp in this.webBrowser1.Document.GetElementsByTagName("div"))
                {
                    if (temp.GetAttribute("id").Equals("ce-comp-lt-title-container"))
                    {

                    }
                }
            }
        }

       
        //每3秒检查count计数值是否改变，如果没有改变或者completed事件的url与navigate的url相同则认为加载完毕
       
        private void ExtractVinfo()
        {
            //每一个网页的视觉单位结构用hashtable存储
            Hashtable Vdomtext = new Hashtable();   //文本节点
            Hashtable Vdomlink = new Hashtable();  //链接节点
            Hashtable Vdompic = new Hashtable();   //图片节点

            //分析head视觉信息 
            String html = null;       //网页                                          
            String titletext = null;  //网页title
            String metatext = null;   //网页描述性信息
            //对于自定义节点，取得其中的信息视觉信息
            /*
            文本节点：节点块位置信息、字体大小、背景色、字体颜色
            链接节点：节点块位置信息、链接数目、链接本身（用于比对链接相似度）、锚文本
            图片节点：节点块位置信息、图片、对应title文本（本身是img标签，其上层节点是a标签）
            */
        
            // System.Console.WriteLine("title文字" + this.webBrowser1.DocumentTitle);
            /* 
              FileStream fs = new FileStream("D:/testhtml/"+0+".html",FileMode.Append);
              StreamWriter sw = new StreamWriter(fs);
              sw.Write(this.webBrowser1.DocumentText);
              sw.Flush();
              fs.Close();
              */
           // html = this.webBrowser1.DocumentText;
            titletext = this.webBrowser1.DocumentTitle;
            //以div节点为单位，对网页进行分割
            HtmlDocument segHtml = this.webBrowser1.Document;
            foreach (HtmlElement temp in segHtml.GetElementsByTagName("div"))
            {
                //对文本格式标记的处理
                if (temp.InnerHtml!=null&&!temp.InnerHtml.Contains("<p>")) {
                  //  segHtml.Write("<HTML><BODY>This is a new HTML document.</BODY></HTML>");
                  //  segHtml.OpenNew(false); 
                  //  System.Console.WriteLine(segHtml);
                    break;
                }
               //对链接标记的处理
               //对图像标记的处理

            }
            foreach (HtmlElement head in this.webBrowser1.Document.GetElementsByTagName("head"))
            {
                
                //网页描述性信息meta         
                foreach (HtmlElement minfo in head.GetElementsByTagName("meta"))
                {
                    if (minfo.GetAttribute("name").Equals("description"))
                    {  // 如果meta中已经有文字则首先要添加空格再拼接文字
                        if (metatext != null) {
                            metatext = metatext + " ";
                        }
                        metatext = metatext+ minfo.GetAttribute("content");
                    }
                    if ( minfo.GetAttribute("name").Equals("keywords"))
                    {
                        if (metatext != null)
                        {
                            metatext = metatext + " ";
                        }
                        metatext = metatext + minfo.GetAttribute("content");
                    }

                }
              //  System.Console.WriteLine("meta文字" + metatext)
            }
        }//ExtractVinfo()函数结束
        private bool textCount(HtmlElement node) {
            int p = 0;//记录段落个数
            int h = 0;//记录标题个数（可能出现段标题）
            int b = 0;//黑体字个数
            int i = 0;//斜体字个数
            
            foreach (HtmlElement t in node.GetElementsByTagName("b"))
            {
                i++;
            }
            foreach (HtmlElement t in node.GetElementsByTagName("b"))
            {
                b++;
            }
            foreach (HtmlElement t in node.GetElementsByTagName("p"))
            {
                p++;
            }
            foreach (HtmlElement t in node.GetElementsByTagName("h1"))
            {
                h++;
            }
            foreach (HtmlElement t in node.GetElementsByTagName("h1"))
            {
                h++;
            }
            foreach (HtmlElement t in node.GetElementsByTagName("h2"))
            {
                h++;
            }
            foreach (HtmlElement t in node.GetElementsByTagName("h3"))
            {
                h++;
            }
            foreach (HtmlElement t in node.GetElementsByTagName("h4"))
            {
                h++;
            }
            foreach (HtmlElement t in node.GetElementsByTagName("h5"))
            {
                h++;
            }
            foreach (HtmlElement t in node.GetElementsByTagName("h6"))
            {
                h++;
            }
            return false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.webBrowser1.Navigate("http://www.amazon.cn/gp/product/B00EEWR792?qid=1453031915&ref_=lp_1772386071_1_1&s=grocery&sr=1-1");          
        }
        private void test()
        {
            foreach (HtmlElement t in this.webBrowser1.Document.All)
            {
                //打印一个HtmlElement下的所有文本
                if (t.TagName != "HTML" && t.TagName != "SCRIPT" && t.TagName != "STYLE")
                {
                    String Stext = "";
                    String Mtext = "";  //锚文本
                //    System.Console.Write("aaaaaaaa：" + t.OuterHtml+ "\n");
                    Stext = t.InnerText;
                    System.Console.WriteLine("打印标签的信息：" + t.TagName);
                    foreach (HtmlElement text in t.GetElementsByTagName("A"))
                    {
                        Mtext = Mtext + text.InnerText;
                    }
                    //删除文本中的无效字符    
                    if (Stext != null) {

                        foreach (HtmlElement text in t.GetElementsByTagName("SCRIPT"))
                        {
                            
                            if (text.InnerText != null) {
                                Stext = System.Text.RegularExpressions.Regex.Replace(Stext, text.InnerText, "");
                                Mtext = System.Text.RegularExpressions.Regex.Replace(Mtext, text.InnerText, "");
                            }                           
                        }
                        if (!Stext.Equals(""))
                        {
                            System.Console.Write("所有文本：" + Stext + "\n");
                            System.Console.Write("锚文本：" + Mtext + "\n");
                        }

                    }
                                    
                }
               
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
