using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace LogAnalyzer
{
    static class Constants
    {
        public const string xmlFileDelimiterPath = "DelimiterHeader.xml";
        public const string xmlFileLogHeaderPath = "LogHeader.xml";
    }

    public class BaseData
    {
        public static readonly List<string> strFileDelimiterList;       //따로 파일을 만들 때를 판단하기 위한 구분자
        public static readonly List<string> strLogHeaderList;           //컨텐츠별 로그 헤더

        static BaseData()
        {
            strFileDelimiterList = new List<string>();

            //현 실행 파일 경로 위치
            string CurrentPath = Environment.CurrentDirectory;

            //파일 분리 구분자
            string xmlFileDelimiterPath = CurrentPath + "/" + Constants.xmlFileDelimiterPath;

            XmlDocument xmlFileDelimiter = new XmlDocument();
            
            //Parse Xml data
            xmlFileDelimiter.Load(xmlFileDelimiterPath);
            
            XmlNodeList xnList = xmlFileDelimiter.SelectNodes("/Delimiter/Val");

            foreach (XmlNode xmNode in xnList)
            {
                BaseData.strFileDelimiterList.Add(xmNode.InnerText);
                Console.Write(xmNode.InnerText);
            }

            //컨텐츠 별 로그 헤더
            string xmlFileLogHeaderPath = CurrentPath + "/" + Constants.xmlFileLogHeaderPath;

            XmlDocument xmlLogHeader = new XmlDocument();

            //Parse Xml data
            xmlFileDelimiter.Load(xmlFileLogHeaderPath);

            xnList = xmlFileDelimiter.SelectNodes("/Delimiter/Val");

            foreach (XmlNode xmNode in xnList)
            {
                BaseData.strLogHeaderList.Add(xmNode.InnerText);
                Console.Write(xmNode.InnerText);
            }
        }

        public int GetFileDelimiterListSize() { return strFileDelimiterList.Count(); }
        public int GetLogHeaderListSize() { return strLogHeaderList.Count(); }

        public string getFileDelimiterListComponent(int idx) { return strFileDelimiterList[idx]; }
    }

    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>

        [STAThread]
        static void Main()
        {
            BaseData cBaseData = new BaseData();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
