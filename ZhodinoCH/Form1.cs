using System;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZhodinoCH
{
    public partial class Form1 : Form
    {

        private string currentDb = "";
        private readonly BindingList<Record> source = new BindingList<Record>();
        private DateTimePicker oDateTimePicker;

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
            
        }

        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Task<string> t;
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
            source.RemoveAt(e.RowIndex);
            source.Insert(e.RowIndex, newItem);
            
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
            //if (oDateTimePicker != null) { oDateTimePicker.Visible = false; }
            //if (e.ColumnIndex == 1)
            //{
            //    oDateTimePicker = new DateTimePicker();
            //    dataGridView1.Controls.Add(oDateTimePicker);
            //    oDateTimePicker.Format = DateTimePickerFormat.Short;
            //    Rectangle oRectangle = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true); 
            //    oDateTimePicker.Size = new Size(oRectangle.Width, oRectangle.Height);
            //    oDateTimePicker.Location = new Point(oRectangle.X, oRectangle.Y);
            //    oDateTimePicker.CloseUp += new EventHandler(oDateTimePicker_CloseUp); 
            //    oDateTimePicker.TextChanged += new EventHandler(dateTimePicker_OnTextChange);
            //    oDateTimePicker.Value = DateTime.Parse(dataGridView1.CurrentCell.Value.ToString());
            //    oDateTimePicker.Visible = true;
                
            //}            
        }

        private void dateTimePicker_OnTextChange(object sender, EventArgs e)
        {             
            dataGridView1.CurrentCell.Value = oDateTimePicker.Text.ToString();
           
        }

        void oDateTimePicker_CloseUp(object sender, EventArgs e)
        {  
            oDateTimePicker.Visible = false;
        }
    }
}
