using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing;
using System.Collections.Generic;   //引用 List<T> 

namespace ChienTsang
{

    class GetData
    {

        public string cnstr;

        public GetData() { }     //控制各個Form (FormMain,FDouble...) 中靜態物件(變數,元件...)需此宣告，建立時一般類別建立方式 GetData dt=new GetData() 

        public string GetPrimekey(string dbfm)        //本方法用於 MSSQL
        {
            string sql2, fsort;
            sql2 = "Select table_name,column_name  FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE  where  table_name = '" + dbfm + "'";      //取得主索引

            DataTable tb = GetTableData(sql2);
            if (tb.Rows.Count == 0)
                fsort = "沒有資料";
            else
            {
                if (tb.Rows.Count == 1)
                    fsort = tb.Rows[0][1].ToString();
                else   //本專案目前Primekey最多二個攔位構成
                    fsort = tb.Rows[1][1].ToString() + "," + tb.Rows[0][1].ToString();
            }
            return fsort;
        }

        public int[] CreateFtypeArray(DataTable tb)         //參數為 Table, 回傳 int 陣列 ftype[]             
        {
            //取Master、Detail 檔資料型態當 ftypM[]
            string fieldname;

            int[] ftype = new int[tb.Columns.Count];
            for (int i = 0; i <= tb.Columns.Count - 1; i++)           //取資料型態當 ftyp[]
            {
                fieldname = tb.Columns[i].DataType.ToString();
                ftype[i] = fieldtype(fieldname);     // 1:String,  2:Boolean, 3:DateTime, 4:整數, 5:小數
                //MessageBox.Show("ftype["+i.ToString()+"]="+ ftype[i].ToString()+"; "+ fieldname);
            }
            return ftype;
        }


        private int fieldtype(string ftp)                   //回傳單一欄位型態代碼給 ftype[i]
        {
            Byte ftype;
            if (ftp.IndexOf("String") != -1)                //表找到 "String"
                ftype = 1;      //string
            else
                if (ftp.IndexOf("Boolean") != -1)           //表找到 "Bloolean"
                ftype = 2;      //Bloolean
            else
                if (ftp.IndexOf("DateTime") != -1)          //表找到 "DateTime"
                ftype = 3;      //DateTime
            else
                if ((ftp.IndexOf("Int") != -1) || (ftp.IndexOf("Byte") != -1) || (ftp.IndexOf("Short") != -1) || (ftp.IndexOf("Long") != -1))
                ftype = 4;                                  //整數
            else                                            //**如有其他型態會需要再調整其他型態
                ftype = 5;                                  //小數

            return ftype;
        }

        public int[] GetStrLength(int fno, string fsort, string dbfm, ref int idx)        //本方法用於 MSSQL
        {
            string sql1;
            int[] flen;
            sql1 = "SELECT ORDINAL_POSITION, " +
                         "COLUMN_NAME, " +
                         "DATA_TYPE, " +
                         "CHARACTER_MAXIMUM_LENGTH, " +
                         "IS_NULLABLE " +
                  "FROM INFORMATION_SCHEMA.COLUMNS " +
                  "WHERE TABLE_NAME = '" + dbfm + "'";

            DataTable tb = GetTableData(sql1);
            int i = 0;
            flen = new int[fno];                   //僅宣告設長度，暫不給值

            for (i = 0; i < tb.Rows.Count; i++)
            {
                if (tb.Rows[i][3].ToString().Trim() == "")
                    flen[i] = 0;
                else
                {
                    flen[i] = Int16.Parse(tb.Rows[i][3].ToString());
                    if (flen[i] < 4)               //寬度加大
                        flen[i] = (int)(flen[i] * 2);
                }
                if (fsort.IndexOf(",") >= 0) { }   //表多欄位組成的PrimeKey
                else
                {
                    if (tb.Rows[i][1].ToString() == fsort)
                    {
                        idx = i;                        //取得主索引的欄位序，使用 ref 
                    }
                }
                //idx雖是以主索引的欄位序，但主要用於主檔編修時使用tBox[x].ReadOnly=true，目前是雙檔作業之主檔(明細檔無使用idx)
                //以及單檔基本檔(廠客,成品,員工)+盤點，僅盤點檔屬多欄位的PromeKey，其idx原預設99,再由程式控制。
            }
            return flen;
        }


        public void DataInit(int fno, TextBox[] tBox, int[] ftypM, int[] flenB)    //查詢、新增時，提供空白UI。
        {
            int j;
            for (j = 0; j <= fno - 1; j++)
            {
                tBox[j].DataBindings.Clear();   //斷開 bds
                tBox[j].Text = "";              //清除原tBox[]內容

                if (ftypM[j] == 4 || ftypM[j] == 5)                   // 1:String,  2:Boolean, 3:DateTime, 4:整數, 5:小數
                    tBox[j].TextAlign = HorizontalAlignment.Right;
                switch (ftypM[j])
                {
                    case 1:
                        tBox[j].Width = Math.Min((flenB[j] * 12), 300);      //只取寬，無空白。　
                        break;
                    case 2:
                        tBox[j].Text = "False ";         //Boolean
                        tBox[j].Width = 6 * 8;
                        break;
                    case 3:
                        tBox[j].Text = "";               //DateTime
                        tBox[j].Width = 11 * 8;
                        break;
                    case 4:
                        tBox[j].Width = 10 * 8;           //數值型態
                        tBox[j].Text = "0";             //整數
                        break;
                    case 5:
                        tBox[j].Width = 10 * 8;           //數值型態
                        tBox[j].Text = "0.0";           //小數
                        break;
                }

            }
            tBox[2].Width = 80;
        }

        public bool Check_Blank(TextBox[] tBox, Label[] Lb)
        {
            string mfname;      //mc 回傳客戶編號是否 error，mfname 顯示欄位名稱用變數
            bool tf = true;
            for (int i = 0; i < Lb.Length - 2; i++)     //最末總金額不必check
            {
                //由於當點選tBox[2]時，會出現公司名稱點選，不點選ComBoBox不會消失，因此無法輸入，所以本段不需要Check 客戶編號是否建立的問題。
                if ((tBox[i].Text == "") || (tBox[i].Text == "0") || (tBox[i].Text == "0.0"))  //本方法僅作空白判斷，如Date型態時，再另作判斷Date格式問題!
                {
                    tf = false;
                    if (i != 2)
                        tBox[i].Select();
                    tBox[i].Focus();
                    mfname = Lb[i].Text;
                    MessageBox.Show(mfname + " 欄位不能空白！");
                    break;
                }
            }
            return tf;
        }

        public string Set_Vhno(string sd1, string dbfm)            //**依個案交易檔單據編號原則調整(單檔暫沒有使用)
        {
            string mvhno, sd2, sd3;

            Int32 vhno1;
            DateTime dt = DateTime.Now;
            vhno1 = (dt.Year - 1911) * 10000 + dt.Month * 100 + dt.Day;     //當天日期文字
            //int r = dvM.Count;
            if (dbfm == "YFGIO")                                            //**依個案交易檔名稱及其編號原則調整(單檔暫沒有使用)
            {
                sd2 = sd1.Substring(0, 7);          //sd2為原最大訂單編號之日期許文字型態
                if (sd2 == vhno1.ToString())        //
                {
                    mvhno = Convert.ToString(Convert.ToInt32(sd1) + 1);
                }
                else
                {
                    mvhno = vhno1 + "01";
                }
            }
            else
            {   //(dbfm == "YODR")                                        //**依個案交易檔名稱及其編號原則調整(單檔暫沒有使用)                                            
                sd2 = sd1.Substring(1, 7);                                  //sd2為原最大訂單編號之日期許文字型態
                if (sd2 == vhno1.ToString())
                {
                    sd3 = sd1.Substring(1, 9);
                    mvhno = "Y" + Convert.ToString(Convert.ToInt32(sd3) + 1);
                    //mvhno = Convert.ToString(Convert.ToInt32(sd2) + 1);
                }
                else
                {
                    mvhno = "Y" + vhno1 + "01";
                }
            }
            return mvhno;
        }

        public string GetFgname(string mfgno, DataTable tbfg)     //以商品型號 .Select() 取得商品名稱
        {
            DataRow[] drs;
            string mfgname;
            string st = "fgno ='" + mfgno + "'";                            //**依個案調整或採多二個參數 fgno,fgname
            drs = tbfg.Select(st);
            try
            {
                mfgname = drs[0]["FGNAME"].ToString();                      //**依個案調整或採多二個參數 fgno,fgname
            }
            catch
            {
                mfgname = "Error";
            }
            return mfgname;
        }

        public string GetCuname(string mcono, DataTable tbcu)      //以公司型號 .Select() 取得公司名稱
        {
            DataRow[] drs;
            string mcuname;
            string st = "cono ='" + mcono + "'";
            drs = tbcu.Select(st);
            try
            {
                mcuname = drs[0]["NAME"].ToString();                        //**依個案調整或採多二個參數 cono, Name(公司編號、公司名稱之欄位名稱
            }
            catch
            {
                mcuname = "Error";
            }
            return mcuname;
        }

        public string GetNo_Name(string mno, DataView dtv, string Field_name) //以編號取得名稱(3個參數) 
        {                         //編號        DataView     回傳名稱的欄位名
            string mname;               //回傳名稱
            int x = dtv.Find(mno);      //DataView.Find() 查詢的欄位要先排序(field_no)
            if (x > -1)
            {
                DataRowView drv = dtv[x];
                mname = drv[Field_name] + "";
            }
            else
                mname = "";
            return mname;
        }

        public DataTable GetTableData(string sql)          //本Class 中 GetPrimekey()、GetStrLength()使用
        {
            //MessageBox.Show("GetData1=");
            //MessageBox.Show("GetData2=" + cnstr);

            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = cnstr;
            DataTable tb = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand(sql, cn);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                tb.Load(dr);                    //載入SqlDataReader的資料
                //MessageBox.Show("recno1=" + tb.Rows.Count);
                dr.Close();                     //關閉SqlDataReader
                cmd.Dispose();                  //釋放SqlCommand所占用的資源
                if (cn.State == ConnectionState.Open)
                    cn.Close();
                return tb;
            }
            catch (System.Exception er)
            {
                MessageBox.Show(er.ToString());
            }
            return tb;
        }
        public List<string> GetColumnsDescrip(string tbn)       //取得描述的中文欄名
        {
            //string desc = "SELECT value FROM::fn_listextendedproperty(NULL, 'user', 'dbo', 'table', '" + tbn + "','column','" + cln + "')";
            string desc = "SELECT  VALUE FROM fn_listextendedproperty(NULL, 'schema', 'dbo', 'table', '" + tbn + "', 'column', DEFAULT)";
            SqlConnection cn = new SqlConnection(cnstr);
            cn.Open();
            SqlCommand cmd = new SqlCommand(desc, cn);
            SqlDataReader dr;
            DataTable tb = new DataTable();
            dr = cmd.ExecuteReader();
            tb.Load(dr);
            cn.Close();
            List<string> list = new List<string>();
            for (int i = 0; i < tb.Rows.Count; i++)
                list.Add(tb.Rows[i][0].ToString());
            tb.Dispose();
            cmd.Dispose();
            return list;
        }


        public void GetDataRow(ref DataRow dr, TextBox[] tBox, int[] ftype) //單檔新增、修改；雙檔的(tBoxN_Enter)
        {
            for (int j = 0; j < tBox.Length; j++)
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

        public void SetMode(List<ToolStripButton> mybtn, bool tf)
        {
            for (int i = 0; i <= mybtn.Count - 1; i++)
            {
                mybtn[i].Enabled = Convert.ToBoolean(tf);
            }
        }

        public static DialogResult InputBox(string title, string promptText, ref string value)      //自訂 InputBox()
        {
            Font font = new Font("Arial", 12);
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 15, 372, 13);        //(int x, int y, int width, int height)
            textBox.SetBounds(12, 46, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 28);
            buttonCancel.SetBounds(309, 72, 75, 28);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 117);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height + 20);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;
            form.BackColor = Color.FromArgb(221, 242, 255);

            textBox.Font = font;
            buttonOk.Font = font;
            buttonCancel.Font = font;
            form.Font = font;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }


    }
}
