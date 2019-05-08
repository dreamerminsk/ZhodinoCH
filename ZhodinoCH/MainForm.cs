using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZhodinoCH.Properties;

namespace ZhodinoCH
{
    public partial class MainForm : Form
    {
        //private 
        private string currentDb = "";
        private ConcurrentQueue<string> queue = new ConcurrentQueue<string>();
        private List<ToolStripButton> toolButtons = new List<ToolStripButton>();
        private readonly BindingList<Record> source = new BindingList<Record>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Repository.InsertUser("editor001", "111");
            //Repository.InsertSecurity("belgosstrakh", "editor");
            for (int i = 0; i < Settings.Default.DbTitles.Count; i++)
            {
                var item = new ToolStripButton(Settings.Default.DbTitles[i])
                {
                    Font = Settings.Default.ToolButtonFont,
                    Tag = Settings.Default.DbNames[i]
                };
                item.Click += Item_ClickAsync;
                toolButtons.Add(item);
                toolStrip1.Items.Insert(0, item);
            }
            dataGridView1.DataSource = source;
            if (Settings.Default.User.Length > 0)
            {
                toolStripLabel1.Text = Settings.Default.User;
            }
            else
            {
                toolStripLabel1.Text = "[" + NetUtils.LocalIPAddress() + "]";
            }
            toolButtons[0].PerformClick();
        }

        private async void Item_ClickAsync(object sender, EventArgs e)
        {
            ToolStripButton toolButton = (ToolStripButton)sender;
            currentDb = toolButton.Tag.ToString();
            await downloads().ConfigureAwait(true);            
            Text = toolButton.Text + " - " + Settings.Default.Application + " " + Settings.Default.Version;            
            foreach (var item in toolButtons)
            {
                if (item == toolButton)
                {
                    item.Checked = true;
                }
                else
                {
                    item.Checked = false;
                }
            }
        }

        private async Task downloads()
        {            
            try
            {
                var recs = await Repository.GetAllAsync(currentDb).ConfigureAwait(true);
                source.Clear();
                foreach (var rec in recs)
                {
                    source.Add(rec);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                _ = MessageBox.Show(ex.TargetSite.Name, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

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
            try
            {
                var item = source[e.RowIndex];
                if (string.IsNullOrEmpty(item.Rev))
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
            catch (Exception)
            { }
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
            Settings.Default.User = toolStripTextBox1.Text;
            Settings.Default.Save();
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
                    Settings.Default.User = toolStripTextBox1.Text;
                    Settings.Default.Save();
                    break;
            }
        }
    }
}
