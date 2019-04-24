using System;
using System.Net;
using System.Windows.Forms;

namespace ZhodinoCH
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.Text = toolStripButton1.Text;
            this.toolStripButton1.Checked = true;
            this.toolStripButton2.Checked = false;
            this.toolStripButton3.Checked = false;
            this.toolStripButton4.Checked = false;
            this.toolStripButton5.Checked = false;
            this.toolStripButton6.Checked = false;
            this.toolStripButton7.Checked = false;
            this.downloads();
        }

        private void downloads()
        {
            WebClient client = new WebClient();
           
            client.DownloadStringCompleted += Client_DownloadStringCompleted;
            client.DownloadStringAsync(new Uri("http://178.124.170.17:5984/pelvic/_all_docs"));
            
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

        }

        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

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
