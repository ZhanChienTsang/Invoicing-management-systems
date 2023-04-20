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
    public partial class Inventory : Form
    {
        public Inventory()
        {
            InitializeComponent();
        }
        #region 變數
        DgvSet dgvset;
        SqlConnection cn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        string ss;          //ss=sql string
        int n;  //record number 紀錄編號
        string documentContents;   //列印用
        string stringToPrint;              //列印用

        #endregion
        #region 自訂方法
        void ShowData()
        {
            DataSet ds = new DataSet();
            //DataAdapter 依照ss的指示, 把資料撈進去dataset 的 dept資料表
            //一個DataAdapter, 對應一個資料表 
            SqlDataAdapter daDept = new SqlDataAdapter(ss, cn);    //da 會自動開關資料庫, 不需程式控制
            daDept.Fill(ds, "YINVENTORY");

            // 在dataGridView1內顯示dataset 的 dept資料表  
            dataGridView1.DataSource = ds.Tables["YINVENTORY"];

            SqlCommand cmdCount;
            //取員工資料筆數           
            cmdCount = new SqlCommand("SELECT COUNT(*) FROM YINVENTORY", cn);
            cn.Open();   //command 需要程式控制資料庫的開關
            cn.Close();
        }
        #endregion

        private void Inventory_Load(object sender, EventArgs e)
        {
            cn.ConnectionString = "Data Source=.\\sql2019;Initial Catalog=YVMENUC;Integrated Security=True";
            cmd.Connection = cn;
            ss = "SELECT * FROM YINVENTORY";    //指示ShowData()的內容
            ShowData();
            //GetColumnsDescrip();  //取得中文欄位名稱
            dgvset = new DgvSet();
            dgvset.dgvSet(dataGridView1);
            dataGridView1.AutoResizeColumns();

            //DGV1中文欄位名稱設定
            dataGridView1.Columns[0].HeaderText = "資料年月";     //改中文欄位名稱
            dataGridView1.Columns[1].HeaderText = "成品編號";
            dataGridView1.Columns[2].HeaderText = "庫存數量";
            dataGridView1.Columns[3].HeaderText = "系統數量";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            n = (int)e.RowIndex;
            textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //新增
            try  //使用try...catch...敘述來補捉異動資料可能發生的例外 
            {
                ss = "INSERT INTO YINVENTORY(YYMM, FGNO, IVQTY,SQTY) VALUES (@YYMM, @FGNO, @IVQTY,@SQTY )";

                cmd.CommandText = ss;

                cmd.Parameters.Add(new SqlParameter("@YYMM", SqlDbType.SmallInt));
                cmd.Parameters["@YYMM"].Value = int.Parse(textBox1.Text.Trim());

                cmd.Parameters.Add(new SqlParameter("@FGNO", SqlDbType.NVarChar));
                cmd.Parameters["@FGNO"].Value = textBox2.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@IVQTY", SqlDbType.Int));
                cmd.Parameters["@IVQTY"].Value = int.Parse(textBox3.Text.Trim());

                cmd.Parameters.Add(new SqlParameter("@SQTY", SqlDbType.Int));
                cmd.Parameters["@SQTY"].Value = int.Parse(textBox4.Text.Trim());

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

                ss = "SELECT * FROM YINVENTORY"; //指示ShowData()的內容
                ShowData();
                MessageBox.Show("新增資料成功");
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message + ", 新增資料發生錯誤");
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //更新
            try	//使用try...catch...敘述來補捉異動資料可能發生的例外
            {
                ss = "UPDATE YINVENTORY  SET  YYMM = @YYMM, FGNO = @FGNO,IVQTY = @IVQTY,SQTY =@SQTY WHERE FGNO = @FGNO";
                cmd.CommandText = ss;

                cmd.Parameters.Add(new SqlParameter("@YYMM", SqlDbType.SmallInt));
                cmd.Parameters["@YYMM"].Value = int.Parse(textBox1.Text.Trim());

                cmd.Parameters.Add(new SqlParameter("@FGNO", SqlDbType.NVarChar));
                cmd.Parameters["@FGNO"].Value = textBox2.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@IVQTY", SqlDbType.Int));
                cmd.Parameters["@IVQTY"].Value = int.Parse(textBox3.Text.Trim());

                cmd.Parameters.Add(new SqlParameter("@SQTY", SqlDbType.Int));
                cmd.Parameters["@SQTY"].Value = int.Parse(textBox4.Text.Trim());

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

                ss = "SELECT * FROM YINVENTORY";   //指示ShowData()的內容
                ShowData();
                MessageBox.Show("更新資料成功");
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message + ", 修改資料發生錯誤");
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //刪除
            try	//使用try...catch...敘述來補捉異動資料可能發生的例外
            {
                ss = "DELETE FROM YINVENTORY WHERE YYMM = '" + textBox1.Text + "'";
                cmd.CommandText = ss;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

                ss = "SELECT * FROM YINVENTORY";  //指示ShowData()的內容
                ShowData();
                MessageBox.Show("刪除資料成功");
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message + ", 刪除資料發生錯誤");
            }
            finally
            {
                cmd.Parameters.Clear();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //查詢
            try //使用try...catch...敘述來補捉異動資料可能發生的例外
            {
                ss = "SELECT * FROM YINVENTORY WHERE ";
                if (textBox1.Text.Trim() != "")
                {
                    ss += " YYMM LIKE '%" + textBox1.Text.Trim() + "%'   ";
                }
                else
                {
                    ss += " 1=1 ";
                }

                if (textBox2.Text.Trim() != "")
                {
                    ss += " AND  FGNO LIKE '%" + textBox2.Text.Trim() + "%'   ";
                }
                else
                {
                    ss += " AND 1=1 ";
                }
                ShowData();   //指示ShowData()的內容

            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message + ", 資料發生錯誤");
            }
            finally
            {
                ;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //清除tbx內容
            textBox1.Text = ""; textBox2.Text = ""; textBox3.Text = ""; textBox4.Text = "";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //確定離開？
            if (MessageBox.Show("確定返回主頁面？", "Confirm Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                this.Dispose();
            }
        }
        Bitmap bmp;
        private void button7_Click(object sender, EventArgs e)
        {
             //列印
            //printDocument1.DocumentName = richTextBox1.Text;
            //documentContents = richTextBox1.Text;
            //stringToPrint = richTextBox1.Text;

            //printPreviewDialog1.Document = printDocument1;
            //printPreviewDialog1.ShowDialog();

            int height = dataGridView1.Height;
            dataGridView1.Height = dataGridView1.RowCount * dataGridView1.RowTemplate.Height * 5;
            bmp = new Bitmap(dataGridView1.Width, dataGridView1.Height);
            dataGridView1.DrawToBitmap(bmp, new Rectangle(0, 0, dataGridView1.Width, dataGridView1.Height));
            printPreviewDialog1.PrintPreviewControl.Zoom = 1;
            printPreviewDialog1.ShowDialog();
            dataGridView1.Height = height;
            


        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(bmp, 0, 0);
            ////列印設定
            //int charactersOnPage = 0;
            //int linesPerPage = 0;
            //// Sets the value of charactersOnPage to the number of characters 
            //// of stringToPrint that will fit within the bounds of the page.
            //e.Graphics.MeasureString(stringToPrint, this.Font,
            //    e.MarginBounds.Size, StringFormat.GenericTypographic,
            //    out charactersOnPage, out linesPerPage);

            //// Draws the string within the bounds of the page.
            //e.Graphics.DrawString(stringToPrint, this.Font, Brushes.Black,
            //    e.MarginBounds, StringFormat.GenericTypographic);

            //// Remove the portion of the string that has been printed.
            //stringToPrint = stringToPrint.Substring(charactersOnPage);

            //// Check to see if more pages are to be printed.
            //e.HasMorePages = (stringToPrint.Length > 0);

            //// If there are no more pages, reset the string to be printed.
            //if (!e.HasMorePages)
            //    stringToPrint = documentContents;
        }


    }
}
