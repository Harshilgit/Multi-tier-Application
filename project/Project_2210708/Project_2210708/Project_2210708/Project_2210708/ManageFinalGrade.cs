using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Project_2210708
{
    public partial class ManageFinalGrade : Form
    {
        internal enum Modes
        {
            UPDATE,
            
        }

        internal static ManageFinalGrade current;

        private Modes mode = Modes.UPDATE;

        private string[] assignInitial;
        public ManageFinalGrade()
        {
            current = this;
            InitializeComponent();

        }

        private void ManageFinalGrade_Load(object sender, EventArgs e)
        {

        }

        internal void Start(Modes m, DataGridViewSelectedRowCollection c)
        {
            mode = m;
            Text = "" + mode;


            comboBox1.DisplayMember = "StId";
            comboBox1.ValueMember = "StId";
            comboBox1.DataSource = Data.Students.GetStudents();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.SelectedIndex = 0;
            comboBox1.Enabled = false;

            comboBox2.DisplayMember = "CId";
            comboBox2.ValueMember = "CId";
            comboBox2.DataSource = Data.Courses.GetCourses();
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.SelectedIndex = 0;
            comboBox2.Enabled = false;

            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            




            if (c[0].Cells["FinalGrade"]?.Value == null || c[0].Cells["FinalGrade"].Value == DBNull.Value)
            {
                MessageBox.Show("FINAL GRADE IS NULL!");
            }
            else
            {
                comboBox1.SelectedValue = c[0].Cells["StId"].Value;
                comboBox2.SelectedValue = c[0].Cells["CId"].Value;
                textBox3.Text = c[0].Cells["FinalGrade"].Value.ToString();
                assignInitial = new string[] { (string)c[0].Cells["FinalGrade"].Value };
            }





            ShowDialog();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                var a = from r in Data.Students.GetStudents().AsEnumerable()
                        where r.Field<string>("StId") == (string)comboBox1.SelectedValue
                        select new { Name = r.Field<string>("StName") };
                textBox1.Text = a.Single().Name;
            }
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null)
            {
                var a = from r in Data.Courses.GetCourses().AsEnumerable()
                        where r.Field<string>("CId") == (string)comboBox2.SelectedValue
                        select new { Name = r.Field<string>("CName") };
                textBox2.Text = a.Single().Name;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            int r = -1;
            if (mode == Modes.UPDATE)
            {
                List<string[]> lId = new List<string[]>();
                lId.Add(assignInitial);

                int? finalGrade = string.IsNullOrEmpty(textBox3.Text) ? (int?)null : int.Parse(textBox3.Text);
                r = BusinessLayer.Enrollments.UpdateEnr("Enrollments", comboBox1.SelectedValue.ToString(), comboBox2.SelectedValue.ToString(), finalGrade);
            }

            if (r == 1)
            {
                Close();
            }
        }
    }
}
