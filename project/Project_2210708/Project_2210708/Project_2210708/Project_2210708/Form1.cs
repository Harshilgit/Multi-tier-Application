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

namespace Project_2210708
{
    public partial class Form1 : Form
    {
        internal enum Grids
        {
            Std,
            Crs,
            Enrl,
            Prog
        }
        internal static Form1 current;
        private Grids grid;
        internal DataGridViewSelectedRowCollection c;
        public Form1()
        {
            current = this;
            InitializeComponent();
        }

        private void studentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grid = Grids.Std;
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            bindingSource3.DataSource = Data.Students.GetStudents();
            bindingSource3.Sort = "StId";
            dataGridView1.DataSource = bindingSource3;

            dataGridView1.Columns["StId"].HeaderText = "Student ID";
            dataGridView1.Columns["StId"].DisplayIndex = 0;
            dataGridView1.Columns["StName"].DisplayIndex = 1;
            dataGridView1.Columns["ProgId"].DisplayIndex = 2;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            new AddEnrollment();
            AddEnrollment.current.Visible = false;

            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void programsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grid = Grids.Prog;
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            bindingSource1.DataSource = Data.Programs.GetPrograms();
            bindingSource1.Sort = "ProgId";
            dataGridView1.DataSource = bindingSource1;
        }

        private void coursesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grid = Grids.Crs;
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            bindingSource2.DataSource = Data.Courses.GetCourses();
            bindingSource2.Sort = "CId";
            dataGridView1.DataSource = bindingSource2;

            dataGridView1.Columns["CId"].HeaderText = "Course ID";
            dataGridView1.Columns["CId"].DisplayIndex = 0;
            dataGridView1.Columns["CName"].DisplayIndex = 1;
            dataGridView1.Columns["ProgId"].DisplayIndex = 2;
            
        }

        private void enrollmentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grid != Grids.Enrl)
            {
                grid = Grids.Enrl;
                dataGridView1.ReadOnly = false;
                dataGridView1.AllowUserToAddRows = true;
                dataGridView1.AllowUserToDeleteRows = true;
                dataGridView1.RowHeadersVisible = true;
                dataGridView1.Dock = DockStyle.Fill;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                bindingSource4.DataSource = Data.Enrollments.GetDisplayEnrl();
                bindingSource4.Sort = "StId, CId";
                dataGridView1.DataSource = bindingSource4;

                dataGridView1.Columns["StId"].HeaderText = "StId";
                dataGridView1.Columns["StId"].DisplayIndex = 0;
                dataGridView1.Columns["StName"].HeaderText = "StName";
                dataGridView1.Columns["StName"].DisplayIndex = 1;
                dataGridView1.Columns["CId"].HeaderText = "CId";
                dataGridView1.Columns["CId"].DisplayIndex = 2;
                dataGridView1.Columns["CId"].HeaderText = "CName";
                dataGridView1.Columns["CName"].DisplayIndex = 3;
                dataGridView1.Columns["FinalGrade"].HeaderText = "FinalGrade";
                dataGridView1.Columns["FinalGrade"].DisplayIndex = 4;
                // dataGridView1.Columns["ProgId"].HeaderText = "ProgId";
                //dataGridView1.Columns["ProgId"].DisplayIndex = 5;
                //dataGridView1.Columns["ProgName"].DisplayIndex = 6;
            }


        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {
            BusinessLayer.Programs.UpdatePrograms();
        }

        private void bindingSource2_CurrentChanged(object sender, EventArgs e)
        {
            BusinessLayer.Courses.UpdateCourses();
        }

        private void bindingSource3_CurrentChanged(object sender, EventArgs e)
        {
            BusinessLayer.Students.UpdateStudents();
        }

        private void bindingSource4_CurrentChanged(object sender, EventArgs e)
        {
            //BusinessLayer.Enrollments.UpdateEnrollments();
        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            BusinessLayer.Programs.UpdatePrograms();
            BusinessLayer.Courses.UpdateCourses();
            BusinessLayer.Students.UpdateStudents();
            //BusinessLayer.Enrollments.UpdateEnrollments();
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Impossible to insert / update / delete");
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            AddEnrollment.current.Start(AddEnrollment.Modes.INSERT, null);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {


            DataGridViewSelectedRowCollection c = dataGridView1.SelectedRows;
            //int c = dataGridView1.SelectedRows[0].Index;
            if (c.Count == 0)
            {
                MessageBox.Show("At least one line must be selected for deletion");
            }

            else // (c.Count > 1)
            {
                List<string[]> lId = new List<string[]>();
                for (int i = 0; i < c.Count; i++)
                {
                    lId.Add(new string[] { ("" + c[i].Cells["StId"].Value),
                                        ("" + c[i].Cells["CId"].Value) });
                }

                Data.Enrollments.DeleteData(lId);
            }
        }

        internal static void UIMessage(string msg)
        {
            MessageBox.Show(msg);
        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection c = dataGridView1.SelectedRows;
            if (c.Count == 0)
            {
                MessageBox.Show("A line must be selected for update");
            }
            else if (c.Count > 1)
            {
                MessageBox.Show("Only one line must be selected for update");
            }
            else
            {
                AddEnrollment.current.Start(AddEnrollment.Modes.UPDATE, c);
            }
        }

        private void manageFinalGradeToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DataGridViewSelectedRowCollection c = dataGridView1.SelectedRows;
            if (c.Count == 0)
            {
                MessageBox.Show("A line must be selected for update");
            }
            else if (c.Count > 1)
            {
                MessageBox.Show("Only one line must be selected for update");
            }
            else
            {
                ManageFinalGrade form = new ManageFinalGrade();
                form.Start(ManageFinalGrade.Modes.UPDATE, c);
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToDeleteRows = false;
                dataGridView1.RowHeadersVisible = true;
                dataGridView1.Dock = DockStyle.Fill;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        int selectedIndex;
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                selectedIndex = dataGridView1.SelectedRows[0].Index;
            }
        }
    }
}
