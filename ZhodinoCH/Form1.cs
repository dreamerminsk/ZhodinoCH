using System;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;

namespace ZhodinoCH
{
    public partial class Form1 : Form
    {

        private string currentDb = "";
        private readonly BindingList<Record> source = new BindingList<Record>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = source;
            toolStripButton1.PerformClick();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Text = toolStripButton1.Text;
            currentDb = "belgosstrakh";
            toolStripButton1.Checked = true;
            toolStripButton2.Checked = false;
            toolStripButton3.Checked = false;
            toolStripButton4.Checked = false;
            toolStripButton5.Checked = false;
            toolStripButton6.Checked = false;
            toolStripButton7.Checked = false;
            downloads();
        }

        private void downloads()
        {
            var recs = Repository.GetAll(currentDb);
            source.Clear();
            foreach (var rec in recs)
            {
                source.Add(rec);
            }
            
        }

        private void Client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            MessageBox.Show(e.Result);
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this.Text = toolStripButton2.Text;
            currentDb = "bca";
            this.toolStripButton1.Checked = false;
            this.toolStripButton2.Checked = true;
            this.toolStripButton3.Checked = false;
            this.toolStripButton4.Checked = false;
            this.toolStripButton5.Checked = false;
            this.toolStripButton6.Checked = false;
            this.toolStripButton7.Checked = false;
            this.downloads();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            this.Text = toolStripButton3.Text;
            currentDb = "viscera";
            this.toolStripButton1.Checked = false;
            this.toolStripButton2.Checked = false;
            this.toolStripButton3.Checked = true;
            this.toolStripButton4.Checked = false;
            this.toolStripButton5.Checked = false;
            this.toolStripButton6.Checked = false;
            this.toolStripButton7.Checked = false;
            this.downloads();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            this.Text = toolStripButton4.Text;
            currentDb = "pelvic";
            this.toolStripButton1.Checked = false;
            this.toolStripButton2.Checked = false;
            this.toolStripButton3.Checked = false;
            this.toolStripButton4.Checked = true;
            this.toolStripButton5.Checked = false;
            this.toolStripButton6.Checked = false;
            this.toolStripButton7.Checked = false;
            this.downloads();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            this.Text = toolStripButton5.Text;
            currentDb = "heart";
            this.toolStripButton1.Checked = false;
            this.toolStripButton2.Checked = false;
            this.toolStripButton3.Checked = false;
            this.toolStripButton4.Checked = false;
            this.toolStripButton5.Checked = true;
            this.toolStripButton6.Checked = false;
            this.toolStripButton7.Checked = false;
            this.downloads();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            this.Text = toolStripButton6.Text;
            currentDb = "fgds";
            this.toolStripButton1.Checked = false;
            this.toolStripButton2.Checked = false;
            this.toolStripButton3.Checked = false;
            this.toolStripButton4.Checked = false;
            this.toolStripButton5.Checked = false;
            this.toolStripButton6.Checked = true;
            this.toolStripButton7.Checked = false;
            this.downloads();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            this.Text = toolStripButton7.Text;
            currentDb = "thyroid";
            this.toolStripButton1.Checked = false;
            this.toolStripButton2.Checked = false;
            this.toolStripButton3.Checked = false;
            this.toolStripButton4.Checked = false;
            this.toolStripButton5.Checked = false;
            this.toolStripButton6.Checked = false;
            this.toolStripButton7.Checked = true;
            this.downloads();
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DataGridView1_RowLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            Console.WriteLine(e.RowIndex + ", " + e.ColumnIndex);
        }

        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string value = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            Console.WriteLine(e.RowIndex + ", " + e.ColumnIndex + " " + value);
            Console.WriteLine(source[e.RowIndex].Name);
            Repository.Update(currentDb, source[e.RowIndex]);
        }

        private void DataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            System.Console.WriteLine(e.RowIndex);

        }
    }
}
