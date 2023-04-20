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
    public partial class Product : Form
    {
        public Product()
        {
            InitializeComponent();
        }
        #region 變數
        DgvSet dgvset;
        SqlConnection cn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        string ss;          //ss=sql string
        int n;  //record number 紀錄編號

        #endregion

        #region 自訂方法
        void ShowData()
        {
            DataSet ds = new DataSet();
            //DataAdapter 依照ss的指示, 把資料撈進去dataset 的 dept資料表
            //一個DataAdapter, 對應一個資料表 
            SqlDataAdapter daDept = new SqlDataAdapter(ss, cn);    //da 會自動開關資料庫, 不需程式控制
            daDept.Fill(ds, "YFGMAST");

            // 在dataGridView1內顯示dataset 的 dept資料表  
            dataGridView1.DataSource = ds.Tables["YFGMAST"];

            SqlCommand cmdCount;
            //取員工資料筆數           
            cmdCount = new SqlCommand("SELECT COUNT(*) FROM YFGMAST", cn);
            cn.Open();   //command 需要程式控制資料庫的開關
            //lblCount.Text = "資料表共 " + cmdCount.ExecuteScalar().ToString() + " 筆記錄";
            cn.Close();
        }
        #endregion
        private void Form7_Load(object sender, EventArgs e)
        {
            cn.ConnectionString = "Data Source=.\\sql2019;Initial Catalog=YVMENUC;Integrated Security=True";
            cmd.Connection = cn;
            ss = "SELECT * FROM YFGMAST";    //指示ShowData()的內容
            ShowData();

            dgvset = new DgvSet();
            dgvset.dgvSet(dataGridView1);

            dataGridView1.Columns[0].HeaderText = "商品編號";     //改中文欄位名稱
            dataGridView1.Columns[1].HeaderText = "商品名稱";
            dataGridView1.Columns[2].HeaderText = "公司編號";
            dataGridView1.Columns[3].HeaderText = "參考售價";
            dataGridView1.Columns[4].HeaderText = "商品重量";
            dataGridView1.Columns[5].HeaderText = "備註";
            dataGridView1.Columns[6].HeaderText = "即時庫存";
            dataGridView1.Columns[7].HeaderText = "最近交易";
            dataGridView1.Columns[8].HeaderText = "圖片檔名";
            dataGridView1.Columns[9].HeaderText = "自有商品";

            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            n = (int)e.RowIndex;
            textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            textBox5.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            textBox6.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            textBox7.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
            textBox8.Text = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
            textBox9.Text = dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();
            textBox10.Text = dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString();
        }

        #region CRUD
        private void button1_Click(object sender, EventArgs e)
        {
            //新增
            try  //使用try...catch...敘述來補捉異動資料可能發生的例外 
            {
                //終版
                //ss = $"INSERT INTO YFGMAST " +
                //$"VALUES ({textBox1.Text.Trim()},{textBox2.Text.Trim()},{textBox3.Text.Trim()},{textBox4.Text.Trim()},{textBox5.Text.Trim()},{textBox6.Text.Trim()},{textBox7.Text.Trim()},@Cdate,{textBox9.Text.Trim()},{ textBox10.Text.Trim()})";
                //cmd.Parameters.Add(new SqlParameter("@Cdate", SqlDbType.Date));
                //cmd.Parameters["@Cdate"].Value = DateTime.Parse(textBox8.Text.Trim());
                //cmd.CommandText = ss;

                //終版2
                ss = "INSERT INTO YFGMAST(FGNO, FGNAME, CONO,PRC,ZWET,NOTE,IQTY,Cdate,PICTURE,YN) " +
                    "VALUES (@FGNO, @FGNAME, @CONO,@PRC,@ZWET,@NOTE,@IQTY,@Cdate,@PICTURE,@YN)";

                cmd.CommandText = ss;

                cmd.Parameters.Add(new SqlParameter("@FGNO", SqlDbType.NVarChar));
                cmd.Parameters["@FGNO"].Value = textBox1.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@FGNAME", SqlDbType.NVarChar));
                cmd.Parameters["@FGNAME"].Value = textBox2.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@CONO", SqlDbType.NVarChar));
                cmd.Parameters["@CONO"].Value = textBox3.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@PRC", SqlDbType.Int));    //int.Parse
                cmd.Parameters["@PRC"].Value = int.Parse(textBox4.Text.Trim());

                cmd.Parameters.Add(new SqlParameter("@ZWET", SqlDbType.Float));
                cmd.Parameters["@ZWET"].Value = float.Parse(textBox5.Text.Trim());

                cmd.Parameters.Add(new SqlParameter("@NOTE", SqlDbType.NVarChar));
                cmd.Parameters["@NOTE"].Value = textBox6.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@IQTY", SqlDbType.Int));
                cmd.Parameters["@IQTY"].Value = int.Parse(textBox7.Text.Trim());

                cmd.Parameters.Add(new SqlParameter("@Cdate", SqlDbType.Date));
                cmd.Parameters["@Cdate"].Value = DateTime.Parse(textBox8.Text.Trim()).ToString("yyyy/MM/dd");

                cmd.Parameters.Add(new SqlParameter("@PICTURE", SqlDbType.NVarChar));
                cmd.Parameters["@PICTURE"].Value = textBox9.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@YN", SqlDbType.Bit));
                cmd.Parameters["@YN"].Value = int.Parse(textBox10.Text.Trim());

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

                ss = "SELECT * FROM YFGMAST"; //指示ShowData()的內容
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
                ss = "UPDATE YFGMAST  SET  FGNO = @FGNO, FGNAME = @FGNAME,CONO = @CONO,PRC =@PRC ,ZWET = @ZWET," +
                         "NOTE = @NOTE,IQTY = @IQTY,Cdate = @Cdate,PICTURE = @PICTURE,YN = @YN WHERE FGNO = @FGNO";
                cmd.CommandText = ss;

                cmd.Parameters.Add(new SqlParameter("@FGNO", SqlDbType.NVarChar));
                cmd.Parameters["@FGNO"].Value = textBox1.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@FGNAME", SqlDbType.NVarChar));
                cmd.Parameters["@FGNAME"].Value = textBox2.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@CONO", SqlDbType.NVarChar));
                cmd.Parameters["@CONO"].Value = textBox3.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@PRC", SqlDbType.Int));
                cmd.Parameters["@PRC"].Value = int.Parse(textBox4.Text.Trim());

                cmd.Parameters.Add(new SqlParameter("@ZWET", SqlDbType.Float));
                cmd.Parameters["@ZWET"].Value = float.Parse(textBox5.Text.Trim());

                cmd.Parameters.Add(new SqlParameter("@NOTE", SqlDbType.NVarChar));
                cmd.Parameters["@NOTE"].Value = textBox6.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@IQTY", SqlDbType.Int));
                cmd.Parameters["@IQTY"].Value = int.Parse(textBox7.Text.Trim());

                cmd.Parameters.Add(new SqlParameter("@Cdate", SqlDbType.Date));
                cmd.Parameters["@Cdate"].Value = DateTime.Parse(textBox8.Text.Trim()).ToString("yyyy/MM/dd");

                cmd.Parameters.Add(new SqlParameter("@PICTURE", SqlDbType.NVarChar));
                cmd.Parameters["@PICTURE"].Value = textBox9.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@YN", SqlDbType.Bit));
                cmd.Parameters["@YN"].Value = Convert.ToInt32(textBox10.Text.Trim());
                
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

                ss = "SELECT * FROM YFGMAST";   //指示ShowData()的內容
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
        {//刪除
            try	//使用try...catch...敘述來補捉異動資料可能發生的例外
            {
                ss = "DELETE FROM YFGMAST WHERE FGNO = '"+textBox1.Text+"'";

                cmd.CommandText = ss;
                #region 原式
                /*
                MessageBox.Show(ss);
                cmd.Parameters.Add(new SqlParameter("@FGNO", SqlDbType.NVarChar));
                cmd.Parameters["@FGNO"].Value = textBox1.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@FGNAME", SqlDbType.NVarChar));
                cmd.Parameters["@FGNAME"].Value = textBox2.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@CONO", SqlDbType.NVarChar));
                cmd.Parameters["@CONO"].Value = textBox3.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@PRC", SqlDbType.Real));
                cmd.Parameters["@PRC"].Value = Convert.ToSingle(textBox4.Text.Trim());

                cmd.Parameters.Add(new SqlParameter("@ZWET", SqlDbType.Float));
                cmd.Parameters["@ZWET"].Value = Convert.ToDouble(textBox5.Text.Trim());

                cmd.Parameters.Add(new SqlParameter("@NOTE", SqlDbType.NVarChar));
                cmd.Parameters["@NOTE"].Value = textBox6.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@IQTY", SqlDbType.Int));
                cmd.Parameters["@IQTY"].Value = Convert.ToInt32(textBox7.Text.Trim());

                cmd.Parameters.Add(new SqlParameter("@Cdate", SqlDbType.Date));
                cmd.Parameters["@Cdate"].Value = DateTime.Parse(textBox8.Text.Trim());

                cmd.Parameters.Add(new SqlParameter("@PICTURE", SqlDbType.NVarChar));
                cmd.Parameters["@PICTURE"].Value = textBox9.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@YN", SqlDbType.Bit));
                cmd.Parameters["@YN"].Value = Convert.ToInt32(textBox10.Text.Trim());

                cmd.CommandText = ss;
                MessageBox.Show(ss);
                */
                #endregion
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
                
                ss = "SELECT * FROM YFGMAST";  //指示ShowData()的內容
              
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
                ss = "SELECT * FROM YFGMAST WHERE ";
                if (textBox1.Text.Trim() != "")
                {
                    ss += " FGNO LIKE '%" + textBox1.Text.Trim() + "%'   ";
                }
                else
                {
                    ss += " 1=1 ";
                }

                if (textBox2.Text.Trim() != "")
                {
                    ss += " AND  FGNAME LIKE '%" + textBox2.Text.Trim() + "%'   ";
                }
                else
                {
                    ss += " AND 1=1 ";
                }

                if (textBox3.Text.Trim() != "")
                {
                    ss += " AND  CONO LIKE '%" + textBox3.Text.Trim() + "%'   ";
                }
                else
                {
                    ss += " AND 1=1 ";
                }
                //MessageBox.Show(ss);
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
            textBox1.Text = ""; textBox2.Text = ""; textBox3.Text = ""; textBox4.Text = ""; textBox5.Text = ""; textBox6.Text = "";
            textBox7.Text = ""; textBox8.Text = ""; textBox9.Text = ""; textBox10.Text = ""; 
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //確定離開？
            if (MessageBox.Show("確定返回主頁面？", "Confirm Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {   
                this.Dispose();
            }
        }
        #endregion


    }
}
