using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChienTsang
{
    public partial class 購物車3 : Form
    {
        public 購物車3()
        {
            InitializeComponent();
        }
        #region 各變數、類別
        List<Meal> lst = new List<Meal>();  //便當串列
        DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
        List<int> prr = new List<int>() { 100, 90, 80, 60, 50 };  //價錢
        float sum,sum1;
        int indx, indx2, a, lst_removeat ;
        int er = 0, ec = 0;
        class Meal
        {//便當的類別
            public string 品項 { get; set; }
            public int 數量 { get; set; }
            public int 總計 { get; set; }
        }
        #endregion

        private void 購物車3_Load(object sender, EventArgs e)
        {          
            //便當種類& $
            string[] m = new string[] { "雞腿飯" + "  " + prr[0]+"元",
                                                             "雞排飯" + "  "  + prr[1]+"元",
                                                             "排骨飯" + "  "  +prr[2]+"元",
                                                             "控肉飯"+ "  "  + prr[3]+"元",
                                                             "菜飯"+ "  "  + prr[4]+"元"};
            listBox1.Items.AddRange(m);  

            dgv1.Columns.Add(chk);
           chk.HeaderText = "確認";
            chk.Name = "chk";
            chk.TrueValue = true; chk.FalseValue = false;

            dgv1.AutoResizeColumns();
        }

        #region Button For Comdy
        private void CmdyAdd_Click(object sender, EventArgs e)
        {
            //商品新增
            listBox1.DataSource = null;
            listBox1.Items.Add(textBox1.Text + "  " + textBox2.Text + "元");
            prr.Add(Convert.ToInt32(textBox2.Text));
        }

        private void CmdyUpdate_Click(object sender, EventArgs e)
        {
            //商品更新
            listBox1.DataSource = null;
            indx = listBox1.SelectedIndex;
            listBox1.Items[indx] = textBox1.Text.Trim() + "  " + textBox2.Text.Trim() + "元";
            prr.Add(int.Parse( textBox2.Text.Trim()));
            for (int i = 0; i < listBox1.Items.Count; i++) 
            {
                if (listBox1.Items[i].ToString().Split(' ')[0] == textBox1.Text.Trim())
                {
                    prr[i] = int.Parse(textBox2.Text.Trim());
                    //dgv1.Rows[er].Cells["品項"].Value = listBox1.Items[indx];
                    //dgv1.Rows[er].Cells["總計"].Value =int.Parse(dgv1.Rows[er].Cells["數量"].Value.ToString()) *prr.ToArray()[i];
                }
            }
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            //textBox1.Text = listBox1.SelectedItem.ToString();
        }

        private void dgv1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //DGV數量更改
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                for (int i = 0; i < listBox1.Items.Count; i++) 
                {
                    if (dgv1.Rows[er].Cells["品項"].Value.ToString().Split(' ')[0] == listBox1.Items[i].ToString().Split(' ')[0])
                    {
                        dgv1.Rows[er].Cells["總計"].Value =int.Parse(dgv1.Rows[er].Cells["數量"].Value.ToString()) *prr.ToArray()[i];
                    }
                }
            }
        }
       
        private void dgv1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            er = e.RowIndex;
            ec = e.ColumnIndex;
            //點選移除位置
            lst_removeat = e.RowIndex;
        }

        private void CmdyDelete_Click(object sender, EventArgs e)
        {
            //商品刪除
            indx2 = listBox1.SelectedIndex;
            listBox1.DataSource = null;
            listBox1.Items.RemoveAt(indx2);
        }
        #endregion

        #region Button For Shopcart
        private void SpctAmount_Click(object sender, EventArgs e)
        {
        //結帳
        richTextBox1.Text = "";
            sum = 0;
            sum1 = 0;
            foreach (DataGridViewRow p in dgv1.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)p.Cells[0];
                if (chk.Value == chk.TrueValue)
                {
                   richTextBox1.Text += p.Cells[1].Value.ToString() + "," +"數量:"+ p.Cells[2].Value.ToString() + ","+"金額:"+p.Cells[3].Value.ToString()+"\n";
                }
                sum +=float.Parse( p.Cells["總計"].Value.ToString() ) ;
                sum1+= float.Parse(p.Cells["數量"].Value.ToString());
            }
            richTextBox1.Text += "\n\n";
            richTextBox1.Text += "總便當數:" + $"{sum1}" + "個"+"\n"+"總計金額:" + $"{sum}" + "元";
            dgv1.EndEdit();
        }


        private void SpctDelete_Click(object sender, EventArgs e)
        {
            //刪除
            lst.RemoveAt(lst_removeat);
            dgv1.DataSource = null;
            dgv1.DataSource = lst;
        }


        private void SpctAdd_Click(object sender, EventArgs e)
        {
            //加入購物車
            a = listBox1.SelectedIndex;
            int total =  prr.ToArray()[a]*(int)numericUpDown1.Value;
            lst.Add(new Meal() { 品項 = listBox1.SelectedItem.ToString(), 數量 = Convert.ToInt32(numericUpDown1.Value), 總計 = total });
            dgv1.DataSource = null;
            dgv1.DataSource = lst;
        }
        #endregion
    }
}
