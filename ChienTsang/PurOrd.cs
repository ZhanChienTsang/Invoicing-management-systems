using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;


namespace ChienTsang
{
    public partial class PurOrd : Form
    {
        public PurOrd()
        {
            InitializeComponent();
        }
        public static string LbTable;
        string dbfm, dbfd, dbfcu, dbffg;            //dbf為Table名稱
        string sqlm, sqld, sqlc, sqlf, ae = "";       //sql 為開啟 Table的語法；ae為狀態
        SqlConnection con;
        SqlDataAdapter dam, dad, dacu, dafg;
        DataSet ds;                                 //ds(主、明細檔), ds1(YCUST,YFGMAST)併入 ds
        SqlCommandBuilder budM, budD, budfg;        //Yfgmast有庫存更新
        DataTable tbm, tbd, tbcu;
        public DataTable tbfg;
        BindingSource bdsM, bdsD;                   //本版不使用DataRowView，故增加 bdsD 
        DataView dvfg, dvcu;
        DataRelation relation1;     //, relation2, relation3;
        DataColumn table1Column, table2Column;  //, table3Column, table4Column, table5Column, table6Column;  //關聯欄位

        TextBox[] tBox;
        Label[] Lb;
        int[] ftypM, ftypD, flenB;                  //以ftyp[]替代ftype變數(自訂欄位資料型態，以數字表示)；flenB 為 tBox[i].Text的設定長度
        ComboBox combo;
        //bool MasterEnd;                             //主檔新增完成
        int fno, po, idx;                           //fno為 tb 的欄位數, po 為tb當下指標位置, idx 表主索引的欄位序
        string fsort, mdc;                          //fsort 主索引欄名；  mdc 判斷 dc 寫入stock 時判斷 + or -
        public string MFGNO, cnstr;                 //public MFGNO  由Form2選定商品型號，讓明細檔填入Cell，cnstr為連線字串 

        DgvSet dgv;                                 //Class DgvSet          
        GetData getdata;                            //Class GetData            
        List<ToolStripButton> Lbtn = new List<ToolStripButton>();
        List<string> FlistM = new List<string>();   //中文欄位名稱陣列(主檔);
        List<string> FlistD = new List<string>();   //中文欄位名稱陣列(明細檔)


        #region 泛型相關

        // 泛型作法：1. 設class FileFg   2. 宣告List<FileFg> Mfg  3. 將商品資料加入 GetFgData()  4. 設回傳方法 GetFgname(string mfgno) 的方法1
        public class FileFg      // 自訂class和變數
        {
            public string fg_no { get; set; }
            public string fg_name { get; set; }
            public int fg_qty { get; set; }

        }

        List<FileFg> Sfg0, Sfg;                             //Gdata()、WStock() 的 fg0(), fg()用
        public List<FileFg> Mfg = new List<FileFg>();       //宣告一個泛型，內含成品編號、成品名稱、數量以0填入

        private void GetFgData()                            //取fgno,fgname到泛形 Mfg_FileFg 
        {
            //存入資料
            for (int i = 0; i < tbfg.Rows.Count; i++)
            {
                Mfg.Add(new FileFg() { fg_no = tbfg.Rows[i][0].ToString(), fg_name = tbfg.Rows[i][1].ToString(), fg_qty = 0 });
            }
        }
        #endregion

        #region 開檔相關

        private void CreateRelation()   //取 Table 主檔、明細檔並作關聯
        {
            ds = new DataSet();
            sqlm = "SELECT * FROM " + dbfm + " Order By " + fsort;
            sqld = "SELECT * FROM " + dbfd + " Order By " + fsort + ", od ";   //**

            //*主檔、明細檔加入Dataset ds 並取主檔 tbm
            dam = new SqlDataAdapter(sqlm, con);
            budM = new SqlCommandBuilder(dam);    //直接用 Update 而非使用 UpdateCommand 時用
            dam.Fill(ds, dbfm);
            tbm = ds.Tables[0];
            //置入Dataset ds 並取 Table 明細檔 tbd
            dad = new SqlDataAdapter(sqld, con);
            budD = new SqlCommandBuilder(dad);    //直接用 Update 而非使用 UpdateCommand 時用
            dad.Fill(ds, dbfd);
            tbd = ds.Tables[1];

            //*加入客戶(廠商)檔與商品檔
            sqlc = "select * from " + dbfcu + " order by cono";      //YCUST
            sqlf = "select * from " + dbffg + " order by fgno";     //YFGMAST

            //置入Dataset ds 並取主檔 tbcu
            dacu = new SqlDataAdapter(sqlc, con);
            //budcu = new SqlCommandBuilder(dacu);    //客戶檔不需要更新，因此不需要使用SqlCommandBuilder
            dacu.Fill(ds, dbfcu);
            tbcu = ds.Tables[2];
            dvcu = new DataView(tbcu);  //取得DataView
            dvcu.Sort = "cono";         //DataView 排序

            //置入Dataset ds 並取 Table tbfg
            dafg = new SqlDataAdapter(sqlf, con);
            budfg = new SqlCommandBuilder(dafg);
            dafg.Fill(ds, dbffg);
            tbfg = ds.Tables[3];
            dvfg = new DataView(tbfg);  //取得DataView
            dvfg.Sort = "fgno";         //DataView 排序

            //creating data relations 
            relation1 = null;      //relation2 = null;    //relation3 = null;

            ///*retrieve column (主檔_明細檔)
            table1Column = ds.Tables[0].Columns[fsort];     //單據編號
            table2Column = ds.Tables[1].Columns[fsort];
            //relating tables 
            relation1 = new DataRelation("relation1", table1Column, table2Column);
            //assign relation to dataset 
            ds.Relations.Add(relation1);

            /*//--暫不使用
            ///retrieve column (主檔_客戶檔)
            table3Column = ds.Tables[2].Columns["CONO"];       // 客戶編號   CONO
            table4Column = ds.Tables[0].Columns["CONO"];       // 進銷主檔   CONO
            //relating tables                                                   
            relation2 = new DataRelation("relation2", table3Column, table4Column);
            //assign relation to dataset 
            ds.Relations.Add(relation2);

            ///retrieve column (明細檔_商品檔)
            table5Column = ds.Tables[3].Columns["FGNO"];       // 商品編號    FGNO
            table6Column = ds.Tables[1].Columns["FGNO"];       // 進銷明細檔  FGNO
            //relating tables                                                   
            relation3 = new DataRelation("relation3", table5Column, table6Column);
            //assign relation to dataset 
            ds.Relations.Add(relation3);
            */
        }

        private void CreateDataColumn()             //建立主、明細檔衍生的計算欄位
        {
            //明細檔小計金額
            DataColumn colAmt = new DataColumn("金額");
            colAmt.DataType = System.Type.GetType("System.Int32");
            colAmt.Expression = "Qty * Prc";
            tbd.Columns.Add(colAmt);

            //明細檔成品名稱   
            DataColumn colfg = new DataColumn("商品名稱");
            colfg.DataType = System.Type.GetType("System.String");
            colfg.MaxLength = 100;
            //colfg.Expression = "Qty * Prc";
            tbd.Columns.Add(colfg);

            //主檔銷售金額
            DataColumn colTamt = new DataColumn("總金額");
            colTamt.DataType = System.Type.GetType("System.Int32");
            colTamt.Expression = "Sum(Child.金額)";
            tbm.Columns.Add(colTamt); 
            tbm.Columns.Add(new DataColumn("廠客名稱", typeof(string)));    //主檔dgv客戶簡稱欄位：*****

            //tbm.Columns.Add(colTamt);  訂單可運,進銷不能運
            // tbm.Columns.Add(new DataColumn("廠客名稱", typeof(string)));  進銷可運,訂單不能運
        }

        /*
        private void CreateCustFgmast()             //建立 ds1    //改加入ds，因此暫不使用
        {
            dbfcu = "YCUST";                        //**
            dbffg = "YFGMAST";                      //**
            string sqlc, sqlf;
            sqlc = "select * from YCUST order by cono";
            sqlf = "select * from YFGMAST order by fgno";
            ds1 = new DataSet();

            //置入Dataset ds 並取主檔 tbm
            dacu = new SqlDataAdapter(sqlc, con);
            //budcu = new SqlCommandBuilder(dacu);    //客戶檔不需要更新，因此不需要使用SqlCommandBuilder
            dacu.Fill(ds1, dbfcu);
            tbcu = ds1.Tables[0];

            //置入Dataset ds1 並取 Table 明細檔 tbfg
            dafg = new SqlDataAdapter(sqlf, con);
            budfg = new SqlCommandBuilder(dafg);
            dafg.Fill(ds1, dbffg);
            tbfg = ds1.Tables[1];
        }
        */

        private void FillFgname()           //明細檔填入商品名稱
        {
            //取得商品名稱給衍生欄位(明細檔) fgname 
            string mfgno;
            for (int i = 0; i < tbd.Rows.Count; i++)
            {
                mfgno = tbd.Rows[i][2].ToString();           //商品編號(明細檔)
                tbd.Rows[i][tbd.Columns.Count - 1] = getdata.GetNo_Name(mfgno, dvfg, "fgname"); //DataView.Find()
                //**getdata.GetFgname(mfgno, tbfg);                     //DataTable.Selct()
            }

            string mcono;
            for (int i = 0; i < tbm.Rows.Count; i++)
            {
                mcono = tbm.Rows[i][2].ToString();           //**客戶編號
                tbm.Rows[i][tbm.Columns.Count - 1] = getdata.GetCuname(mcono, tbcu);
            }

        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            dbfm = LbTable;     //dbfm = comboBox1.Text;
            if (dbfm == "YODR")                     //**
                dbfd = "YODRDT";
            if (dbfm == "YFGIO")                    //**
                dbfd = "YFGIODT";
            dbfcu = "YCUST";                        //**
            dbffg = "YFGMAST";                      //**

            fsort = getdata.GetPrimekey(dbfm);   //Class GetData 取得主索引 fsort
            if (fsort == "沒有資料")
            {
                MessageBox.Show("請先將 " + dbfm + " 建立主索引！");
                this.Dispose();
            }
            else
            {
                ae = "";                        //BindingData();會作判斷
                FlistM = getdata.GetColumnsDescrip(dbfm);        //取得中文欄名
                FlistD = getdata.GetColumnsDescrip(dbfd);        //取得中文欄名

                //取 Table 主檔、明細檔並作建立Relation關聯
                CreateRelation();               //取 Table 主檔、明細檔並作關聯
                CreateDataColumn();             //增加明細檔金額欄位
                //CreateCustFgmast();           //建立CUST,FGMAST之Table //併入ds，暫不使用
                FillFgname();                   //明細檔填入商品名稱

                ftypM = getdata.CreateFtypeArray(tbm);   //取Master檔資料型態當 ftypM[]
                ftypD = getdata.CreateFtypeArray(tbd);   //取Detail檔資料型態當 ftypD[]
                FreetBox_LbArray();             //tBox[]、Lb[] 如果已存在,作釋放

                //建立 UserInterface 
                fno = tbm.Columns.Count;        //必要,後有 flenB[fno]  
                UiDataCreate(fno);              //動態建立tBox[]、Lb[]

                //以泛型取商品資料 (衍生檔用，新增、編修採開新Form作法)
                GetFgData();                    //泛型作法可替代方案，使用時，Getfgname()需改用方法1
                ComBoBoxGetData();              //ComBoBox 取公司資料。

                //BindingSource 相關作業，設定tBox[] Binding、dataGridView1的資料繫結(BindingSource)
                BindingData();                  //設定tBox[] Binding、dataGridView1控制項顯示BindingSource的資料來源

                //事件建立
                bdsM.PositionChanged += new EventHandler(bdsM_PositionChanged);
                bdsM.MoveLast();               //末筆，因開啟是首筆，到末筆才會觸發 bds_PositionChanged

                /// <summary> DataGridView 相關設定
                dgv.dgvSet(dgvM);          //dgvM 設定(ClsaaSet)
                dgv.dgvSet(dgvD);          //dgvD 設定(ClsaaSet)
                dgv.dgv_Align(dgvM, tbm, ftypM);    //文字靠左、數字靠右
                dgv.dgv_Align(dgvD, tbd, ftypD);    //文字靠左、數字靠右
                dgvM.AllowUserToAddRows = false;    //主檔使其無法新增
                dgvD.AllowUserToAddRows = false;    //明細檔使其無法新增在[新增]時開啟
                /// </summary>

                ReadOnlyTrue();                     //tBox[x]以及DataGridView皆設 ReadOnly=true;，新增、編修再ReadOnlyFalse() 

                Lbtn.Clear();
                Lbtn = new List<ToolStripButton>() { btnAdd, btnDel, btnEdit, btnSeek };      //取消、寫入免納入，因程序內控制ae="A"、"E"
                getdata.SetMode(Lbtn, true);

                Lbtn.Clear();
                Lbtn = new List<ToolStripButton>() { btnCancel, btnUpdate, btnSeekDo, btnRefresh };
                getdata.SetMode(Lbtn, false);
            }
        }

        private void UiDataCreate(int n)            //建立UserInterface            
        {
            int x, y, j;
            tBox = new TextBox[n];
            Lb = new Label[n];
            Font font = new Font("Arial", 12);

            //取 Schema(tb1) 的欄位長度  flenB[]
            flenB = getdata.GetStrLength(fno, fsort, dbfm, ref idx);
            x = 0;
            for (j = 0; j <= n - 1; j++)
            {
                if (j % 2 == 1)
                {
                    y = 1;
                    x = x - 1;
                }
                else
                    y = 0;
                tBox[j] = new TextBox();
                tBox[j].Name = string.Format("tBox{0}", j); //tBox0、tBox1...
                tBox[j].Left = 100 + 420 * y;
                tBox[j].Top = 10 + 40 * x;
                tBox[j].Height = 30;
                tBox[j].Font = font;

                if (ftypM[j] == 4 || ftypM[j] == 5)                   // 1:String,  2:Boolean, 3:DateTime, 4:整數, 5:小數
                    tBox[j].TextAlign = HorizontalAlignment.Right;
                else
                    tBox[j].TextAlign = HorizontalAlignment.Left;
                switch (ftypM[j])
                {
                    case 1:
                        tBox[j].Width = Math.Min((flenB[j] * 12), 310);      //只取寬，無空白。　　
                        break;
                    case 2:
                        tBox[j].Width = 6 * 8;
                        break;
                    case 3:
                        tBox[j].Width = 11 * 8;
                        break;
                    case 4:
                        tBox[j].Width = 10 * 8;           //數值型態
                        break;
                    case 5:
                        tBox[j].Width = 10 * 8;           //數值型態
                        break;
                }

                tBox[j].BackColor = Color.FromArgb(255, 255, 212);
                tBox[j].ForeColor = Color.FromArgb(0, 0, 153);
                panel1.Controls.Add(tBox[j]);

                Lb[j] = new Label();
               // Lb[j].Text = tbm.Columns[j].ColumnName;
                if (j < FlistM.Count)
                {
                    //MessageBox.Show($"{FlistM[j]}");
                    if (FlistM[j].Trim() != "")
                        Lb[j].Text = FlistM[j];                          //中文欄名(描述)
                    else
                        Lb[j].Text = tbm.Columns[j].ColumnName;
                }
                else if (j == FlistM.Count)     //衍生金額欄，再下一欄客戶名稱不處理
                    Lb[j].Text = "總金額";
                Lb[j].Top = 13 + 40 * x;
                Lb[j].Left = 10 + 420 * y;
                Lb[j].Width = 80;
                Lb[j].Height = 25;
                Lb[j].Font = font;
                panel1.Controls.Add(Lb[j]);
                x++;
            }
            tBox[2].Width = 80;
            tBox[2].Enter += new System.EventHandler(this.tBox2_Enter);
            if (dbfm == "YFGIO")
                tBox[4].Enter += new System.EventHandler(this.tBoxN_Enter);      //TAMT
            else
                tBox[3].Enter += new System.EventHandler(this.tBoxN_Enter);      //TAMT

            //畫面隨資料表欄位多少自動調整畫面的高

            panel1.Height = 10 + (int)(fno * 1.0 / 2 + 0.5) * 40 + 20 + 15;   //本式替代下式
            //panel1.Height = 10 + Convert.ToInt32(Math.Ceiling((decimal)(fno*1.0 / 2))) * 40 + 20 + 15;        //Ceiling(decimal) 或 Ceiling(double) 
            label3.Top = panel1.Height - 35;
            label4.Top = panel1.Height - 35;
            dgvM.Top = panel1.Top + panel1.Height + 15;
            dgvM.Height = 220;
            dgvD.Top = dgvM.Top + dgvM.Height + 15;
            label5.Top = dgvD.Top + dgvD.Height - 30;
            this.Height = dgvD.Top + dgvD.Height + 60;

        }

        private void BindingData()                  //控制項的資料繫結
        {
            //建立BindingSource物件 //不使用dvmng
            if (ae != "S")
            {
                bdsM = new BindingSource(ds, dbfm);             //(DataSource , DataMember)
                bdsD = new BindingSource(bdsM, "relation1");    //110.05.31
            }
            //客戶檔、商品檔不適合與主、檔明細檔作關聯，但可以置在同一個DataSet ***|||

            //設定DataGridView控制項顯示BindingSource的資料來源
            dgvM.DataSource = bdsM;
            dgvD.DataSource = bdsD;

            //設定TextBox控制項的資料繫結
            for (int i = 0; i <= tbm.Columns.Count - 1; i++)
            {
                tBox[i].DataBindings.Add("Text", bdsM, tbm.Columns[i].ColumnName);
                if (i < FlistM.Count)
                    dgvM.Columns[i].HeaderText = FlistM[i];//**中文欄名(主檔)
            }
            for (int i = 0; i <= tbd.Columns.Count - 1; i++)
                if (i < FlistD.Count)
                    dgvD.Columns[i].HeaderText = FlistD[i];    //**中文欄名(明細檔)

        }

        private void BindingDataOff()               //取消控制項的資料繫結
        {
            //設定TextBox控制項的資料繫結
            for (int i = 0; i <= tbm.Columns.Count - 1; i++)
            {
                tBox[i].DataBindings.Clear();
            }
            dgvM.DataSource = null;
            dgvD.DataSource = null;
        }

        #endregion

        #region 動態事件
        private void bdsM_PositionChanged(Object sender, EventArgs e)
        {
            po = bdsM.Position;
            for (int j = 0; j < tbm.Columns.Count - 1; j++)
            {
                if (tbm.Columns[j].DataType.ToString().IndexOf("DateTime") != -1)
                {
                    if (tbm.Rows[po][j].ToString().Trim() == "")
                        tBox[j].Text = "";
                    else
                        tBox[j].Text = DateTime.Parse((tbm.Rows[po][j].ToString())).ToString("yyyy/MM/dd");  //只顯示日期格式，不要時,分,秒
                }
            }

            btnTop.Enabled = (bdsM.Position > 0);
            btnPrior.Enabled = (bdsM.Position > 0);
            btnNext.Enabled = (bdsM.Position < bdsM.Count - 1);
            btnLast.Enabled = (bdsM.Position < bdsM.Count - 1);

            label3.Text = "記錄數：" + (bdsM.Position + 1).ToString() + "/" + (bdsM.Count).ToString();  //本式必須在下段前

            //主檔取公司名稱
            label2.Text = getdata.GetNo_Name(tBox[2].Text, dvcu, "name"); //**原getdata.GetCuname(tBox[2].Text, tbcu);

        }

        private void ComBoBoxGetData()                      //ComBoBox 取資料
        {
            Font font = new Font("Arial", 12);
            //動態建立combo (ComBOBox)
            combo = new ComboBox();
            combo.Left = 188;
            combo.Top = 52;
            combo.Width = 180;
            combo.Height = 24;
            combo.Font = font;
            //combo.TabIndex = 52;            //以原手動設定之值(控制項在容器中的定位順序)
            combo.Text = "--請選擇公司--";
            combo.BackColor = Color.FromArgb(192, 255, 255);
            combo.BringToFront();           //移到最上層
            panel1.Controls.Add(combo);
            combo.SelectedIndexChanged += new System.EventHandler(combo_SelectedIndexChanged);

            //設定DataSource取資料
            combo.DataSource = tbcu;
            combo.DisplayMember = "NAME";
            combo.ValueMember = "CONO";
            combo.SelectedItem = null;
            combo.SelectedText = "--請選擇公司--";
            combo.Visible = false;                  //暫隱藏，點選tBox[x]時再出現。
        }

        private void combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((ae == "A") || (ae == "E"))
            {
                int ind = combo.SelectedIndex;
                if (ind >= 0 && ind < combo.Items.Count)
                    tBox[2].Text = combo.SelectedValue.ToString();
            }
            combo.Visible = false;
            label2.Visible = true;
            label2.Text = combo.Text;
        }

        private void tBox2_Enter(object sender, EventArgs e)
        {
            if ((ae == "A") || (ae == "E"))
            {
                combo.Visible = true;                       //Combo與label2重疊,因此一顯示,另一隱藏。
                label2.Visible = false;
            }
        }

        private void tBoxN_Enter(object sender, EventArgs e)
        {
            if (ae == "A")
            {
                bool tf = getdata.Check_Blank(tBox, Lb);        //(類別方法)
                if (tf == true)
                {
                    //MasterEnd = true;                           //主檔輸入完成， MasterEnd = true 讓dgvD_Click判斷作存主檔。
                    //if (ae == "A" && MasterEnd == true)
                    {
                        //使用tBox[]新增
                        DataRow dr = ds.Tables[0].NewRow();
                        dr.BeginEdit();
                        getdata.GetDataRow(ref dr, tBox, ftypM);
                        /*
                        for (int j = 0; j < fno; j++)
                        {
                            //寫入DataRow dr[]    //可免寫入 dataGridView1，重開檔即可
                            if (tBox[j].Text == null)
                                continue;

                            //寫入DataRow dr
                            if (ftypM[j] == 1 || ftypM[j] == 3)    // 1:String,  2:Boolean, 3:DateTime, 4:整數, 5:小數
                            {
                                if (ftypM[j] == 3 && tBox[j].Text == "")
                                    dr[j] = "1900/1/1";
                                else
                                    dr[j] = tBox[j].Text;
                            }
                            else if (ftypM[j] == 2)
                            {
                                if (tBox[j].Text.Substring(0, 1) == "F")
                                    dr[j] = 0;
                                else
                                    dr[j] = 1;
                            }
                            else if (ftypM[j] == 4)
                                dr[j] = Convert.ToInt32(tBox[j].Text);
                            else if (ftypM[j] == 5)
                                dr[j] = Math.Round(Convert.ToSingle(tBox[j].Text), 4, MidpointRounding.AwayFromZero);
                        }
                        */
                        dr.EndEdit();
                        ds.Tables[0].Rows.Add(dr);
                        BindingDataOff();
                        BindingData();
                        bdsM.PositionChanged += new EventHandler(bdsM_PositionChanged);     //事件必需重新設
                        bdsM.MoveLast();                    //移到末筆，可觸發bdsM_PositionChanged
                        btnUpdate.Enabled = true;

                        dgv.dgv_Align(dgvM, tbm, ftypM);    //文字靠左、數字靠右
                        dgv.dgv_Align(dgvD, tbd, ftypD);    //文字靠左、數字靠右
                        //MasterEnd = false;
                        tBox[0].ReadOnly = true;            //存檔後VHNO不能修改
                        dgvD.Columns[0].ReadOnly = true;    //VHNO不可修改
                        label4.Visible = false;             //提示 tBox_End
                        label5.Visible = true;              //提示 點選型號
                    }
                }
            }

        }
        #endregion

        #region 指標異動
        private void btnTop_Click(object sender, EventArgs e)
        {
            bdsM.MoveFirst();                    //首筆
        }

        private void btnPrior_Click(object sender, EventArgs e)
        {
            bdsM.MovePrevious();                 //上筆
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            bdsM.MoveNext();                     //下筆
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            bdsM.MoveLast();                     //末筆
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
                        Lb[j].Dispose();
                        flenB[j] = 0;     //tb的結構表(tb1)，取得文字欄位的設定長度
                    }
                }
                //MessageBox.Show("釋放完畢!"+ Convert.ToString(Lb.Count()));
            }
            dgvM.DataSource = null;
            dgvD.DataSource = null;
        }

        private void ReadOnlyTrue()                 //設定tBox[]、dataGridView1、dataGridView2為 ReadOnly=true 
        {
            //設定TextBox控制項的資料繫結
            for (int i = 0; i <= tbm.Columns.Count - 1; i++)
            {
                tBox[i].ReadOnly = true;
            }
            dgvM.ReadOnly = true;
            dgvD.ReadOnly = true;
            //dgvD.Columns[0].ReadOnly = true;       //不能設，否則上一式會受影響，改在新增、編修時設定
        }

        private void ReadOnlyFalse()                 //設定tBox[]、dataGridView1、dataGridView2為 ReadOnly=true 
        {
            //設定TextBox控制項的資料繫結
            for (int i = 0; i <= tbm.Columns.Count - 1; i++)
            {
                tBox[i].ReadOnly = false;
            }
            dgvM.ReadOnly = false;           //需要時另外解除 ReadOnly
            dgvD.ReadOnly = false;
        }

        private void btnResume()        //取消、寫入失敗時用
        {
            Lbtn.Clear();
            Lbtn = new List<ToolStripButton>() { btnUpdate, btnCancel };
            getdata.SetMode(Lbtn, false);

            Lbtn.Clear();
            Lbtn = new List<ToolStripButton>() { btnAdd, btnDel, btnEdit, btnSeek };
            getdata.SetMode(Lbtn, true);
        }

        private void ReShowData()               //重作tBox[]、dgv1、dgv2之資料繫結
        {
            BindingDataOff();
            BindingData();
            bdsM.PositionChanged += new EventHandler(bdsM_PositionChanged);     //事件必需重新設

            bdsM.MoveLast();                    //移到末筆，可觸發bdsM_PositionChanged
            btnResume();
            dgv.dgv_Align(dgvM, tbm, ftypM);    //文字靠左、數字靠右 (DgvSet dgv)
            dgv.dgv_Align(dgvD, tbd, ftypD);    //文字靠左、數字靠右
            //ae = "";                          //新增、編修、刪除寫入各自清除
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
        #endregion

        #region 資料維護
        //新增
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ae = "A";
            label2.Text = "";
            //MasterEnd = false;          //主檔新增是否完成初始化
            Lbtn.Clear();
            Lbtn = new List<ToolStripButton>() { btnTop, btnPrior, btnNext, btnLast, btnAdd, btnDel, btnEdit, btnSeek };      //
            getdata.SetMode(Lbtn, false);

            Lbtn.Clear();
            Lbtn = new List<ToolStripButton>() { btnUpdate, btnCancel };      //
            getdata.SetMode(Lbtn, true);

            string maxvhno = dgvM.Rows[dgvM.RowCount - 1].Cells[0].Value.ToString();
            //MessageBox.Show("max=" + maxvhno);
            getdata.DataInit(fno, tBox, ftypM, flenB);  //類別方法 DataInit，包含tBox[j].DataSource.clear;
            dgvM.Enabled = false;                       //不可作用
            ReadOnlyFalse();                            //tBox[]、dgvM、dgvD

            string vhno1 = getdata.Set_Vhno(maxvhno, dbfm);   //取單據自動編號 
            tBox[0].Text = vhno1;
            if (dbfm == "YFGIO")
                tBox[1].Text = vhno1.Substring(0, 7);
            else
                tBox[1].Text = vhno1.Substring(1, 7);

            dgvD.AllowUserToAddRows = true;    //可新增
            label3.Text = "記錄數：";
            label4.Visible = true;
            dgvM.DataSource = null;
            dgvD.DataSource = null;
        }

        private void Leave_Click(object sender, EventArgs e)
        {
            //確定離開？
            if (MessageBox.Show("確定返回主頁面？", "Confirm Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                this.Dispose();
            }
        }

        //編修
        private void btnEdit_Click(object sender, EventArgs e)
        {
            ae = "E";
            Lbtn.Clear();
            Lbtn = new List<ToolStripButton>() { btnTop, btnPrior, btnNext, btnLast, btnAdd, btnDel, btnEdit, btnSeek };      //
            getdata.SetMode(Lbtn, false);

            Lbtn.Clear();
            Lbtn = new List<ToolStripButton>() { btnUpdate, btnCancel };
            getdata.SetMode(Lbtn, true);

            //this.KeyPreview = true;
            dgvM.Enabled = false;              //使其無法作用(點擊)，以tBox[]作編修
            ReadOnlyFalse();                   //解除tBox[x]=ReadOnly、dgvM、dgvD，但dgvM上式使其無法點擊，以作為編修
            tBox[idx].ReadOnly = true;         //idx(主所引欄位序)由程式取得,編修不得修改
            dgvD.Columns[0].ReadOnly = true;   //細檔的單據編號不可異動；ReadOnlyTrue()不能設，需在新增、編修設定
            dgvD.AllowUserToAddRows = true;    //編修可新增明細檔
            if (dbfm == "YFGIO")
            {
                GetFgData_ForStock(-1);                 //泛型 Sfg0,取明細檔的型號、數量，-1表編輯前應扣，後續再與Sfg比對
            }
            for (int i = 0; i <= tbm.Columns.Count - 1; i++)        //tBox 斷開 bdsM
                tBox[i].DataBindings.Clear();
        }

        //刪除
        private void btnDel_Click(object sender, EventArgs e)
        {
            Lbtn.Clear();
            Lbtn = new List<ToolStripButton>() { btnTop, btnPrior, btnNext, btnLast, btnAdd, btnDel, btnEdit, btnSeek };
            getdata.SetMode(Lbtn, false);
            ae = "D";
            if (MessageBox.Show("確定本筆主檔以及所屬明細檔資料一起刪除？", "Confirm Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {   //確定刪除？
                if (dbfm == "YFGIO")
                {
                    GetFgData_ForStock(-1);     //泛型 Sfg,取明細檔的型號、數量，-1表
                    Write_StockSQL();           //更新庫存數 By SQL 
                    //Write_Stock();            //更新庫存數 By ds1.Update()  
                }
                bool deltf = DeleteDb();        //刪主檔、明細檔

                if (deltf)                  //刪除成功
                {
                    btnOpen_Click(sender, e);   //重新開檔
                    //不需要作Lbtn 控制，因Open_Click會執行Close_Click()、OpenTable;
                }
                else
                {
                    btnResume();     //恢復btnAdd, btnDel, btnEdit, btnSeek=Enabled；btnUpdate, btnCancel=Disabled
                    bdsM_PositionChanged(sender, e);
                }
            }
            else
            {
                MessageBox.Show("刪除明細檔特定筆數，請於編修時，滑鼠點擊該筆，然後按 [Delete]！");
                ds.RejectChanges();                     //資料集(主、明細檔)所有記錄都放棄更新
                btnResume();     //恢復btnAdd, btnDel, btnEdit, btnSeek=Enabled；btnUpdate, btnCancel=Disabled
                bdsM_PositionChanged(sender, e);
            }
            ae = "";
        }

        //取消
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ds.RejectChanges();                     //資料集(主、明細檔)所有記錄都放棄更新
            if (ae == "A")
            {
                MessageBox.Show("放棄新增！");
                ReShowData();                         //重作tBox[]、dgv1、dgv2之資料繫結 (包含btnResume())
                po = dgvM.RowCount - 1;
            }

            if (ae == "E")
            {
                MessageBox.Show("放棄編修！");
                po = dgvM.CurrentRow.Index;
                //this.KeyPreview = false;            //回復；因在編修時 dataGridView2.KeyDown 無作用，設定KeyPreview=true，再改用this.KeyDown
                dgvM.CurrentCell = dgvM[0, po];         //指標移到特定筆
                btnResume();     //恢復btnAdd, btnDel, btnEdit, btnSeek=Enabled；btnUpdate, btnCancel=Disabled
                for (int i = 0; i <= tbm.Columns.Count - 1; i++)     //改於編修時斷開tBox，取消時需要重新Binding
                    tBox[i].DataBindings.Add("Text", bdsM, tbm.Columns[i].ColumnName);
            }
            ReadOnlyTrue();                      //tBox[]、dgvM、dgvD 一起處理 
            dgvM.Enabled = true;                 //恢復可作用
            //dgvM.AllowUserToAddRows = false;    //不可新增 ReadOnly=true，即不能新增
            //dgvD.AllowUserToAddRows = false;    //不可新增
            if (combo != null)
                combo.Visible = false;
            label2.Visible = true;
            label4.Visible = false;
            label5.Visible = false;
            bdsM.Position = po - 1;
            bdsM.Position = dgvM.CurrentRow.Index + 1;
            ae = "";
        }

        //寫入
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            bool aetf = false;
            int po1 = 0;

            if ((ae == "A") || (ae == "E"))
            {
                if (ae == "A")
                {
                    if (MessageBox.Show("確定寫入本筆資料？", "Confirm Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {   //確定寫入
                        if (dbfm == "YFGIO")
                        {
                            dgvD.CurrentCell = dgvD.Rows[bdsD.Position].Cells[0];       //移動欄位，否則當筆數量無更新   (1)
                            GetFgData_ForStock(1);          //取得 fg()
                        }
                        aetf = UpdateDb();
                    }
                    else   //讓不寫入與寫入不成功同樣重新Binding和Button設定
                    {
                        aetf = false;
                    }
                    po1 = dgvM.RowCount - 1;             //末筆

                }
                else        //if (ae == "E")
                {
                    po1 = dgvM.CurrentRow.Index;
                    if (MessageBox.Show("確定寫入本筆資料？", "Confirm Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {   //確定寫入
                        if (dbfm == "YFGIO")
                        {
                            dgvD.CurrentCell = dgvD.Rows[bdsD.Position].Cells[0];       //移動欄位，否則當筆數量無更新   (1)
                            GetFgData_ForStock(1);          //取得 fg()
                            //匯整 fg0()->fg()
                            CompareFg0_Fg();
                        }
                        aetf = UpdateDb();
                    }
                    else   //讓不寫入與寫入不成功同樣重新Binding和Button設定
                    {
                        aetf = false;
                    }
                }

                if (aetf == false)                          //
                {
                    ds.RejectChanges();                     //資料集(主、明細檔)所有記錄都放棄更新
                    ReShowData();                           //重作tBox[]、dgv1、dgv2之資料繫結、btnResume
                    //btnResume();                          //不使用ReShowData(); 而使用btnResume();，再新增寫入時會產生錯過。
                    bdsM_PositionChanged(sender, e);
                }
                else
                {
                    if (dbfm == "YFGIO")
                        Write_StockSQL();       //更新庫存數 By SQL
                                                //Write_Stock();            //更新庫存數 By ds1.Update()  

                    btnOpen_Click(sender, e);   //重新開檔
                    //不需要作Lbtn 控制，因Open_Click會執行Close_Click()、OpenTable;
                }
            }
            ReadOnlyTrue();        //tBox[x]回復 ReadOnly=true; dgvM、dgvD 皆設true
            dgvM.Enabled = true;   //恢復可作用(點擊)
            //dgvD.AllowUserToAddRows = false;    //當ReadOnly=true，即不能新增

            if (combo != null) { combo.Visible = false; }

            label2.Visible = true;
            label5.Visible = false;

            if (ae == "E")
            {
                //this.KeyPreview = false;            //回復；因在編修時 dataGridView2.KeyDown 無作用，設定KeyPreview=true，再改用this.KeyDown
                dgvM.CurrentCell = dgvM[0, po1];     //指標移到特定筆
            }
        }


        private bool UpdateDb()                 //更新寫入(新增、修改用)
        {
            bool editf = false;
            dgvM.Enabled = true;
            //先更新明細檔
            //MessageBox.Show("tbd=" + tbd.Rows.Count.ToString());      //(全部筆數)
            dgvD.CurrentCell = dgvD.Rows[bdsD.Position].Cells[0];       //移動欄位，否則當筆會無更新   (1)
            foreach (DataRowView drv in bdsD)                            //(2)   當修改某一欄位值，滑鼠如果不移動，(1)與(2)需要都使用，否則更新無效
            {
                drv.BeginEdit();
                drv.EndEdit();
            }

            //更新主檔
            //dgvM.CurrentCell = dgvM.Rows[0].Cells[0];     //方法1：移動指標，否則當筆會無更新  
            //方法2   (**本案例主擋欄位皆非人工輸入，可以不需要使用本法，使用方法1即可。
            DataRow dr = ds.Tables[0].Rows[bdsM.Position];
            dr.BeginEdit();
            try
            {
                getdata.GetDataRow(ref dr, tBox, ftypM);    //GetDataRow(ref dr);   
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
            editf = UpdateDatabase_ds();

            return editf;
        }

        private bool UpdateDatabase_ds()              //最後實際寫入
        {
            bool updatetf;  //= false;
            //寫入資料庫
            try
            {
                dam.Update(ds.Tables[dbfm]);       //不能寫 dam.Update(ds)，可寫dam.Update(ds.Tables[dbf]);
                dad.Update(tbd);
                MessageBox.Show("寫入更新記錄成功!");
                updatetf = true;
            }
            catch (System.Exception e)
            {
                ds.RejectChanges();                     //資料集所有資料表記錄都放棄更新
                MessageBox.Show("寫入更新記錄失敗!" + e.ToString());
                updatetf = false;
            }
            return updatetf;
        }

        private bool DeleteDb()                                 //實際刪除
        {
            bool deltf = false;

            //先Delete明細檔
            //MessageBox.Show("tbd=" + tbd.Rows.Count.ToString());      //(全部筆數)
            foreach (DataRowView dr in bdsD)                            //此處使用 DataRowView
            {
                dr.Delete();
            }

            //刪除主檔
            //int crow = dgvM.CurrentRow.Index;
            //DataRow drm = ds.Tables[0].Rows[crow];      //擬刪除該筆
            //drm.Delete();                               //主檔上方已改使用DataTable，編修刪除應使用 DataRow
            bdsM.RemoveCurrent();        //**本式替代上3式
            //寫入資料庫
            deltf = UpdateDatabase_ds();

            return deltf;
        }
        /*
        private void GetDataRow(ref DataRow dr)
        {
            for (int j = 0; j < fno - 1; j++)                     //**主檔末欄是虛擬，因此迴圈少一圈
            {
                //寫入DataRow dr[]
                if (ftypM[j] == 1 || ftypM[j] == 3)             //1:String,  2:Boolean, 3:DateTime, 4:整數, 5:小數
                {
                    if (ftypM[j] == 3 && tBox[j].Text == "")
                        dr[j] = "2000/1/1";
                    else
                        dr[j] = tBox[j].Text;
                }
                else if (ftypM[j] == 2)
                {
                    if (tBox[j].Text.Substring(0, 1) == "F")    //bool 值
                        dr[j] = 0;
                    else
                        dr[j] = 1;
                }
                else if (ftypM[j] == 4)                         //整數
                {
                    tBox[j].Text = tBox[j].Text == "" ? "0" : tBox[j].Text;  //**如果tBox[j].Text需要寫入null時，應 dr[j] = DBNull.Value;不能dr[j] = null
                    dr[j] = Convert.ToInt32(tBox[j].Text);
                }
                else if (ftypM[j] == 5)                         //小數
                {
                    tBox[j].Text = tBox[j].Text == "" ? "0" : tBox[j].Text;
                    dr[j] = Math.Round(Convert.ToSingle(tBox[j].Text), 4, MidpointRounding.AwayFromZero);
                }
            }
        }
        */
        #endregion

        #region 即時庫存異動
        private void CompareFg0_Fg()               //比對 fg0()和fg()
        {
            string fgno1;
            foreach (FileFg fg0 in Sfg0)
            {
                fgno1 = fg0.fg_no;
                int i = Sfg.FindIndex(x => x.fg_no == fgno1);         //比對修改前與修改後，先Find()，fg()同型號者  
                if (i >= 0)
                {
                    Sfg[i].fg_qty = Sfg[i].fg_qty + fg0.fg_qty;      //找到的話，以 fg.qty+fg0.qty，fg0為負值
                }
                else
                {   //找不到的話，表修改時型號被修改，則Sfg新增一筆，數量為fg0.fg_qty(負值)
                    Sfg.Add(new FileFg() { fg_no = fgno1, fg_name = "", fg_qty = fg0.fg_qty });
                }
            }
        }

        private void GetFgData_ForStock(int n)       //取fg0(),fg()到泛形 Sfg0、Sfg
        {
            int r = dgvM.CurrentRow.Index;
            mdc = tbm.Rows[r][3].ToString();

            int m = 0;
            if ((mdc == "D") || (mdc == "R")) { m = 1; }       // 
            else
            {
                if ((mdc == "C") || (mdc == "J")) { m = -1; }
                else { m = 0; }
            }

            if ((n == -1) && (ae == "E"))    //編修前 使用 fg0,tqty0 陣列
            {
                Sfg0 = new List<FileFg>();                  //宣告一個泛型  fg0()
                //存入資料
                for (int i = 0; i < bdsD.Count; i++)     //原dvD.Count,但不能使用dgvD.RowCount,會多一筆空白列。
                {
                    Sfg0.Add(new FileFg()
                    {
                        fg_no = dgvD.Rows[i].Cells[2].Value.ToString(),
                        fg_name = "",
                        fg_qty = int.Parse(dgvD.Rows[i].Cells[3].Value.ToString()) * m * n
                    });
                }
            }
            else
            {
                Sfg = new List<FileFg>();                   //宣告另一個泛型 fg()
                //MessageBox.Show("bdsD.Row=" + bdsD.Count.ToString() + "; dgvD.Row=" + dgvD.RowCount.ToString());
                //存入資料
                for (int i = 0; i < bdsD.Count; i++)        //原dvD.Count,但不能使用dgvD.RowCount,會多一筆空白列。
                {
                    Sfg.Add(new FileFg()
                    {
                        fg_no = dgvD.Rows[i].Cells[2].Value.ToString(),
                        fg_name = "",
                        fg_qty = int.Parse(dgvD.Rows[i].Cells[3].Value.ToString()) * m * n
                    });
                }
            }
        }

        private void Write_Stock()                              //將庫存數的異動寫入資料庫
        {
            //庫存數調整(寫入)
            string fgno1 = "";
            foreach (FileFg fg in Sfg)
            {
                if (fg.fg_qty != 0)                             //無變動不需寫入
                {
                    fgno1 = fg.fg_no;
                    DataRow[] drs;
                    string st = "fgno ='" + fgno1 + "'";        //st 作 tbfg.Select(條件)
                    drs = tbfg.Select(st);
                    foreach (DataRow row in drs)
                    {
                        row.BeginEdit();
                        row[6] = (int)row[6] + fg.fg_qty;        //**
                        row.EndEdit();
                    }
                }
            }

            //寫入資料庫
            try
            {
                dafg.Update(tbfg);                  //不能寫 da.Update(ds)，可寫da.Update(ds.Tables[dbf]);
            }
            catch (System.Exception e)
            {
                ds.RejectChanges();                 //資料集所有資料表記錄都放棄更新 原使用ds1
                MessageBox.Show("庫存寫入記錄失敗!" + e.ToString());
            }
        }

        private void Write_StockSQL()                            //將庫存數的異動寫入資料庫
        {
            //庫存數調整(寫入)
            foreach (FileFg fg in Sfg)
            {
                string fgno1, str;
                if (fg.fg_qty != 0)                             //無變動不需寫入
                {
                    fgno1 = fg.fg_no;
                    str = "Update " + dbffg + " set Iqty = Iqty + " + fg.fg_qty.ToString() + "  where fgno = '" + fgno1 + "' ";      //**
                    //寫入資料庫
                    try
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand(str, con);
                        cmd.ExecuteNonQuery();                          //執行資料庫指令SqlCommand
                        MessageBox.Show("庫存更新成功!");
                        con.Close();
                    }
                    catch (System.Exception e)
                    {
                        MessageBox.Show("庫存更新失敗! " + e.ToString());
                    }
                }
            }
        }


        #endregion

        #region 資料查詢
        //查詢
        private void btnSeek_Click(object sender, EventArgs e)
        {
            ae = "S";
            label2.Text = "";
            label3.Text = "記錄數：";
            dgvM.DataSource = null;
            dgvD.DataSource = null;

            Lbtn.Clear();
            Lbtn = new List<ToolStripButton>() { btnTop, btnPrior, btnNext, btnLast, btnAdd, btnDel, btnEdit, btnUpdate, btnCancel, btnSeekDo, btnRefresh };
            getdata.SetMode(Lbtn, false);

            Lbtn.Clear();
            Lbtn = new List<ToolStripButton>() { btnSeekDo };
            getdata.SetMode(Lbtn, true);

            getdata.DataInit(fno, tBox, ftypM, flenB);   //類別方法
            ReadOnlyFalse();        //設定 ReadOnly=false;使其可編修，僅tBox[x]，datagridView1,2 仍然 ReadOnly=true;。

        }

        //執行查詢
        private void btnSeekDo_Click(object sender, EventArgs e)
        {
            if (ae == "S")        //使用filter
            {
                try
                {
                    int i;
                    string str, ss;
                    str = fsort + " is not null and ";                   // "SELECT * FROM " + dbfm + " where 2<>1 ";
                    for (i = 0; i <= tbm.Columns.Count - 1; i++)
                    {
                        ss = tBox[i].Text.Trim();

                        // 1:String,  2:Boolean, 3:DateTime, 4:整數, 5:小數
                        if (ftypM[i] == 1)
                        {
                            str += tbm.Columns[i].ToString() + " Like '%" + ss + "%' and ";   //文字
                        }
                        else if (ftypM[i] == 2 && ss != "")
                        {
                            if (ss.Substring(0, 1) == "T" || ss == "1")
                                ss = "1";
                            else
                                ss = "0";
                            str += tbm.Columns[i].ToString() + " = " + ss + " and ";
                        }
                        if ((ftypM[i] == 3) && (ss != ""))                                             //日期
                        {
                            str += " and " + tbm.Columns[i].ToString() + " = '" + ss + "'";
                            //str += " CONVERT(varchar(12) ," + tbm.Columns[i].ToString() + ", 111 ) " + " = '" + ss + "' and ";   //日期
                        }
                        else if ((ftypM[i] == 4 || ftypM[i] == 5) && (ss != "0") && (ss != "0.0"))
                        {
                            str += tbm.Columns[i].ToString() + " = " + ss + " and "; ;
                        }

                    }
                    int l = str.Length;
                    str = str.Substring(0, l - 5);
                    bdsM.Filter = str;

                    BindingData();     //如使用BindingData();，會另外new 一個bdsM = new BindingSource(ds, dbfm);，改使用下段 
                    /*//**修改BindingData()，="S"會不再new BindingSource，因此下方可以免
                    dgvM.DataSource = bdsM;
                    dgvD.DataSource = bdsD;           //改使用DataView dvD,在bdsM_PositionChanged
                    //設定TextBox控制項的資料繫結
                    for (i = 0; i <= tbm.Columns.Count - 1; i++)
                        tBox[i].DataBindings.Add("Text", bdsM, tbm.Columns[i].ColumnName);
                    */
                }
                catch (System.Exception er)
                {
                    MessageBox.Show(er.ToString());
                }
                finally
                {
                    btnSeekDo.Enabled = false;

                    Lbtn.Clear();
                    Lbtn = new List<ToolStripButton>() { btnTop, btnPrior, btnNext, btnLast, btnRefresh };
                    getdata.SetMode(Lbtn, true);

                    dgv.dgv_Align(dgvM, tbm, ftypM);    //文字靠左、數字靠右
                    dgv.dgv_Align(dgvD, tbd, ftypD);
                    bdsM_PositionChanged(sender, e);    //該事件內不處理dgv.dgv_Align
                }
                ReadOnlyTrue();             //回復 ReadOnly=true;
            }

        }

        //重置
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            string mfieldname = tbm.Columns[0].ColumnName;     //主索引欄位
            string fText = tBox[0].Text;

            //解除 filter
            bdsM.Filter = "";       //改用DataView後,僅解除 filter，不必重新作關聯、設資料繫結

            int po1 = bdsM.Find(mfieldname, fText);     //查詢一筆資料的(int)位置(該欄位不需要排序，但必須需完全穩合，可不必唯一)。
            bdsM.Position = po1;
            //dgvM.CurrentCell = dgvM.Rows[po1].Cells[0];   //本式同上式
            bdsM_PositionChanged(sender, e);
            //上述作法可以使用先查詢特定筆數，重置後指標停於原目標位置，再作編修。

            Lbtn.Clear();
            Lbtn = new List<ToolStripButton>() { btnAdd, btnDel, btnEdit, btnUpdate, btnSeek };
            getdata.SetMode(Lbtn, true);

            btnRefresh.Enabled = false;
            btnSeekDo.Enabled = false;

            ae = "";
            ReadOnlyTrue();         //回復 ReadOnly=true;
        }

        #endregion

        #region Form 事件

        private void FDouble_Load(object sender, EventArgs e)
        {
            cnstr = Main.cnstr;
            con = new SqlConnection(cnstr);
            LbTbName.Text = LbTable;            //LbTable由FormMain指派

            dgv = new DgvSet();          //DataGridView 屬性設定 (ClsaaSet)
            getdata = new GetData();    //Class 本GetData類別皆改成不必去控制其他Form的非靜態方法,元件..
            getdata.cnstr = cnstr;

            Lbtn.Clear();
            ToolStripButton[] toolary = { btnTop, btnPrior, btnNext, btnLast, btnAdd, btnDel, btnEdit,
           btnUpdate, btnCancel, btnSeek, btnSeekDo, btnRefresh };
            Lbtn.AddRange(toolary);
            getdata.SetMode(Lbtn, false);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            FreetBox_LbArray();                                 //陣列釋放
            Lbtn.Clear();
            Lbtn = new List<ToolStripButton>() { btnAdd, btnDel, btnEdit, btnUpdate, btnCancel, btnSeek, btnSeekDo, btnRefresh };
            getdata.SetMode(Lbtn, false);

            if (Lbtn != null)
                for (int i = 0; i <= Lbtn.Count - 1; i++)
                { Lbtn[i] = null; }

            if (bdsM != null)
            {
                bdsM = null;
                ds = null;
                ///ds1 = null;
                dgvM.DataSource = null;
                dgvD.DataSource = null;
            }
            label2.Text = "  ";
            label3.Text = "記錄數";
        }


        private void FDouble_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnClose_Click(sender, e);
        }

        private void FDouble_SizeChanged(object sender, EventArgs e)
        {
            dgvD.Width = this.Width - (990 - 835);          //dgvM暫不需要調
        }

        private void FDouble_KeyDown(object sender, KeyEventArgs e)
        {
            // 本事件可正常運作，但需配合this.KeyPreview = true; 
            //MessageBox.Show("e.KeyCode =" + e.KeyCode.ToString()+ "; e.Modifiers ="+ Keys.Control.ToString()+"; Emode="+ dgvD.IsCurrentCellInEditMode);
            /*
            if (ae == "E" && e.KeyCode == Keys.Delete)
            {
                if (MessageBox.Show("確定刪除本筆？", "Confirmation", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;

                //明細檔 DataRow 刪除作法
                string mvhno, mfgno, txt;
                int rno = dgvD.CurrentRow.Index;
                mvhno = dgvD.Rows[rno].Cells[0].Value.ToString();
                mfgno = dgvD.Rows[rno].Cells[2].Value.ToString();
                MessageBox.Show(dgvD.Columns[0].Name + "; " + dgvD.Columns[2].Name);
                txt = dgvD.Columns[0].Name + "='" + mvhno + "' and " + dgvD.Columns[2].Name + "='" + mfgno + "'";
                //MessageBox.Show(txt);
                DataRow[] Rows = tbd.Select(txt);       //雖然是DataRow陣列，但只會有一筆(單據編號+商品型號)
                foreach (DataRow row in Rows)
                { row.Delete(); }
            }
            */
        }

        #endregion

        #region DataGridView 事件

        private void dgvM_Leave(object sender, EventArgs e)
        {
            //if (ae == "A" && MasterEnd == false)
            //{
            //    MasterEnd = true;       //新增主檔結束(擬到明細檔)
            //}
        }

        private void dgvD_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //本事件可正常使用 (免用this.KeyPreview = true;
            //if ((ae == "E" && e.KeyCode == Keys.Delete && e.Modifiers == Keys.Control) && (dgvD.IsCurrentCellInEditMode))
            if (ae == "E" && e.KeyCode == Keys.Delete)
            {
                if (MessageBox.Show("確定刪除本筆？", "Confirmation", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;
                bdsD.RemoveCurrent();       //方法1 使用BindingSource的RemoveCurrent

                /*
                //明細檔 DataRow 刪除作法
                string mvhno, mfgno;
                int rno = dgvD.CurrentRow.Index;
                mvhno = dgvD.Rows[rno].Cells[0].Value.ToString();
                mfgno = dgvD.Rows[rno].Cells[2].Value.ToString();
                //MessageBox.Show(dgvD.Columns[0].Name + "; " + dgvD.Columns[2].Name);
                */
                /* //方法2
                string txt;
                txt = dgvD.Columns[0].Name + "='" + mvhno + "' and " + dgvD.Columns[2].Name + "='" + mfgno + "'";
                DataRow[] Rows = tbd.Select(txt);       //雖然是DataRow陣列，但只會有一筆(單據編號+商品型號)
                foreach (DataRow row in Rows)
                { row.Delete(); }
                */
                /*
                //方法3
                foreach (DataRowView dr in bdsD)                            //此處使用 DataRowView
                {
                    if (dr.Row[0].ToString() == mvhno && dr.Row[2].ToString() == mfgno)
                        dr.Delete();
                }
                */
            }
        }

        private void dgvD_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show("Row=" + e.RowIndex.ToString()+"; col="+ e.ColumnIndex.ToString());
            if (e.ColumnIndex == 2)
            {
                int po = dgvD.CurrentRow.Index;   //取當下的Row，同e.RowIndex.ToString()
                string mfgno = dgvD.Rows[po].Cells[2].Value.ToString();  //取 cells(2,po)的值
                int co = tbd.Columns.Count;


                //DataRow dr = ((DataRowView)bdsP.Current).Row; //bdsP..GetCurrentDataRow();
                dgvD.Rows[po].Cells[co - 1].Value = getdata.GetFgname(mfgno, tbfg);   //((DataRowView)bdsP.Current).Row["FGNAME"];         
                //OD序號改在 dgvD_CellDoubleClick()

            }
        }

        private void dgvD_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (ae == "A" || ae == "E")
            {
                if (e.ColumnIndex == 2)     //Fgno 欄位
                {
                    //新增、編修填入明細檔的序號 OD；，刪除暫以手動修改。
                    dgvD.Rows[e.RowIndex].Cells[1].Value = e.RowIndex + 1;

                    FormFg frm2 = new FormFg(this);
                    if (frm2.ShowDialog() == DialogResult.OK)
                    {
                        dgvD.Rows[e.RowIndex].Cells[2].Value = MFGNO;      //MFGNO由form2寫入值，再填入商品型號
                        dgvD.CurrentCell = dgvD.Rows[e.RowIndex].Cells[3];  //移動指標到下一欄，資料才顯現到商品編號欄位內

                        //###增下2判斷避免因沒有輸入單價、數量而成二個相乘之異常(金額)
                        if (dgvD.Rows[e.RowIndex].Cells[3].Value.ToString() == "")
                            dgvD.Rows[e.RowIndex].Cells[3].Value = 1;       //###數量預設1
                        if (dgvD.Rows[e.RowIndex].Cells[4].Value.ToString() == "")
                            dgvD.Rows[e.RowIndex].Cells[4].Value = 1;       //###單價預設1
                    }
                }
            }
        }

        private void dgvD_Click(object sender, EventArgs e)
        {

        }

        #endregion

    }
}
