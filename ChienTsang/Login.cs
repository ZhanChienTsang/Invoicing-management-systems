using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ChienTsang
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        int iTimes;
        string cnstr, sql;

        private void button1_Click(object sender, EventArgs e)
        {
            string dbfm = "YEMPLOYEE";
            sql = string.Format("Select Eno,Ename  from " + dbfm + " WHERE Eno = '{0}' " +
                                             "AND Psword = '{1}' ",
                                             textBox1.Text, textBox2.Text);

            //連資料庫取Employee
            SqlConnection cn = new SqlConnection(cnstr);
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(sql, cn);
            cn.Open();                                  //必須
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);                                //載入SqlDataReader的資料
            dr.Close();                                 //關閉SqlDataReader
            cmd.Dispose();                              //釋放SqlCommand所占用的資源
            cn.Close();

            if (dt.Rows.Count > 0)
            {
                try
                {
                    this.DialogResult = DialogResult.OK;
                    //下方為指派給靜態成員，才能使用此方式
                    Main.FCheck = true;
                    Main.FUserID = (string)dt.Rows[0]["Eno"];
                    Main.FUserName = (string)dt.Rows[0]["Ename"];
                    Main.cnstr = cnstr;
                    //CanClose = true;  主畫面關閉確認其他(非主Form)是否仍有程式未關閉(本案例不會有此狀況, 免用)
                }
                catch (System.Exception er)
                {
                    MessageBox.Show(er.ToString());
                }
            }
            else
            {
                MessageBox.Show("使用者代碼或密碼錯誤。");
                iTimes = iTimes + 1;
                if (iTimes == 3)
                {
                    MessageBox.Show("認證失敗已達3次，系統將結束！");
                    Close(); //關檔
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            string srvname, dbname;
            srvname = ".\\SQL2019";               //ServerName
            dbname = "YVMENUC";          //DataBase   //YVMENUC

            cnstr = "Data Source=" + srvname + ";Initial Catalog=" + dbname +
                "; Integrated Security = true; ";

            //認證次數
            iTimes = 0;
            Main.FCheck = false;
        }
    }
}
