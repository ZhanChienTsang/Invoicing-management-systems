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
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Drawing.Printing;
using Microsoft.VisualBasic;

namespace ChienTsang
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }
        #region 變數
        public static bool FCheck = false;          //是否已經通過認證
        public static string FUserID, FUserName;    //登入的使用者代碼與使用者名稱
        public static string cnstr;                 //資料庫連接字串

        #endregion
        private void Form4_Load(object sender, EventArgs e)
        {
            //登入
            Login Login = new Login();
            Login.ShowDialog();

            if (!FCheck)        //如果未通過認證，就結束系統
            {
                Close();
            }

            //主頁面配置
            toolStripStatusLabel1.Text = "系統時間：" + DateTime.Now.ToString("yyyy/MM/dd  hh:mm:ss");
            this.timer1.Interval = 1000;
            timer1.Enabled = true;
        }

        private void 商品檔維護ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // Form7.LbTable = "YFGMAST";
            Product prod = new Product();
            prod.Show();
        }

        private void 客戶表維護ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Customer cust = new Customer();
            cust.Show();
        }

        

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "系統時間：" + DateTime.Now.ToString("yyyy/MM/dd  hh:mm:ss");
        }

    

        private void 庫存表維護ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Inventory inv = new Inventory();
            inv.Show();
        }

        private void 進銷檔維護ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PurOrd.LbTable = "YFGIO";
            PurOrd fdouble2 = new PurOrd();
            fdouble2.Show();
        }

        private void 訂單檔維護ToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            PurOrd.LbTable = "YODR";
            PurOrd fdouble1 = new PurOrd();
            fdouble1.Show();
        }

        private void 購物車ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShopCart shopcart = new ShopCart();
            shopcart.Show();
        }
    }
}
