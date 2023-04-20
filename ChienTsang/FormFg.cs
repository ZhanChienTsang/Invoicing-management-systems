using System;
using System.Windows.Forms;


namespace ChienTsang
{
    public partial class FormFg : Form
    {
        private PurOrd frm;

        public FormFg()   
        {
            InitializeComponent();
        }
        public FormFg(PurOrd formM)   //Like Uses 
        {
            frm = formM;              //方法1
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            dgvFg.DataSource = frm.tbfg;
            DgvSet dgv = new DgvSet();          //dgvFg 設定 (ClsaaSet)
            dgv.dgvSet(dgvFg);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int idx = e.RowIndex;
            frm.MFGNO = frm.tbfg.Rows[idx][0].ToString();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
