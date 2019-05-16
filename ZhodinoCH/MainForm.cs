﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZhodinoCH.Model;
using ZhodinoCH.Properties;
using ZhodinoCH.Utils;

namespace ZhodinoCH
{
    public partial class MainForm : Form
    {

        private static readonly Random random = new Random();

        private Session session = new Session();

        private string currentDb = "";
        //private ConcurrentQueue<string> queue = new ConcurrentQueue<string>();
        private List<ToolStripButton> toolButtons = new List<ToolStripButton>();
        private readonly BindingList<QueueItem> source = new BindingList<QueueItem>();

        public MainForm()
        {
            InitializeComponent();
        }

        private async void Form1_LoadAsync(object sender, EventArgs e)
        {
            var str = await WebClient.DownloadStringAsync("http://tut.by").ConfigureAwait(true);
            MessageBox.Show(str.Length.ToString(CultureInfo.InvariantCulture));
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
            toolStripLabel1.Text = "[" + NetUtils.LocalIPAddress() + "]";
            Source.InsertSession(session);
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
                var recs = await Source.GetAllAsync(currentDb).ConfigureAwait(true);
                source.Clear();
                foreach (var rec in recs)
                {
                    source.Add(rec);
                    if (random.Next(0, 100) > 80)
                    {
                        Application.DoEvents();
                    }
                }
            }
            catch (Exception)
            {
                source.Clear();
            }
        }

        private void DataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;

            Console.WriteLine(sender.GetType().Name);
        }

        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {

        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DataGridView1_RowLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("DataGridView1_CellValueChanged");
            try
            {
                var item = source[e.RowIndex];
                if (string.IsNullOrEmpty(item.Rev))
                {
                    Source.Insert(currentDb, item);
                }
                else
                {
                    Source.Update(currentDb, item);
                }
                var newItem = Source.Get(currentDb, item.ID);
                item.Rev = newItem.Rev;
            }
            catch (Exception)
            { }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                var s = Source.GetSession(session.ID);
                Source.DeleteSession(s);
            }
            finally
            {
                e.Cancel = false;
            }
        }

        private void DataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ToolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }
    }
}
