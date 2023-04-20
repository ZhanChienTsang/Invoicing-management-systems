using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChienTsang
{
    public partial class test_use : Form
    {
        public test_use()
        {
            InitializeComponent();
        }
        #region 變數
        string ss, sql, tbname, cnstr, ae = "";      //sql string
        int[] ftype;
        int fno, po;
        SqlConnection cn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter adapt;
        DataTable dt;
        DataSet ds;
        BindingSource bds;
        TextBox[] tBox;
        Label[] Lb;
        DgvSet dgvset;
        Color[] color;
        List<string> DescList = new List<string>();     //中文欄名
        List<ToolStripButton> Lbtn = new List<ToolStripButton>();
        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            tbname = comboBox1.Text;
            sql = "SELECT * FROM " + tbname;
            btnClose_Click(sender, e);    //做釋放!!!!在開主檔才不會出問題
            GetColumnsDescrip(tbname);  //取得中文欄位名稱
            OpenTable();  //開檔
            dgv_Align(dataGridView1, dt, ftype);  //格式

            //fno = dt.Columns.Count;
            //ftype = CreateFtypeArray(dt);           //各欄位資料型別陣列
            //UiDataCreate(fno);
            //GetColumnsDescrip(tbname);
            //dgv_Align(dataGridView1, dt, ftype);
            //Set the DataMember to the Menu table.

            Lbtn.Clear();
            Lbtn = new List<ToolStripButton>() { toolStripButton1, toolStripButton2, toolStripButton3, toolStripButton4 };      //取消、寫入免納入，因程序內控制ae="A"、"E"
            //SetMode(Lbtn, true);
        }
       

        #region 自訂方法
        private void OpenTable()                    //開檔
        {
            //tbname = comboBox1.Text;
            //sql = "SELECT * FROM " + tbname;
            //cn = new SqlConnection(cnstr);
            //SqlDataAdapter adapt = new SqlDataAdapter(sql, cn);
            //DataSet ds = new DataSet();

            adapt = new SqlDataAdapter(sql, cn);
            ds = new DataSet();
            adapt.Fill(ds, tbname);
            dt = ds.Tables[0];
            //adapt.Update(dt);
            dataGridView1.DataSource = dt;

           
            fno = dt.Columns.Count;
            ftype = CreateFtypeArray(dt);           //各欄位資料型別陣列
            UiDataCreate(fno);                      //資料維護之介面建立

            bds = new BindingSource();
            bds.DataSource = ds;
            bds.DataMember = tbname;                //Set the DataMember to the Menu table.


            BindingData();          //設定tBox[] Binding、dataGridView1控制項顯示BindingSource的資料來源(資料繫結BindingSource)
            ////動態事件建立
            //bds.PositionChanged += new EventHandler(bds_PositionChanged);
            //bds.MoveLast();
        }
        public int[] CreateFtypeArray(DataTable tb)             //參數為 Table, 回傳 int 陣列 ftype[]             
        {
            //取單檔資料型態當 ftype[]
            string fieldname;   //欄位型態名稱字串
            int tp;
            ftype = new int[tb.Columns.Count];
            for (int i = 0; i <= tb.Columns.Count - 1; i++)     //取資料型態當 ftyp[]
            {
                fieldname = tb.Columns[i].DataType.ToString();

                if (fieldname.IndexOf("String") != -1)           //表找到 "String"
                    tp = 1;      //string
                else
                if (fieldname.IndexOf("Boolean") != -1)          //表找到 "Bloolean"
                    tp = 2;      //Bloolean
                else
                if (fieldname.IndexOf("DateTime") != -1)         //表找到 "DateTime"
                    tp = 3;      //DateTime
                else
                if ((fieldname.IndexOf("Int") != -1) || (fieldname.IndexOf("Byte") != -1) ||
                    (fieldname.IndexOf("Short") != -1) || (fieldname.IndexOf("Long") != -1))
                    tp = 4;        //整數
                else                                            //如有其他型態會有問題，屆時再增加判斷條件
                    tp = 5;        //小數
                ftype[i] = tp;

                //ftype[i] = fieldtype(fieldname);     // 1:String,  2:Boolean, 3:DateTime, 4:整數, 5:小數
                //MessageBox.Show("ftype["+i.ToString()+"]="+ ftype[i].ToString()+"; "+ fieldname);
            }
            return ftype;
        }

        private void UiDataCreate(int n)     //動態tbox
        {
            int x, y, j;
            tBox = new TextBox[n];
            Lb = new Label[n];
            Font font = new Font("Arial", 12);
            //GetStrLength();       //取 Schema(tb1) 的欄位長度  flenB[]及主索引的欄位序
            x = 0;
            for (j = 0; j < n; j++)
            {
                if (j % 2 == 1)
                {
                    y = 1;
                    x = x - 1;
                }
                else
                    y = 0;
                tBox[j] = new TextBox();
                tBox[j].Name = string.Format("tBox{0}", j);             //tBox0、tBox1...
                tBox[j].Left = 100 + 400 * y;                           //500 * y;
                tBox[j].Top = 80 + 40 * x;                              //60+40*x
                tBox[j].Height = 30;
                tBox[j].Font = font;

                if (ftype[j] == 4 || ftype[j] == 5)                     // 1:String,  2:Boolean, 3:DateTime, 4:整數, 5:小數
                    tBox[j].TextAlign = HorizontalAlignment.Right;
                else
                    tBox[j].TextAlign = HorizontalAlignment.Left;
                switch (ftype[j])
                {
                    case 1:
                        tBox[j].Width = 200;                            //Math.Min((flenB[j] * 14), 310);      //只取寬，無空白。　　
                        break;
                    case 2:
                        tBox[j].Width = 12 * 5;
                        break;
                    case 3:
                        {
                            tBox[j].Width = 12 * 10;
                            break;
                        }
                    case 4:
                        tBox[j].Width = 12 * 8;                         //數值型態
                        break;
                    case 5:
                        tBox[j].Width = 12 * 8;                         //數值型態
                        break;
                }

                tBox[j].BackColor = Color.FromArgb(255, 255, 212);
                tBox[j].ForeColor = Color.FromArgb(0, 0, 153);
                this.Controls.Add(tBox[j]);

                Lb[j] = new Label();
                if (DescList.Count == 0)
                    Lb[j].Text = dt.Columns[j].ColumnName;
                else
                    Lb[j].Text = DescList[j];
                Lb[j].Top = 83 + 40 * x;
                Lb[j].Left = 10 + 400 * y;
                Lb[j].Width = 80;
                Lb[j].Height = 25;
                Lb[j].Font = font;
                Lb[j].ForeColor = Color.FromArgb(0, 0, 125);
                this.Controls.Add(Lb[j]);
                x++;
            }
            //畫面隨資料表欄位多少自動調整畫面的高
            int h = 10 + (int)(fno * 1.0 / 2 + 0.5) * 40 + 20 + 15;   //h為L+tBox空間高
           // label3.Top = 30 + h;
            dataGridView1.Top = 30 + h + 30;
            this.Height = 80 + h + 15 + 20 + dataGridView1.Height;
        }

        public void GetColumnsDescrip(string tbn)  //取得資料庫中建立的中文名稱
        {
            //string desc = "SELECT value FROM::fn_listextendedproperty(NULL, 'user', 'dbo', 'table', '" + tbn + "','column','" + cln + "')";
            string desc = "SELECT  VALUE FROM fn_listextendedproperty(NULL, 'schema', 'dbo', 'table', '" + tbn + "', 'column', DEFAULT)";
            cn.Open();
            SqlCommand cmd = new SqlCommand(desc, cn);
            SqlDataReader dr;
            DataTable tb = new DataTable();
            dr = cmd.ExecuteReader();
            tb.Load(dr);
            cn.Close();
            DescList.Clear();
            for (int i = 0; i < tb.Rows.Count; i++)
                DescList.Add(tb.Rows[i][0].ToString());
            tb.Dispose();
            cmd.Dispose();
        }

        public void dgv_Align(DataGridView dgv, DataTable tbname, int[] ftyp)     //文字靠左、數字靠右
        {
            //MessageBox.Show("tb=" + tbname + "; count=" + tbname.Columns.Count.ToString());
            //文字Alignment (數字型欄位資料靠右)
            for (int i = 0; i <= tbname.Columns.Count - 1; i++)
            {
                if (ftyp[i] == 4 || ftyp[i] == 5)
                {
                    dgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; //數字型文字靠右。 //MiddleLeft
                }
                if (ftyp[i] == 3)
                {
                    dgv.Columns[i].DefaultCellStyle.Format = "yyyy/MM/dd";          //日期欄位格式
                }
                if (DescList.Count > 0)                                             //2021.06.12增
                    dgv.Columns[i].HeaderText = DescList[i];                        //**中文欄名
            }
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;     //欄寬自動調整
        }

        private void BindingData()      //控制項的資料繫結
        {
            //設定TextBox控制項的資料繫結
            for (int i = 0; i <= dt.Columns.Count - 1; i++)
            {
                tBox[i].DataBindings.Add("Text", bds, dt.Columns[i].ColumnName);
            }
            //bindingNavigator1.BindingSource = bds;
            dataGridView1.DataSource = bds;
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            FreetBox_LbArray();                                 //陣列釋放

            if (bds != null)
            {
                bds = null;
                ds = null;
                dataGridView1.DataSource = null;
            }
            //Lbtn.Clear();
            //Lbtn = new List<ToolStripButton>() { btnAdd, btnDel, btnEdit, btnUpdate, btnCancel, btnSeek, btnSeekDo, btnRefresh };
            //上式可替代下二式 
            //Button[] btn = { btnAdd, btnDel, btnEdit, btnSeek };          
            //Lbtn.AddRange(btn);
            //SetMode(Lbtn, false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ae = "A";
            DataInit();     //斷開 bds，清除tBox[]

            Lbtn.Clear();
            Lbtn = new List<ToolStripButton>();//{ btnAdd, btnDel, btnEdit, btnSeek };      //
            SetMode(Lbtn, false);

            Lbtn.Clear();
            Lbtn = new List<ToolStripButton>();// { btnUpdate, btnCancel };      //
            SetMode(Lbtn, true);
            ReadOnlyFalse();
        }

        private void FreetBox_LbArray()                                 //陣列釋放
        {
            //tBox[]、Lb[] 如果已存在,作釋放
            if (Lb != null)                                             //如果陣列存在
            {
                for (int j = 0; j < tBox.Length; j++)                   //先釋放陣列 tBox.Length  較 tBox.Count() 快
                {
                    if (tBox[j] != null)
                    {
                        tBox[j].Dispose();
                        //panel1.Controls.Remove(tBox[j]);              //本式與下一式功能同上
                        //panel1.Controls.RemoveByKey(tBox[j].Name);    //有物件名稱，則以 RemoveByKey(object[i,j].Name) 移除
                        Lb[j].Dispose();
                    }
                }
                //MessageBox.Show("釋放完畢!"+ Convert.ToString(Lb.Count()));
            }
        }

        #endregion

        private void Form2_Load(object sender, EventArgs e)
        {
            //cn.ConnectionString = "Data Source=.\\SQL2019;Initial Catalog=YVMENUC;Integrated Security=True";
            ////cn.ConnectionString = "Data Source=.;Initial Catalog=mydb;Persist Security Info=True;User ID=sa;pwd=1qaz@wsx";
            //cmd.Connection = cn;
            //ss = "SELECT * FROM YCUST";    //指示ShowData()的內容

            string srvname, dbname;     //string usrid="sa", spwd="";
            srvname = ".\\SQL2019";              //ServerName
            dbname = "YVMENUC";         //DataBase
            cnstr = "Data Source=" + srvname + "; Database = " + dbname +"; Trusted_Connection = True";       //使用信任連線的方式
            /// ";User ID=" + usrid + ";PWD=" + spwd + ";";  
            /// // cnstr = "Data Source=.\\sql2019;Initial Catalog=YVMENUC;Integrated Security=True";
             cn = new SqlConnection(cnstr);

            comboBox1.SelectedIndex = 0;
            //ShowData();
            dgvset = new DgvSet();
            dgvset.dgvSet(dataGridView1);
        }

        #region 新增,刪除....
        public void DataInit()                                  //查詢、新增時，提供空白UI。
        {
            for (int j = 0; j <= fno - 1; j++)
            {
                tBox[j].DataBindings.Clear();                   //斷開 bds
                tBox[j].Text = "";                              //清除原tBox[]內容

                if (ftype[j] == 4 || ftype[j] == 5)                     // 1:String,  2:Boolean, 3:DateTime, 4:整數, 5:小數
                    tBox[j].TextAlign = HorizontalAlignment.Right;
                else
                    tBox[j].TextAlign = HorizontalAlignment.Left;
                switch (ftype[j])
                {
                    case 1:
                        tBox[j].Width = 200;            //Math.Min((flenB[j] * 14), 310);      //只取寬，無空白。
                        tBox[j].Text = "";
                        break;
                    case 2:
                        tBox[j].Width = 12 * 5;
                        tBox[j].Text = "False ";        //Boolean
                        break;
                    case 3:
                        tBox[j].Width = 12 * 10;
                        tBox[j].Text = "";              //DateTime.Now.ToString("yyyy/MM/dd");
                        break;
                    case 4:
                        tBox[j].Width = 12 * 8;                         //數值型態
                        tBox[j].Text = "0";             //整數
                        break;
                    case 5:
                        tBox[j].Width = 12 * 8;                         //數值型態
                        tBox[j].Text = "0.0";             //整數
                        break;
                }
            }
        }
        private void SetMode(List<ToolStripButton> mybtn, bool tf)
        {
            for (int i = 0; i <= mybtn.Count() - 1; i++)
            {
                mybtn[i].Enabled = Convert.ToBoolean(tf);
            }
        }

        private void ReadOnlyFalse()                 //設定tBox[]、dataGridView1、dataGridView2為 ReadOnly=true 
        {
            //設定TextBox解除 ReadOnly
            for (int i = 0; i <= dt.Columns.Count - 1; i++)
                tBox[i].ReadOnly = false;

            dataGridView1.ReadOnly = false;          //需要時另外解除 ReadOnly
        }

        #endregion
    }
}
