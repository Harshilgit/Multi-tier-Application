using BusinessLayer;
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
    public partial class AddEnrollment : Form
    {
        internal enum Modes
        {
            INSERT,
            UPDATE
        }

        internal static AddEnrollment current;
        private Modes mode = Modes.INSERT;
        private string[] assignInitial;
        public AddEnrollment()
        {
            current = this;
            InitializeComponent();
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

            comboBox2.DisplayMember = "CId";
            comboBox2.ValueMember = "CId";
            comboBox2.DataSource = Data.Courses.GetCourses();
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.SelectedIndex = 0;

            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            
            

            if ((mode == Modes.UPDATE) && (c != null))
            {
                
                comboBox1.SelectedValue = c[0].Cells["StId"].Value;
                comboBox2.SelectedValue = c[0].Cells["CId"].Value;
                
                
                assignInitial = new string[] { (string)c[0].Cells["StId"].Value, (string)c[0].Cells["CId"].Value };
            }



            ShowDialog();
        }

        

        

        private void AddEnrollment_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int r = -1;
            if (mode == Modes.INSERT)
            {
                r = Data.Enrollments.InsertData(new string[] { (string)comboBox1.SelectedValue, (string)comboBox2.SelectedValue });
            }
            if (mode == Modes.UPDATE)
            {
                List<string[]> lId = new List<string[]>();
                lId.Add(assignInitial);

                r = Data.Enrollments.InsertData(new string[] { (string)comboBox1.SelectedValue, (string)comboBox2.SelectedValue });

                if (r == 0)
                {
                    r = Data.Enrollments.DeleteData(lId);
                    //r = Data.Enrollments.DeleteData(lId.Select(id => new string[] { id.ToString() }).ToList());

                }

            }

            if (r == 0) { Close(); }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
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
    }
}
