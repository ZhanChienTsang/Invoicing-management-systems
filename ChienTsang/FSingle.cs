using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Transactions;              //需加入參考 System.Transactions.dll
using System.Globalization;             //日期轉換
using System.IO;


namespace ChienTsang
{
    public partial class FSingle : Form
    {
        SqlConnection con;
        SqlDataAdapter adapt;
        SqlCommandBuilder build;
        DataSet ds;
        BindingSource bds;
        DataTable dt;
        string sql, tbname, cnstr, ae = "";     //tbname:Table Name；cnstr:資料庫連接字串；se:系統狀況 "A","E","D","S"
        TextBox[] tBox;
        Label[] Lb;
        int[] ftype;
        int fno, po;
        List<ToolStripButton> Lbtn = new List<ToolStripButton>();
        List<string> DescList = new List<string>();     //中文欄名
        public static string LbTable;
        DgvSet dgv; GetData getdata; string fsort;
        int[] flenB; int idx; //分別為getdata.DataInit()、GetStrLength()參數之1
        string path, pictname; 
        Bitmap pict;
        DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory);   //using System.IO;

        public FSingle()
        {
            InitializeComponent();
        }


        #region 開\關檔相關
        private void btnOpen_Click(object sender, EventArgs e)
        {   //使用的Table，必須要有主索引，本專案為簡單版，程式不提醒!
            tbname = LbTable;     //tbname = comboBox1.Text;
            fsort = getdata.GetPrimekey(tbname);
            if (fsort == "沒有資料")
            {
                MessageBox.Show("請先將 " + tbname + " 建立主索引！");
                this.Dispose();
            }

            sql = "SELECT * FROM " + tbname;
            btnClose_Click(sender, e);                  //包含陣列釋放 FreetBox_LbArray(); 
            DescList = getdata.GetColumnsDescrip(tbname);   //取得中文欄名
            //GetColumnsDescrip(tbname);                  //取得中文欄名

            OpenTable();
            dgv.dgvSet(dataGridView1);
            dgv.dgv_Align(dataGridView1, dt, ftype);
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                if (DescList.Count > 0)                                     
                    dataGridView1.Columns[i].HeaderText = DescList[i];      //**單檔中文欄名
            }
            ReadOnlyTrue();
            ae = "";

            Lbtn.Clear();
            Lbtn = new List<ToolStripButton>() { btnAdd, btnDel, btnEdit, btnSeek };      //取消、寫入免納入，因程序內控制ae="A"、"E"
            getdata.SetMode(Lbtn, true);
        }

        private void OpenTable()                    //開檔
        {
            adapt = new SqlDataAdapter(sql, con);
            ds = new DataSet();
            //置入Dataset ds 並取主檔 tbm
            build = new SqlCommandBuilder(adapt);    //直接用 Update 而非使用 UpdateCommand 時用
            adapt.Fill(ds, tbname);
            dt = ds.Tables[0];

            fno = dt.Columns.Count;
            ftype = getdata.CreateFtypeArray(dt);   //各欄位資料型別陣列
            //ftype = CreateFtypeArray(dt);           //各欄位資料型別陣列
            UiDataCreate(fno);                      //資料維護之介面建立

            bds = new BindingSource();
            bds.DataSource = ds;
            bds.DataMember = tbname;                //Set the DataMember to the Menu table.


            BindingData();          //設定tBox[] Binding、dataGridView1控制項顯示BindingSource的資料來源(資料繫結BindingSource)
            //動態事件建立
            bds.PositionChanged += new EventHandler(bds_PositionChanged);
            bds.MoveLast();                         //末筆，因開啟是首筆，到末筆才會觸發 bds_PositionChanged
        }

        private void UiDataCreate(int n)
        {
            int x, y, j;
            tBox = new TextBox[n];
            Lb = new Label[n];
            Font font = new Font("Arial", 12);
            flenB = getdata.GetStrLength(fno, fsort, tbname, ref idx);
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
            label3.Top = 30 + h;
            dataGridView1.Top = 30 + h + 30;
            this.Height = 80 + h + 15 + 20 + dataGridView1.Height;

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
            Lbtn.Clear();
            Lbtn = new List<ToolStripButton>() { btnAdd, btnDel, btnEdit, btnUpdate, btnCancel, btnSeek, btnSeekDo, btnRefresh };
            //上式可替代下二式 
            //Button[] btn = { btnAdd, btnDel, btnEdit, btnSeek };          
            //Lbtn.AddRange(btn);
            getdata.SetMode(Lbtn, false);
        }

        #endregion

        #region 動態事件
        private void bds_PositionChanged(Object sender, EventArgs e)                //動態事件
        {
            po = bds.Position;                                                      //當下指標位置
            if (po >= 0)              //查數字型態無資料實,bds會觸發本事件，因此加本判斷，避免dt.Rows[po][j]之bug
            {
                for (int j = 0; j < dt.Columns.Count - 1; j++)
                {
                    if (dt.Columns[j].DataType.ToString().IndexOf("DateTime") != -1)     //日期欄位顯示控制
                    {
                        if (dt.Rows[po][j].ToString().Trim() == "")
                            tBox[j].Text = "";
                        else
                            tBox[j].Text = DateTime.Parse((dt.Rows[po][j].ToString())).ToString("yyyy/MM/dd");  //只顯示日期格式，不要時,分,秒
                    }
                }
            }
            btnTop.Enabled = (bds.Position > 0);
            btnPrior.Enabled = (bds.Position > 0);
            btnNext.Enabled = (bds.Position < bds.Count - 1);
            btnLast.Enabled = (bds.Position < bds.Count - 1);
            label3.Text = "記錄數：" + (bds.Position + 1).ToString() + "/" + (bds.Count).ToString();  //
            //取照片                                                                                      //取照片
            if (tbname == "YEMPLOYEE" || tbname == "YFGMAST")
            {
                pictname = tBox[8].Text;    //**配合dgv排序，由下式改本式，但需配合不同Table調整照片的欄位序
                string path1 = "";
                if (pictname != "")
                {
                    string pdir = tbname == "YEMPLOYEE" ? "\\EmPicture\\" : "\\ItPicture\\";
                    path1 = path + pdir + pictname;
                    try
                    {
                        pict = new Bitmap(path1);
                        this.pictureBox1.Image = pict;
                    }
                    catch { };
                }
                else
                    pictureBox1.Image = null;               //&&
            }

        }
        #endregion

        #region 指標異動
        private void btnTop_Click(object sender, EventArgs e)
        {
            bds.MoveFirst();                    //首筆
        }

        private void btnPrior_Click(object sender, EventArgs e)
        {
            bds.MovePrevious();                 //上筆
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            bds.MoveNext();                     //下筆
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            bds.MoveLast();                     //末筆
        }
        #endregion

        #region 資料維護
        //新增
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ae = "A";
            getdata.DataInit(fno, tBox, ftype, flenB); //DataInit();     //斷開 bds，清除tBox[]

            Lbtn.Clear();
            Lbtn = new List<ToolStripButton>() { btnAdd, btnDel, btnEdit, btnSeek };      //
            getdata.SetMode(Lbtn, false);

            Lbtn.Clear();
            Lbtn = new List<ToolStripButton>() { btnUpdate, btnCancel };      //
            getdata.SetMode(Lbtn, true);
            ReadOnlyFalse();
        }

        //編修
        private void btnEdit_Click(object sender, EventArgs e)
        {
            Lbtn.Clear();
            Lbtn = new List<ToolStripButton>() { btnAdd, btnDel, btnEdit, btnSeek };      //
            getdata.SetMode(Lbtn, false);

            Lbtn.Clear();
            Lbtn = new List<ToolStripButton>() { btnUpdate, btnCancel };
            getdata.SetMode(Lbtn, true);

            ae = "E";
            po = bds.Position;
            ReadOnlyFalse();
            tBox[0].ReadOnly = true;    //主索引用編號欄不能作異動
            for (int i = 0; i <= dt.Columns.Count - 1; i++)
                tBox[i].DataBindings.Clear();
        }

        //刪除
        private void btnDel_Click(object sender, EventArgs e)
        {
            Lbtn.Clear();
            Lbtn = new List<ToolStripButton>() { btnAdd, btnDel, btnEdit, btnSeek };
            getdata.SetMode(Lbtn, false);

            bool deltf = false;
            if (MessageBox.Show("確定刪除本筆資料？", "Confirm Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {   //確定刪除？
                ae = "D";
                deltf = TransactionDML(ae);     //deltf = DeleteDb();                 
                if (deltf == true)
                    btnOpen_Click(sender, e);   //重新開檔；  不需要作Lbtn 控制，因Open_Click會執行Close_Click()、OpenTable;
                else
                    btnResume();     //恢復btnAdd, btnDel, btnEdit, btnSeek=Enabled；btnUpdate, btnCancel=Disabled
            }
            else
                btnResume();     //恢復btnAdd, btnDel, btnEdit, btnSeek=Enabled；btnUpdate, btnCancel=Disabled
        }

        //取消
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if ((ae == "A") || (ae == "E"))
            {
                if (ae == "A")
                {
                    MessageBox.Show("取消新增！");
                    BindingData();                          //tBox[i]、dataGridView1 作 Binding
                    bds_PositionChanged(sender, e);         //新增曾斷開BindingData(); 後需再bds_PositionChanged(sender, e);  
                }    //新增時曾斷開 bds

                if (ae == "E")
                {
                    MessageBox.Show("取消編修！");
                    for (int i = 0; i <= dt.Columns.Count - 1; i++)     //改於編修時斷開tBox，取消時需要重新Binding
                        tBox[i].DataBindings.Add("Text", bds, dt.Columns[i].ColumnName);
                }
                //dt.Rows[po].RejectChanges();              //資料表當筆記錄都放棄更新
                dt.RejectChanges();                         //資料表所有記錄都放棄更新 
            }
            ae = "";
            btnResume();     //恢復btnAdd, btnDel, btnEdit, btnSeek=Enabled；btnUpdate, btnCancel=Disabled (含ReadOnlyTrue();)
        }

        //寫入
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            bool addtf = false, editf = false;
            int po1;
            if ((ae == "A") || (ae == "E"))
            {
                if (ae == "A")
                {
                    addtf = TransactionDML(ae);             //addtf = AddDb();
                    po1 = bds.Count - 1;                    //dt.Rows.Count - 1;  //dataGridView1.RowCount - 1; //末筆
                }
                else        //if (ae == "E")
                {
                    po1 = bds.Position;
                    if (MessageBox.Show("確定寫入本筆資料？", "Confirm Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                        editf = TransactionDML(ae);         //editf = UpdateDb();    //確定寫入
                    else
                    { editf = false; }
                }
                if (addtf == false && editf == false)       //當true時,會執行 ReShowData(),即包含這些動作.
                {
                    //tb.Rows[po].RejectChanges();          //資料表當筆記錄都放棄更新
                    dt.RejectChanges();                     //資料表所有記錄都放棄更新 (本專案採即動即修(寫入)，因此不使用上式
                    btnResume();                            //恢復btnAdd, btnDel, btnEdit, btnSeek=Enabled；btnUpdate, btnCancel=Disabled
                    bds_PositionChanged(sender, e);
                }
                else if (addtf == true || editf == true)
                {
                    btnOpen_Click(sender, e);   //重新開檔  (含ReadOnlyTrue();) //不需要作Lbtn 控制，因Open_Click會執行Close_Click()、OpenTable;
                    bds.Position = po1;         //dataGridView1.CurrentCell = dataGridView1[0, po1];     //指標移到特定筆
                }
            }
        }

        private bool TransactionDML(string ae1)
        {
            bool tf = false;
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    if (ae1 == "A")
                        tf = AddDb();                            //包含寫入資料庫
                    else if (ae1 == "E")
                        tf = UpdateDb();                     //確定寫入(包含寫入資料庫)
                    else if (ae1 == "D")
                        tf = DeleteDb();  //包含寫入資料庫
                    ts.Complete();
                }
                catch
                { };     //發生例外時，會自動 Rollback
            }
            return tf;
        }
        /*
        private void GetDataRow(ref DataRow dr)
        {
            for (int j = 0; j < fno; j++)
            {
                //寫入DataRow dr[]
                if (ftype[j] == 1 || ftype[j] == 3)             //1:String,  2:Boolean, 3:DateTime, 4:整數, 5:小數
                {
                    if (ftype[j] == 3 && tBox[j].Text == "")
                        dr[j] = "2000/1/1";
                    else
                        dr[j] = tBox[j].Text;
                }
                else if (ftype[j] == 2)
                {
                    if (tBox[j].Text.Substring(0, 1) == "F")    //bool 值
                        dr[j] = 0;
                    else
                        dr[j] = 1;
                }
                else if (ftype[j] == 4)                         //整數
                {
                    tBox[j].Text = tBox[j].Text == "" ? "0" : tBox[j].Text;  //**如果tBox[j].Text需要寫入null時，應 dr[j] = DBNull.Value;不能dr[j] = null
                    dr[j] = Convert.ToInt32(tBox[j].Text);
                }
                else if (ftype[j] == 5)                         //小數
                {
                    tBox[j].Text = tBox[j].Text == "" ? "0" : tBox[j].Text;
                    dr[j] = Math.Round(Convert.ToSingle(tBox[j].Text), 4, MidpointRounding.AwayFromZero);
                }
            }
        }
        */
        private bool AddDb()                                    //新增(寫入)DataBase  
        {
            bool addtf;
            DataRow dr = ds.Tables[0].NewRow();                 //建立DataRow物件為Table[0]的 NewRow
            getdata.GetDataRow(ref dr, tBox, ftype);    //GetDataRow(ref dr);                                 //異動資料(tBox[]寫入到NewRow - 新資料)
            ds.Tables[0].Rows.Add(dr);
            //寫入資料庫
            try
            {
                adapt.Update(dt);          //不能寫 da.Update(ds)，可寫da.Update(ds.Tables[dbf]);
                MessageBox.Show("新增寫入記錄成功!");
                addtf = true;
            }
            catch (System.Exception e)
            {
                ds.RejectChanges();
                MessageBox.Show("新增寫入記錄失敗!" + e.ToString());
                addtf = false;
            }
            return addtf;
        }


        private bool UpdateDb()                                 //更新(寫入)DataBase  
        {
            //將 tBox[] To dataGridView1 To DataRow dr[]
            int crow = dataGridView1.CurrentRow.Index;          //可使用 po 因 crow=po
            bool editf;
            DataRow dr = ds.Tables[0].Rows[crow];
            dr.BeginEdit();
            try
            {
                getdata.GetDataRow(ref dr, tBox, ftype);    //GetDataRow(ref dr);   
                //異動資料(tBox[]寫入到dr(原資料)，也可不使用本式，但最好在DataGridView作編修
                editf = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                editf = false;
            }
            dr.EndEdit();
            //寫入資料庫
            try
            {
                adapt.Update(dt);                               //不能寫 da.Update(ds)，可寫da.Update(ds.Tables[dbf]);
                MessageBox.Show("寫入更新記錄成功!");
                editf = true;
            }
            catch (System.Exception e)
            {
                ds.RejectChanges();                             //資料集所有資料表記錄都放棄更新
                MessageBox.Show("寫入更新記錄失敗!" + e.ToString());
                editf = false;
            }
            return editf;
        }


        private bool DeleteDb()                                 //實際刪除
        {
            bool deltf = false;
            bds.RemoveCurrent();       //使用BindingSource的RemoveCurrent

            //寫入資料庫
            try
            {
                adapt.Update(dt);                               //不能寫 da.Update(ds)，可寫da.Update(ds.Tables[dbf]);
                MessageBox.Show("寫入刪除記錄成功!");
                deltf = true;
            }
            catch (System.Exception e)
            {
                ds.RejectChanges();                              //資料集所有資料表記錄都放棄更新
                MessageBox.Show("寫入刪除記錄失敗!" + e.ToString());
                deltf = false;
            }
            return deltf;
        }

        #endregion

        #region 資料查詢
        //查詢
        private void btnSeek_Click(object sender, EventArgs e)
        {
            ae = "S";
            Lbtn.Clear();
            Lbtn = new List<ToolStripButton>() { btnAdd, btnDel, btnEdit, btnUpdate, btnCancel, btnSeekDo, btnRefresh };
            getdata.SetMode(Lbtn, false);

            btnSeekDo.Enabled = true;
            //Lbtn.Clear();
            //Lbtn = new List<ToolStripButton>() { btnSeekDo };
            //SetMode(Lbtn, true);

            getdata.DataInit(fno, tBox, ftype, flenB);  //DataInit();
            ReadOnlyFalse();
        }

        //執行查詢
        private void btnSeekDo_Click(object sender, EventArgs e)
        {
            if (ae == "S")
            {
                try
                {
                    int i;
                    string str="", ss;
                    str = " 2>1 ";
                    for (i = 0; i <= dt.Columns.Count - 1; i++)
                    {
                        ss = tBox[i].Text.Trim();
                        //MessageBox.Show("i=" + i + "; ss=" + ss);
                        if (ss != "")
                        {
                            // 1:String,  2:Boolean, 3:DateTime, 4:整數, 5:小數
                            if (ftype[i] == 1)
                            {
                                str += " and " + dt.Columns[i].ToString() + " Like '%" + ss + "%' ";   //文字
                            }
                            else if ((ftype[i] == 2) && (ss.Substring(0, 1) == "T" || ss.Substring(0, 1) == "F"))
                            {
                                if (ss.Substring(0, 1) == "F")
                                    ss = "0";
                                else
                                    ss = "1";
                                str += " and " + dt.Columns[i].ToString() + " = " + ss;                 //Boolean
                            }
                            if (ftype[i] == 3)                                                          //日期
                            {
                                str += " and " + dt.Columns[i].ToString() + " = '" + ss + "'";
                                /*
                                DateTime date1 = Convert.ToDateTime(ss);
                                str += string.Format(" and " + dt.Columns[i].ToString() + " = #{0}#",
                                    date1.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture));            
                                */
                            }
                            else if ((ftype[i] == 4 || ftype[i] == 5) && (ss != "0") && (ss != "0.0"))  //數字型態
                            {
                                str += " and " + dt.Columns[i].ToString() + " = " + ss;
                            }
                        }
                    }
                    //MessageBox.Show("str=" + str);
                    bds.Filter = str;
                    BindingData();                  //tBox[i]、dataGridView1 作 Binding
                    bds_PositionChanged(sender, e);
                }
                catch (Exception er)
                {
                    //MessageBox.Show(er.Message + ", Data Error");
                    MessageBox.Show(er.ToString());
                }
                finally
                {
                    Lbtn.Clear();
                    Lbtn = new List<ToolStripButton>() { btnSeekDo };
                    getdata.SetMode(Lbtn, false);

                    Lbtn.Clear();
                    Lbtn = new List<ToolStripButton>() { btnRefresh };
                    getdata.SetMode(Lbtn, true);
                }
            }
        }

        //重置
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            string mfieldname = dt.Columns[0].ColumnName;     //主索引欄位
            string fText = tBox[0].Text;
            bds.Filter = "";
            int po1 = bds.Find(mfieldname, fText);          //該欄位必須有排序
            po1 = po1 == -1 ? 0 : po1;                      //如果找不到資料
            bds.Position = po1;
            bds_PositionChanged(sender, e);
            //dataGridView1.CurrentCell = dataGridView1.Rows[po1].Cells[0];     //同上
            //上述作法可以使用先查詢特定筆數，重置後指標停於原目標位置，再作編修。

            Lbtn.Clear();
            Lbtn = new List<ToolStripButton>() { btnSeekDo, btnRefresh };
            getdata.SetMode(Lbtn, false);

            Lbtn.Clear();
            Lbtn = new List<ToolStripButton>() { btnAdd, btnDel, btnEdit, btnUpdate, btnSeek };
            getdata.SetMode(Lbtn, true);
        }
        #endregion

        #region Form 事件
        private void FSingle_Load(object sender, EventArgs e)
        {
            cnstr = Main.cnstr;
            con = new SqlConnection(cnstr);
            LbTbName.Text = LbTable;            //LbTable由FormMain指派 
            dgv = new DgvSet(); 
            getdata = new GetData();
            getdata.cnstr = cnstr;
            path = dir.Parent.Parent.Parent.FullName;
        }

        private void FSingle_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnClose_Click(sender,e);
        }

        private void Fsingle_SizeChanged(object sender, EventArgs e)
        {
            dataGridView1.Width = this.Width - (832 - 680);
        }
        #endregion

        #region 一般自訂事件
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

        /*
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
                    (fieldname.IndexOf("Short") != -1) || (fieldname.IndexOf("Long") != -1) )
                    tp = 4;        //整數
                else                                            //如有其他型態會有問題，屆時再增加判斷條件
                    tp = 5;        //小數
                ftype[i] = tp;

                //ftype[i] = fieldtype(fieldname);     // 1:String,  2:Boolean, 3:DateTime, 4:整數, 5:小數
                //MessageBox.Show("ftype["+i.ToString()+"]="+ ftype[i].ToString()+"; "+ fieldname);
            }
            return ftype;
        }
        */
        private void btnResume()        //取消、寫入失敗時用
        {
            Lbtn.Clear();
            Lbtn = new List<ToolStripButton>() { btnUpdate, btnCancel };
            getdata.SetMode(Lbtn, false);

            Lbtn.Clear();
            Lbtn = new List<ToolStripButton>() { btnAdd, btnDel, btnEdit, btnSeek };
            getdata.SetMode(Lbtn, true);
            ReadOnlyTrue();
        }
        /*
        private void SetMode(List<ToolStripButton> mybtn, bool tf)
        {
            for (int i = 0; i <= mybtn.Count() - 1; i++)
            {
                mybtn[i].Enabled = Convert.ToBoolean(tf);
            }
        }
        */
        /*
        public void dgvSet(DataGridView dgv)                    //dataGridView1 設定
        {
            //使用Binding，不能設定行、列數
            // Set the column header style.
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
            dgv.EnableHeadersVisualStyles = false;
            columnHeaderStyle.BackColor = Color.FromArgb(125, 225, 255);                    //必須要上列設定才能有效   淺藍色
            columnHeaderStyle.ForeColor = Color.FromArgb(0, 0, 255);                        //字體色  藍色;
            columnHeaderStyle.Font = new Font("Arial", 12, FontStyle.Regular);              //字型,大小,字體
            columnHeaderStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;        //水平-置中(必須關閉欄位排序功能)
            dgv.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
            // Set the row header style.
            DataGridViewCellStyle rowHeaderStyle = new DataGridViewCellStyle();
            dgv.EnableHeadersVisualStyles = false;
            rowHeaderStyle.BackColor = Color.FromArgb(125, 225, 255);                       //必須要上列設定才能有效   淺藍色
            dgv.RowHeadersDefaultCellStyle = rowHeaderStyle;

            //Cell 資料文字設定
            dgv.Font = new Font("Arial", 12, FontStyle.Regular);                            //字型,大小,字體
            dgv.ForeColor = Color.FromArgb(0, 0, 128);                                      //字體色  藍色;
            //背景色
            dgv.BackgroundColor = Color.FromArgb(255, 255, 204);                            //dataGridView1 背景色  淺黃色   
            dgv.DefaultCellStyle.BackColor = Color.FromArgb(255, 225, 200);                 //Cells背景色 淺茶色
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(201, 245, 255);  //偶數列背景色  淺藍色   
            //dgv.RowHeadersVisible = false;                                                //RowHeaders不顯示
            dgv.AutoResizeColumns();                                                        //欄寬自動調整 
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
        */
        /*
        public void GetColumnsDescrip(string tbn)
        {
            //string desc = "SELECT value FROM::fn_listextendedproperty(NULL, 'user', 'dbo', 'table', '" + tbn + "','column','" + cln + "')";
            string desc= "SELECT  VALUE FROM fn_listextendedproperty(NULL, 'schema', 'dbo', 'table', '" + tbn + "', 'column', DEFAULT)";
            con.Open();
            SqlCommand cmd = new SqlCommand(desc, con);
            SqlDataReader dr;
            DataTable tb = new DataTable();
            dr = cmd.ExecuteReader();
            tb.Load(dr);
            con.Close();
            DescList.Clear();
            for (int i = 0; i < tb.Rows.Count; i++)
                 DescList.Add(tb.Rows[i][0].ToString());
            tb.Dispose();
            cmd.Dispose();
        }
        */

        private void ReadOnlyTrue()                 //設定tBox[]、dataGridView1、dataGridView2為 ReadOnly=true 
        {
            //設定TextBox解除 ReadOnly
            for (int i = 0; i < dt.Columns.Count; i++)
                tBox[i].ReadOnly = true;
            dataGridView1.ReadOnly = true;
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
