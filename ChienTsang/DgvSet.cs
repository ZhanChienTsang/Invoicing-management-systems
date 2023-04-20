using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Data;


namespace ChienTsang
{
    class DgvSet
    {
        public void dgvSet(DataGridView dgv)              //dataGridView1 設定
        {
            //使用Binding，不能設定行、列數
            // Set the column header style.
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
            dgv.EnableHeadersVisualStyles = false;
            columnHeaderStyle.BackColor = Color.FromArgb(153, 255, 204);     //必須要上列設定才能有效   淺綠色
            columnHeaderStyle.ForeColor = Color.FromArgb(0, 0, 255);         //字體色  藍色;
            columnHeaderStyle.Font = new Font("Arial", 12, FontStyle.Regular);              //字型,大小,字體
            columnHeaderStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;        //水平-置中(必須關閉欄位排序功能)
            dgv.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
            // Set the row header style.
            DataGridViewCellStyle rowHeaderStyle = new DataGridViewCellStyle();
            dgv.EnableHeadersVisualStyles = false;
            rowHeaderStyle.BackColor = Color.FromArgb(153, 255, 204);     //必須要上列設定才能有效   淺綠色
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
            }
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;     //欄寬自動調整
        }


    }
}
