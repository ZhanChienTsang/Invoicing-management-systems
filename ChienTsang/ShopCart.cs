using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.IO;
using System.Drawing.Printing;

namespace ChienTsang
{
    public partial class ShopCart : Form
    {
        public ShopCart()
        {
            InitializeComponent();
        }
        #region 變數
        DgvSet dgvset;
        SqlConnection cn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        string ss;          //ss=sql string
        int rowidx;     //點擊事件用
        string documentContents;   //列印用
        string stringToPrint;              //列印用

        List<Scart> cart = new List<Scart>();  //購物車Listview用
        class Scart    ////購物車Listview用
        {
            public string Pno { get; set; }
            public string Pname { get; set; }
            public double Pprice { get; set; }
            public int Qty { get; set; }
            public double Amount { get; set; }
        }
        #endregion

        #region 自訂方法
        void ShowData()           // //顯示SQL資料在dataGridView1上
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
            cn.Close();
        }

        private void dataGridView1_data(int idx)    //dataGridView1->點擊顯示內容、圖片,購物車
        {
            string pfname, nopicfn;
            //顯示用戶點選產品資訊
            richTextBox1.Font = new Font("微軟正黑體", 12, FontStyle.Bold);
            richTextBox1.Text = "";
            richTextBox1.Text = "商品編號: " + dataGridView1.Rows[idx].Cells[0].Value.ToString() + "\n";
            richTextBox1.Text += "商品名稱: " + dataGridView1.Rows[idx].Cells[1].Value.ToString() + "\n";
            richTextBox1.Text += "商品價格: " + dataGridView1.Rows[idx].Cells[3].Value.ToString() + "\n";
            richTextBox1.Text += "庫存:\n" + dataGridView1.Rows[idx].Cells[6].Value.ToString() + "\n";
            richTextBox1.Text += " " + "".PadLeft(36, '-') + "\n";
            richTextBox1.Text += "備註:\n" + dataGridView1.Rows[idx].Cells[5].Value.ToString() + "\n";

            //圖片
            pfname = "..\\..\\Pictures\\" + dataGridView1.Rows[idx].Cells[8].Value.ToString();   //照片加上類別路徑
            if (File.Exists(pfname.Trim()) == true)
            {
                pictureBox1.Image = Image.FromFile(pfname);
            }
            else
            {
                nopicfn = "..\\..\\Pictures\\NoPic.png";
            }
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            //購物車
            for (int i = 0; i < cart.Count; i++)
                cart.RemoveAt(i);    //清除buffer
            string str1 = dataGridView1.Rows[idx].Cells[0].Value.ToString();
            string str2 = dataGridView1.Rows[idx].Cells[1].Value.ToString();
            int price = int.Parse(dataGridView1.Rows[idx].Cells[3].Value.ToString());
            cart.Add(new Scart() { Pno = str1, Pname = str2, Pprice = price });
        }
        #endregion

        private void Form4_Load(object sender, EventArgs e)
        {
            //連線設定
            cn.ConnectionString = "Data Source=.\\sql2019;Initial Catalog=YVMENUC;Integrated Security=True";
            cmd.Connection = cn;
            ss = "SELECT * FROM YFGMAST";    //指示ShowData()的內容
            ShowData();

            dgvset = new DgvSet();
            dgvset.dgvSet(dataGridView1);

            //dataGridView1中文欄位名稱設定
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

            //listview1設定
            listView1.View = View.Details;                                   //設置視圖   獲取或設置項在控件中的顯示方式
            listView1.FullRowSelect = true;                                 //設置是否行選擇模式
            listView1.GridLines = true;                                         //設置網格線
            listView1.AllowColumnReorder = true;                  //設置是否可拖動列標頭來對改變列的順序
            listView1.MultiSelect = true;                                     //設置是否可以選擇多個項
            listView1.LabelEdit = true;                                        //設置用戶是否可以編輯控件中項的標籤，對於Detail視圖，只能編輯行第一列的內容
            listView1.CheckBoxes = true;                                   //設置控件中各項的旁邊是否顯示複選框
            listView1.BackColor = Color.Cornsilk;

            //listview1中文欄位名稱設定
            listView1.Columns.Clear();
            listView1.Columns.Add("商品編號", 80, HorizontalAlignment.Left);   //Column width=100 pixel
            listView1.Columns.Add("商品名稱", 100, HorizontalAlignment.Left);   //100 pixel
            listView1.Columns.Add("單價", 60, HorizontalAlignment.Left);     //70 pixel
            listView1.Columns.Add("數量", 60, HorizontalAlignment.Left);
            listView1.Columns.Add("合計", 100, HorizontalAlignment.Left);
        }
       
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //點擊事件
            if (e.RowIndex < 0) return; //若使用者按到上方表頭, 不處理, 返回
            rowidx = e.RowIndex;
            dataGridView1_data(rowidx);
        }

        #region Button of shopcart
        private void button1_Click(object sender, EventArgs e)
        {
            //加入購物車
            bool sameItem = false;
            if (cart.Count == 0)
            { MessageBox.Show("請挑選商品!!!"); }
            else
            {
                cart[0].Qty = Convert.ToInt32(textBox1.Text.Trim());
                cart[0].Amount = cart[0].Pprice * cart[0].Qty;
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    if (listView1.Items[i].SubItems[0].Text == cart[0].Pno)  //若同商品, 數量累加
                    {
                        listView1.Items[i].SubItems[3].Text = (Convert.ToInt32(listView1.Items[i].SubItems[3].Text) + cart[0].Qty).ToString();
                        listView1.Items[i].Checked = true;
                        sameItem = true;
                    }
                }
                if (sameItem == false)
                {
                    foreach (var p in cart)
                    {
                        ListViewItem lvitm = new ListViewItem(p.Pno);
                        lvitm.SubItems.Add(p.Pname.ToString());
                        lvitm.SubItems.Add(p.Pprice.ToString());
                        lvitm.SubItems.Add(p.Qty.ToString());
                        lvitm.SubItems.Add(p.Amount.ToString());

                        listView1.Items.Add(lvitm);
                        listView1.Items[listView1.Items.Count - 1].Checked = true;
                    }
                }
            }
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            //全選
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].Checked = true;   
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            //取消全選
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].Checked = false;    //None
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //刪除
            foreach (ListViewItem p in listView1.CheckedItems)
            {
                p.Remove();
            }
            MessageBox.Show("已刪除");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //列印
            printDocument1.DocumentName = richTextBox2.Text;
            documentContents = richTextBox2.Text;
            stringToPrint = richTextBox2.Text;

            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            //列印設定
            int charactersOnPage = 0;
            int linesPerPage = 0;
            // Sets the value of charactersOnPage to the number of characters 
            // of stringToPrint that will fit within the bounds of the page.
            e.Graphics.MeasureString(stringToPrint, this.Font,
                e.MarginBounds.Size, StringFormat.GenericTypographic,
                out charactersOnPage, out linesPerPage);

            // Draws the string within the bounds of the page.
            e.Graphics.DrawString(stringToPrint, this.Font, Brushes.Black,
                e.MarginBounds, StringFormat.GenericTypographic);

            // Remove the portion of the string that has been printed.
            stringToPrint = stringToPrint.Substring(charactersOnPage);

            // Check to see if more pages are to be printed.
            e.HasMorePages = (stringToPrint.Length > 0);

            // If there are no more pages, reset the string to be printed.
            if (!e.HasMorePages)
                stringToPrint = documentContents;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //查詢
            try //使用try...catch...敘述來補捉異動資料可能發生的例外
            {
                ss = "SELECT * FROM YFGMAST WHERE ";
                if (textBox2.Text.Trim() != "")
                {
                    ss += " FGNO LIKE '%" + textBox2.Text.Trim() + "%'   ";
                }
                else
                {
                    ss += " 1=1 ";
                }

                if (textBox3.Text.Trim() != "")
                {
                    ss += " AND  FGNAME LIKE '%" + textBox3.Text.Trim() + "%'   ";
                }
                else
                {
                    ss += " AND 1=1 ";
                }

                if (textBox4.Text.Trim() != "")
                {
                    ss += " AND  CONO LIKE '%" + textBox4.Text.Trim() + "%'   ";
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
        private void button2_Click(object sender, EventArgs e)
        {
            //  結帳
            int total = 0;
            int count = 0;
            if (listView1.CheckedItems.Count <= 0)
            {
                MessageBox.Show("尚未購物, 請選購商品!!!");
                return;
            }

            richTextBox2.Font = new Font("標楷體", 11);    //"標楷體"
            richTextBox2.Text = "";
            richTextBox2.Text = $"{" ",23}消 費 清 單\n\n";

            foreach (ListViewItem p in listView1.CheckedItems)
            {
                // Checked 
                count++;
                byte[] pc = Encoding.GetEncoding("big5").GetBytes(p.SubItems[1].Text.Trim());

                if (pc.Length < 8)
                    richTextBox2.Text += $"{count,3}. {p.SubItems[1].Text.Trim(),-0}\t\t\t\t\t {p.SubItems[2].Text,4}  x {p.SubItems[3].Text,3} = {p.SubItems[4].Text,6:n0}" + "\n";
                else if (pc.Length < 14)
                    richTextBox2.Text += $"{count,3}. {p.SubItems[1].Text.Trim(),-0}\t\t\t\t {p.SubItems[2].Text,4}  x {p.SubItems[3].Text,3} = {p.SubItems[4].Text,6:n0}" + "\n";
                else if (pc.Length < 20)
                    richTextBox2.Text += $"{count,3}. {p.SubItems[1].Text.Trim(),-0}\t\t\t {p.SubItems[2].Text,4}  x {p.SubItems[3].Text,3} = {p.SubItems[4].Text,6:n0}" + "\n";
                else if (pc.Length < 25)
                    richTextBox2.Text += $"{count,3}. {p.SubItems[1].Text.Trim(),-0}\t\t {p.SubItems[2].Text,4}  x {p.SubItems[3].Text,3} = {p.SubItems[4].Text,6:n0}" + "\n";
                else if (pc.Length < 32)
                    richTextBox2.Text += $"{count,3}. {p.SubItems[1].Text.Trim(),-0}\t {p.SubItems[2].Text,4}  x {p.SubItems[3].Text,3} = {p.SubItems[4].Text,6:n0}" + "\n";
                else
                    richTextBox2.Text += $"{count,3}. {p.SubItems[1].Text.Trim(),-0} {p.SubItems[2].Text,4}  x {p.SubItems[3].Text,3} = {p.SubItems[4].Text,6:n0}" + "\n";

                total += Convert.ToInt32(p.SubItems[4].Text);

            }
            richTextBox2.Text += "\n";
            richTextBox2.Text += " " + "".PadLeft(55, '-') + "\n";
            richTextBox2.Text += "\n";
            richTextBox2.Text += "  購買項目：" + $"{count,44:n0}\n";
            richTextBox2.Text += "\n";
            richTextBox2.Text += "  總    計：" + $"{total,44:n0}\n";
            richTextBox2.Text += "\n" + $"{" ",23}謝 謝 惠 顧 !" + "\n";
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
