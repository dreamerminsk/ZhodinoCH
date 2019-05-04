using System;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;

namespace ZhodinoCH
{
    public partial class Form1 : Form
    {
        //private 
        private string currentDb = "";
        private readonly BindingList<Record> source = new BindingList<Record>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("ru-RU");
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
            dataGridView1.DataSource = source;
            toolStripLabel1.Text = "Аноним " + NetUtils.LocalIPAddress();
            toolStripButton1.PerformClick();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Text = "<<" + NetUtils.GetLocalName() + ">> " + toolStripButton1.Text;
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

        private async void downloads()
        {
            try
            {
                var recs = await Repository.GetAllAsync(currentDb);
                source.Clear();
                foreach (var rec in recs)
                {
                    source.Add(rec);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Text = "<<" + NetUtils.GetLocalName() + ">> " + toolStripButton1.Text;
            currentDb = "bca";
            toolStripButton1.Checked = false;
            toolStripButton2.Checked = true;
            toolStripButton3.Checked = false;
            toolStripButton4.Checked = false;
            toolStripButton5.Checked = false;
            toolStripButton6.Checked = false;
            toolStripButton7.Checked = false;
            downloads();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Text = "<<" + NetUtils.GetLocalName() + ">> " + toolStripButton1.Text;
            currentDb = "viscera";
            toolStripButton1.Checked = false;
            toolStripButton2.Checked = false;
            toolStripButton3.Checked = true;
            toolStripButton4.Checked = false;
            toolStripButton5.Checked = false;
            toolStripButton6.Checked = false;
            toolStripButton7.Checked = false;
            downloads();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Text = "<<" + NetUtils.GetLocalName() + ">> " + toolStripButton1.Text;
            currentDb = "pelvic";
            toolStripButton1.Checked = false;
            toolStripButton2.Checked = false;
            toolStripButton3.Checked = false;
            toolStripButton4.Checked = true;
            toolStripButton5.Checked = false;
            toolStripButton6.Checked = false;
            toolStripButton7.Checked = false;
            downloads();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            Text = "<<" + NetUtils.GetLocalName() + ">> " + toolStripButton1.Text;
            currentDb = "heart";
            toolStripButton1.Checked = false;
            toolStripButton2.Checked = false;
            toolStripButton3.Checked = false;
            toolStripButton4.Checked = false;
            toolStripButton5.Checked = true;
            toolStripButton6.Checked = false;
            toolStripButton7.Checked = false;
            downloads();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            Text = "<<" + NetUtils.GetLocalName() + ">> " + toolStripButton1.Text;
            currentDb = "fgds";
            toolStripButton1.Checked = false;
            toolStripButton2.Checked = false;
            toolStripButton3.Checked = false;
            toolStripButton4.Checked = false;
            toolStripButton5.Checked = false;
            toolStripButton6.Checked = true;
            toolStripButton7.Checked = false;
            downloads();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            Text = "<<" + NetUtils.GetLocalName() + ">> " + toolStripButton1.Text;
            currentDb = "thyroid";
            toolStripButton1.Checked = false;
            toolStripButton2.Checked = false;
            toolStripButton3.Checked = false;
            toolStripButton4.Checked = false;
            toolStripButton5.Checked = false;
            toolStripButton6.Checked = false;
            toolStripButton7.Checked = true;
            downloads();
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
            var item = source[e.RowIndex];
            if (item.Rev == "")
            {
                Repository.Insert(currentDb, item);
            }
            else
            {
                Repository.Update(currentDb, item);
            }
            var newItem = Repository.Get(currentDb, item.ID);
            item.Rev = newItem.Rev;

        }

        private void DataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            
        }

        private void SplitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {           
        }

        private void ToolStripTextBox1_Click(object sender, EventArgs e)
        {
            toolStripTextBox1.Enabled = true;
        }

        private void ToolStripTextBox1_Leave(object sender, EventArgs e)
        {
            toolStripLabel1.Visible = true;
            toolStripTextBox1.Visible = false;
            toolStripLabel1.Text = toolStripTextBox1.Text;
        }

        private void ToolStripLabel1_DoubleClick(object sender, EventArgs e)
        {
        }

        private void ToolStripLabel1_Click(object sender, EventArgs e)
        {
            toolStripLabel1.Visible = false;
            toolStripTextBox1.Width = toolStripLabel1.Width;
            toolStripTextBox1.Text = toolStripLabel1.Text;
            toolStripTextBox1.Visible = true;
        }

        private void ToolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case '\r':
                    toolStripLabel1.Visible = true;
                    toolStripTextBox1.Visible = false;
                    toolStripLabel1.Text = toolStripTextBox1.Text;
                    break;
            }
        }
    }
}
