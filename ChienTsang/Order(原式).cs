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
    public partial class Order : Form
    {
        public Order()
        {
            InitializeComponent();
        }
        #region 變數
        string cnstr, fsort;    //fsort:關聯欄位
        string dbfmm, dbfmd;    //dbf為Table名稱
        string sqlmm, sqlmd;    //sql 為開啟 Table的語法
        string ss, str;
        SqlConnection cn;
        SqlDataAdapter damm, damd;
        DataSet ds;
        SqlCommandBuilder budmm, budmd;      //更新用
        DataTable tbmm, tbmd;
        BindingSource bdsmm, bdsmd;
        DataRelation relation1;
        DataColumn tb1Column, tb2Column;
        #endregion

        #region Button CRUD
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //單據編號
            bdsmm.Filter = null;
            ss = toolStripTextBox1.Text.Trim();
            str = tb1Column + " Like '%" + ss + "%'";           //VHNO
            bdsmm.Filter = str;
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //公司編號
            ss = toolStripTextBox1.Text.Trim();
            str = "CONO" + " Like '%" + ss + "%'";              //**CONO
            bdsmm.Filter = str;
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //重新整理
            bdsmm.RemoveFilter();
        }
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            //新增寫入
            try
            {
                damm.Update(tbmm);
                damd.Update(tbmd);
                MessageBox.Show("寫入更新記錄成功!");
            }
            catch (System.Exception er)
            {
                ds.RejectChanges();                             //資料集所有資料表記錄都放棄更新
                MessageBox.Show("寫入更新記錄失敗!" + er.ToString());
            }
        }
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            //確定離開？
            if (MessageBox.Show("確定返回主頁面？", "Confirm Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                this.Dispose();
            }
        }

        #endregion

        #region 自訂方法
        private void CreateRelation()   //取 Table 主檔、明細檔並作關聯
        {
            sqlmm = "SELECT * FROM " + dbfmm + " Order By " + fsort;
            sqlmd = "SELECT * FROM " + dbfmd;

            ds = new DataSet();
            //置入Dataset ds 並取主檔 tbmm
            damm = new SqlDataAdapter(sqlmm, cn);
            budmm = new SqlCommandBuilder(damm);    //直接用 Update 而非使用 UpdateCommand 時用
            damm.Fill(ds, dbfmm);
            tbmm = ds.Tables[0];

            //置入Dataset ds 並取 Table 明細檔 tbmd
            damd = new SqlDataAdapter(sqlmd, cn);
            budmd = new SqlCommandBuilder(damd);    //直接用 Update 而非使用 UpdateCommand 時用
            damd.Fill(ds, dbfmd);
            tbmd = ds.Tables[1];

            //creating data relations 
            relation1 = null;

            //retrieve column (Relation)
            tb1Column = ds.Tables[0].Columns[fsort];        // 進銷主檔   VHNO
            tb2Column = ds.Tables[1].Columns[fsort];        // 進銷明細檔 VHNO

            //relation1's relating tables 關聯的方法
            relation1 = new DataRelation("relation1", tb1Column, tb2Column);
            ds.Relations.Add(relation1);        //assign relation to dataset 
        }

        private void BindingData()      //控制項的資料繫結
        {
            //建立BindingSource物件
            bdsmm = new BindingSource(ds, dbfmm);             //(DataSet,    主檔名)
            bdsmd = new BindingSource(bdsmm, "relation1");    //(主檔bds , 關聯名稱)

            //設定DataGridView控制項顯示BindingSource的資料來源
            dataGridView1.DataSource = bdsmm;
            dataGridView2.DataSource = bdsmd;

            //設定bindingNavigator1控制項指標的BindingSource之資料來源
            bindingNavigator1.BindingSource = bdsmm;
        }

        void ShowData()
        {
            dbfmm = "YODR"; fsort = "ORDER1";
            dbfmd = "YODRDT";
            //取 Table 主檔、明細檔並作建立Relation關聯
            CreateRelation();
            BindingData();

            DgvSet dgvset = new DgvSet();
            dgvset.dgvSet(dataGridView1);
            dgvset.dgvSet(dataGridView2);
        }
        #endregion
        private void Order_Load(object sender, EventArgs e)
        {
            string srvname, dbname;
            srvname = ".\\SQL2019";               //ServerName
            dbname = "YVMENUC";          //DataBase

            cnstr = "Data Source=" + srvname + ";Initial Catalog=" + dbname +
                "; Integrated Security = true; ";
            cn = new SqlConnection();
            cn.ConnectionString = cnstr;

            ShowData();

            //DGV1、2中文欄位
            dataGridView1.Columns[0].HeaderText = "訂單編號";     //改中文欄位名稱
            dataGridView1.Columns[1].HeaderText = "訂單日期";
            dataGridView1.Columns[2].HeaderText = "公司編號";
            dataGridView1.Columns[3].HeaderText = "訂單金額";

            dataGridView2.Columns[0].HeaderText = "訂單編號";     //改中文欄位名稱
            dataGridView2.Columns[1].HeaderText = "成品代號";
            dataGridView2.Columns[2].HeaderText = "成品數量";
            dataGridView2.Columns[3].HeaderText = "成品單價";
            dataGridView2.Columns[4].HeaderText = "備註";

            //dataGridView1.AutoResizeColumns();
            //dataGridView2.AutoResizeColumns();

        }
    }
}
