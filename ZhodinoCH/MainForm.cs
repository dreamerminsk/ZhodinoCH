using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZhodinoCH.Properties;

namespace ZhodinoCH
{
    public partial class MainForm : Form
    {

        private static readonly Random random = new Random();

        private string currentDb = "";
        //private ConcurrentQueue<string> queue = new ConcurrentQueue<string>();
        private List<ToolStripButton> toolButtons = new List<ToolStripButton>();
        private readonly BindingList<Record> source = new BindingList<Record>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //for (int i = 1000; i < 10000; i++)
            //{
            //    var rec = new Record(Guid.NewGuid().ToString(),
            //        null, DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            //        "name-" + i, "" + i + "" + (i+7), "comment");
            //    Repository.Insert("fgds", rec);
            //    Console.WriteLine(i + ". " + rec.ID);
            //    Thread.Sleep(36);
            //}
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
            toolStrip1.Enabled = false;
            ToolStripButton toolButton = (ToolStripButton)sender;
            currentDb = toolButton.Tag.ToString();
            await Downloads().ConfigureAwait(true);            
            Text = toolButton.Text + " - " + Settings.Default.Application + " " + Settings.Default.Version;
            toolButton.ToolTipText = toolButton.Text + " (" + source.Count + ")";
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
            toolStrip1.Enabled = true;
        }

        private async Task Downloads()
        {            
            try
            {
                var recs = await Repository.GetAllAsync(currentDb).ConfigureAwait(true);
                source.Clear();
                foreach (var rec in recs)
                {
                    source.Add(rec);
                    if (random.Next(0, 100) > 64)
                    {
                        Application.DoEvents();
                    }                    
                }
            }
            catch (Exception ex)
            {
                source.Clear();
                Console.WriteLine(ex.StackTrace);
                _ = MessageBox.Show(ex.TargetSite.Name, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void DataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {

        }
    }
}
