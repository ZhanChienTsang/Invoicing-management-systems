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
    public partial class Customer : Form
    {
        public Customer()
        {
            InitializeComponent();
        }
        #region 變數
        DgvSet dgvset;
        SqlConnection cn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        string ss;          //ss=sql string
        int n;  //record number 紀錄編號

        //List<string> DescList = new List<string>();     //中文欄名
        #endregion

        #region 自訂方法
        void ShowData()
        {
            DataSet ds = new DataSet();
            //DataAdapter 依照ss的指示, 把資料撈進去dataset 的 dept資料表
            //一個DataAdapter, 對應一個資料表 
            SqlDataAdapter daDept = new SqlDataAdapter(ss, cn);    //da 會自動開關資料庫, 不需程式控制
            daDept.Fill(ds, "YCUST");

            // 在dataGridView1內顯示dataset 的 dept資料表  
            dataGridView1.DataSource = ds.Tables["YCUST"];

            SqlCommand cmdCount;
            //取員工資料筆數           
            cmdCount = new SqlCommand("SELECT COUNT(*) FROM YCUST", cn);
            cn.Open();   //command 需要程式控制資料庫的開關
            //lblCount.Text = "資料表共 " + cmdCount.ExecuteScalar().ToString() + " 筆記錄";
            cn.Close();
        }
        #endregion

        private void Form5_Load(object sender, EventArgs e)
        {
            cn.ConnectionString = "Data Source=.\\sql2019;Initial Catalog=YVMENUC;Integrated Security=True";
            cmd.Connection = cn;
            ss = "SELECT * FROM YCUST";    //指示ShowData()的內容
            ShowData();
            //GetColumnsDescrip();  //取得中文欄位名稱
            dgvset = new DgvSet();
            dgvset.dgvSet(dataGridView1);

            //DGV1中文欄位名稱設定
            dataGridView1.Columns[0].HeaderText = "公司編號";     //改中文欄位名稱
            dataGridView1.Columns[1].HeaderText = "公司簡稱";
            dataGridView1.Columns[2].HeaderText = "公司名稱";
            dataGridView1.Columns[3].HeaderText = "公司統編";
            dataGridView1.Columns[4].HeaderText = "負責人";
            dataGridView1.Columns[5].HeaderText = "聯絡人";
            dataGridView1.Columns[6].HeaderText = "電話一";
            dataGridView1.Columns[7].HeaderText = "電話二";
            dataGridView1.Columns[8].HeaderText = "行動電話";
            dataGridView1.Columns[9].HeaderText = "傳真機";
            dataGridView1.Columns[10].HeaderText = "公司地址";
            dataGridView1.Columns[11].HeaderText = "備註";
            dataGridView1.Columns[12].HeaderText = "付款方式";
            dataGridView1.Columns[13].HeaderText = "區域";

            string[] pay = new string[] { "現金", "信用卡", "支票" };
            checkedListBox1.Items.AddRange(pay);
            string[] loc = new string[] { "北區", "中區", "南區", "東區" };
            comboBox1.Items.AddRange(loc);
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
            textBox11.Text = dataGridView1.Rows[e.RowIndex].Cells[10].Value.ToString();
            textBox12.Text = dataGridView1.Rows[e.RowIndex].Cells[11].Value.ToString();
            textBox13.Text = dataGridView1.Rows[e.RowIndex].Cells[12].Value.ToString();
            textBox14.Text = dataGridView1.Rows[e.RowIndex].Cells[13].Value.ToString();

        }
        #region CRUD
        private void button1_Click(object sender, EventArgs e)
        {
            //新增
            try  //使用try...catch...敘述來補捉異動資料可能發生的例外 
            {
                ss = "INSERT INTO YCUST(CONO, NA, NAME,ID,BOSS,AGENT,TEL1,TEL2,PTEL,FAX,IADD,MEMO,PAY,AREA) " +
                    "VALUES (@CONO, @NA, @NAME,@ID,@BOSS,@AGENT,@TEL1,@TEL2,@PTEL,@FAX,@IADD,@MEMO,@PAY,@AREA )";

                cmd.CommandText = ss;

                cmd.Parameters.Add(new SqlParameter("@CONO", SqlDbType.NVarChar));
                cmd.Parameters["@CONO"].Value = textBox1.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@NA", SqlDbType.NVarChar));
                cmd.Parameters["@NA"].Value = textBox2.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@NAME", SqlDbType.NVarChar));
                cmd.Parameters["@NAME"].Value = textBox3.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.NVarChar));
                cmd.Parameters["@ID"].Value = textBox4.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@BOSS", SqlDbType.NVarChar));
                cmd.Parameters["@BOSS"].Value = textBox5.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@AGENT", SqlDbType.NVarChar));
                cmd.Parameters["@AGENT"].Value = textBox6.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@TEL1", SqlDbType.NVarChar));
                cmd.Parameters["@TEL1"].Value = textBox7.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@TEL2", SqlDbType.NVarChar));
                cmd.Parameters["@TEL2"].Value = textBox8.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@PTEL", SqlDbType.NVarChar));
                cmd.Parameters["@PTEL"].Value = textBox9.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@FAX", SqlDbType.NVarChar));
                cmd.Parameters["@FAX"].Value = textBox10.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@IADD", SqlDbType.NVarChar));
                cmd.Parameters["@IADD"].Value = textBox11.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@MEMO", SqlDbType.NVarChar));
                cmd.Parameters["@MEMO"].Value = textBox12.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@PAY", SqlDbType.NVarChar));
                        string s =$"";
                        foreach(var p in checkedListBox1.CheckedItems)
                       {s += p+" ";}
                        cmd.Parameters["@PAY"].Value = s;

                cmd.Parameters.Add(new SqlParameter("@AREA", SqlDbType.NVarChar));
                cmd.Parameters["@AREA"].Value =comboBox1.SelectedItem;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

                ss = "SELECT * FROM YCUST"; //指示ShowData()的內容
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
                ss = "UPDATE YCUST  SET  NA = @NA, NAME = @NAME,ID = @ID,BOSS =@BOSS ,AGENT = @AGENT," +
                         "TEL1 = @TEL1,TEL2 = @TEL2,PTEL = @PTEL,FAX = @FAX,IADD = @IADD,MEMO = @MEMO,PAY = @PAY,AREA = @AREA WHERE CONO = @CONO";
                cmd.CommandText = ss;

                cmd.Parameters.Add(new SqlParameter("@CONO", SqlDbType.NVarChar));
                cmd.Parameters["@CONO"].Value = textBox1.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@NA", SqlDbType.NVarChar));
                cmd.Parameters["@NA"].Value = textBox2.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@NAME", SqlDbType.NVarChar));
                cmd.Parameters["@NAME"].Value = textBox3.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.NVarChar));
                cmd.Parameters["@ID"].Value = textBox4.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@BOSS", SqlDbType.NVarChar));
                cmd.Parameters["@BOSS"].Value = textBox5.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@AGENT", SqlDbType.NVarChar));
                cmd.Parameters["@AGENT"].Value = textBox6.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@TEL1", SqlDbType.NVarChar));
                cmd.Parameters["@TEL1"].Value = textBox7.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@TEL2", SqlDbType.NVarChar));
                cmd.Parameters["@TEL2"].Value = textBox8.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@PTEL", SqlDbType.NVarChar));
                cmd.Parameters["@PTEL"].Value = textBox9.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@FAX", SqlDbType.NVarChar));
                cmd.Parameters["@FAX"].Value = textBox10.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@IADD", SqlDbType.NVarChar));
                cmd.Parameters["@IADD"].Value = textBox11.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@MEMO", SqlDbType.NVarChar));
                cmd.Parameters["@MEMO"].Value = textBox12.Text.Trim();

                cmd.Parameters.Add(new SqlParameter("@PAY", SqlDbType.NVarChar));
                string s = $"";
                foreach (var p in checkedListBox1.CheckedItems)
                { s += p + " "; }
                cmd.Parameters["@PAY"].Value = s;

                cmd.Parameters.Add(new SqlParameter("@AREA", SqlDbType.NVarChar));
                cmd.Parameters["@AREA"].Value = comboBox1.SelectedItem;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

                ss = "SELECT * FROM YCUST";   //指示ShowData()的內容
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
                ss = "DELETE FROM YCUST WHERE CONO = '" + textBox1.Text + "'";
                cmd.CommandText = ss;

                #region 原式
                //ss = "DELETE FROM YCUST WHERE  CONO = @CONO";
                //cmd.CommandText = ss;

                //cmd.Parameters.Add(new SqlParameter("@CONO", SqlDbType.NVarChar));
                //cmd.Parameters["@CONO"].Value = textBox1.Text.Trim();

                //cmd.Parameters.Add(new SqlParameter("@NA", SqlDbType.NVarChar));
                //cmd.Parameters["@NA"].Value = textBox2.Text.Trim();

                //cmd.Parameters.Add(new SqlParameter("@NAME", SqlDbType.NVarChar));
                //cmd.Parameters["@NAME"].Value = textBox3.Text.Trim();

                //cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.NVarChar));
                //cmd.Parameters["@ID"].Value = textBox4.Text.Trim();

                //cmd.Parameters.Add(new SqlParameter("@BOSS", SqlDbType.NVarChar));
                //cmd.Parameters["@BOSS"].Value = textBox5.Text.Trim();

                //cmd.Parameters.Add(new SqlParameter("@AGENT", SqlDbType.NVarChar));
                //cmd.Parameters["@AGENT"].Value = textBox6.Text.Trim();

                //cmd.Parameters.Add(new SqlParameter("@TEL1", SqlDbType.NVarChar));
                //cmd.Parameters["@TEL1"].Value = textBox7.Text.Trim();

                //cmd.Parameters.Add(new SqlParameter("@TEL2", SqlDbType.NVarChar));
                //cmd.Parameters["@TEL2"].Value = textBox8.Text.Trim();

                //cmd.Parameters.Add(new SqlParameter("@PTEL", SqlDbType.NVarChar));
                //cmd.Parameters["@PTEL"].Value = textBox9.Text.Trim();

                //cmd.Parameters.Add(new SqlParameter("@FAX", SqlDbType.NVarChar));
                //cmd.Parameters["@FAX"].Value = textBox10.Text.Trim();

                //cmd.Parameters.Add(new SqlParameter("@IADD", SqlDbType.NVarChar));
                //cmd.Parameters["@IADD"].Value = textBox11.Text.Trim();

                //cmd.Parameters.Add(new SqlParameter("@MEMO", SqlDbType.NVarChar));
                //cmd.Parameters["@MEMO"].Value = textBox12.Text.Trim();
                #endregion

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

                ss = "SELECT * FROM YCUST";  //指示ShowData()的內容
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
                ss = "SELECT * FROM YCUST WHERE ";
                if (textBox1.Text.Trim() != "")
                {
                    ss += " CONO LIKE '%" + textBox1.Text.Trim() + "%'   ";
                }
                else
                {
                    ss += " 1=1 ";
                }

                if (textBox2.Text.Trim() != "")
                {
                    ss += " AND  NA LIKE '%" + textBox2.Text.Trim() + "%'   ";
                }
                else
                {
                    ss += " AND 1=1 ";
                }

                if (textBox3.Text.Trim() != "")
                {
                    ss += " AND  NAME LIKE '%" + textBox3.Text.Trim() + "%'   ";
                }
                else
                {
                    ss += " AND 1=1 ";
                }

                if (checkedListBox1.CheckedItems.Count > 0) {
                    if (checkedListBox1.CheckedItems.Count == 1)
                    {
                        ss += $"AND PAY LIKE '%{checkedListBox1.CheckedItems[0]}%'";
                    }
                    else if (checkedListBox1.CheckedItems.Count == 2) {
                        ss += $"AND PAY LIKE '%{checkedListBox1.CheckedItems[0]}%' AND PAY LIKE '%{checkedListBox1.CheckedItems[1]}%'";
                    }
                    else if (checkedListBox1.CheckedItems.Count == 3)
                    {
                        ss += $"AND PAY LIKE '%{checkedListBox1.CheckedItems[0]}%' AND PAY LIKE '%{checkedListBox1.CheckedItems[1]}%' AND PAY LIKE '%{checkedListBox1.CheckedItems[2]}%'";
                    }
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
            textBox1.Text = ""; textBox2.Text = ""; textBox3.Text = ""; textBox4.Text = ""; textBox5.Text = ""; textBox6.Text = "";
            textBox7.Text = ""; textBox8.Text = ""; textBox9.Text = ""; textBox10.Text = ""; textBox11.Text = ""; textBox12.Text = ""; textBox13.Text = ""; textBox14.Text = "";
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
