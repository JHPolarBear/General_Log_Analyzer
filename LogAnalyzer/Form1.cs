using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LogAnalyzer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {



        }

        private void File_Click(object sender, EventArgs e)
        {            
            Stream myStream = null;
            OpenFileDialog OpenLogDialog = new OpenFileDialog();

            OpenLogDialog.InitialDirectory = "c:\\";
            OpenLogDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            OpenLogDialog.FilterIndex = 2;
            OpenLogDialog.RestoreDirectory = true;

            if (OpenLogDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //파일 전체 경로
                    string path = OpenLogDialog.FileName.ToString();

                    //확장자 분리 - idx 0: 파일경로(확장자 제외) / idx: 1: 파일 확장자
                    string[] strPathSubString = path.Split('.');               


                    //파일 경로 확인 후 Stream Reader 생성
                    if (path != null)
                    {
                        StreamReader sr = new StreamReader(path);

                        int nRoundCnt = 1;  //로그 파일 안에 여러 라운드가 있을 수 있으므로 라운드 수 표기
                        int nHoleCnt = 1;   //라운드별 홀 카운트

                        int nFileCount = 1;
                        int nCrntFileDelimiterIdx = 0;
                                                                       
                        StreamWriter sw;

                        String strTempPath = strPathSubString[0] + "_temp" + nFileCount + "." + strPathSubString[1];

                        //먼저 한번 Peek
                        sr.Peek();

                        //최초 시작 파일의 어떤 종류인지 확인(파일 이어하기로 쓰여졌는지 아닌지)
                        string strStartLine = sr.ReadLine();

                        int nDelimiterStart = CheckFileDelimeter(strStartLine);

                        switch(nDelimiterStart)
                        {
                            case 0:
                                nCrntFileDelimiterIdx = 0;
                                break;


                            case 1:
                                nCrntFileDelimiterIdx = 1;
                                break;
                        }

                        //using (sw = new StreamWriter(strTempPath, true, System.Text.Encoding.GetEncoding("utf-8") ) );

                        sw = new StreamWriter(strTempPath, true, System.Text.Encoding.GetEncoding("utf-8"));

                        sw.WriteLine(strStartLine);

                        //로그 파일의 끝까지 읽음
                        while (sr.Peek() >= 0)
                        {
                            //일단 처음 한 줄을 읽어들입니다.
                            string strLine = sr.ReadLine();

                            //파일 구분자가 존재하는 지 확인
                            int nDelimiterIdx = CheckFileDelimeter(strLine);

                            //새로운 구분자가 나타나면 현재 작성하던 파일을 적당한 파일 명으로 바꾸고 새로 작성 시작
                            if (nDelimiterIdx >= 0)
                            {
                                String strNewPath = strPathSubString[0] + "_" + "File" + nFileCount + "." + strPathSubString[1];

                                //Delete File if exists
                                FileInfo fileDel = new FileInfo(strNewPath);
                                if (fileDel.Exists)
                                {
                                    fileDel.Delete();
                                }

                                File.Move(strTempPath, strNewPath);

                                nFileCount++;

                                strTempPath = strPathSubString[0] + "_temp" + nFileCount + "." + strPathSubString[1];

                                //현재 사용하던 stream wrtier를 닫고 새로 오픈
                                sw.Close();

                                sw = new StreamWriter(strTempPath, true, System.Text.Encoding.GetEncoding("utf-8"));
                            }

                            sw.WriteLine(strLine);
                        }

                    }

                    MessageBox.Show("Finish Converting");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        public bool CheckBasicNeccesaryStringData(string strLine)
        {
            //사용하지 않는 구분자가 있는지 확인
            bool bRet = strLine.Contains("<IGA>");

            return !(bRet);
        }

        public int CheckFileDelimeter(string strLine)
        {
            int nRet = -1;

            BaseData cBaseData = new BaseData();

            int nCnt = cBaseData.GetFileDelimiterListSize();

            for (int i = 0; i < nCnt; i++)
            {
                string strDelimiter = cBaseData.getFileDelimiterListComponent(i);

                if (strLine.Contains(strDelimiter) == true)
                {
                    nRet = i;
                    break;
                }
            }

            return nRet;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
