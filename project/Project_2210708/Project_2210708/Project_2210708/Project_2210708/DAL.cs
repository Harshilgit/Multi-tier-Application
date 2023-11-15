using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Data
{
    internal class Connect
    {
        private static String cliComConnectionString = GetConnectString();
        internal static String ConnectionString { get => cliComConnectionString; }

        private static String GetConnectString()
        {
            SqlConnectionStringBuilder cs = new SqlConnectionStringBuilder();
            cs.DataSource = "(local)";
            cs.InitialCatalog = "College1en";
            cs.UserID = "sa";
            cs.Password = "sysadm";
            return cs.ConnectionString;
        }

    }

    internal class DataTables
    {
        private static SqlDataAdapter adapterStudents = InitAdapterStudents();
        private static SqlDataAdapter adapterEnrollments = InitAdapterEnrollments();
        private static SqlDataAdapter adapterCourses = InitAdapterCourses();
        private static SqlDataAdapter adapterPrograms = InitAdapterPrograms();

        private static DataSet ds = InitDataSet();

        private static SqlDataAdapter InitAdapterStudents()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM Students ORDER BY StId",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }

        //private static SqlDataAdapter InitAdapterEnrollments()
        //{
        //    SqlDataAdapter r = new SqlDataAdapter(
        //        "SELECT * FROM Enrollments ORDER BY StId",
        //        Connect.ConnectionString);

        //    SqlCommandBuilder builder = new SqlCommandBuilder(r);
        //    r.UpdateCommand = builder.GetUpdateCommand();

        //    return r;
        //}
        private static SqlDataAdapter InitAdapterEnrollments()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM Enrollments ORDER BY StId, CId ",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;

            // SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            //adapter.UpdateCommand = builder.GetUpdateCommand();


            //return adapter;
        }


        private static SqlDataAdapter InitAdapterCourses()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM Courses ORDER BY CId",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }

        private static SqlDataAdapter InitAdapterPrograms()
        {
            SqlDataAdapter r = new SqlDataAdapter(
                "SELECT * FROM Programs ORDER BY ProgId",
                Connect.ConnectionString);

            SqlCommandBuilder builder = new SqlCommandBuilder(r);
            r.UpdateCommand = builder.GetUpdateCommand();

            return r;
        }

        private static DataSet InitDataSet()
        {
            DataSet ds = new DataSet();
            loadPrograms(ds);
            loadStudents(ds);
            loadCourses(ds);
            loadEnrollments(ds);

            return ds;
        }

        private static void loadPrograms(DataSet ds)
        {
            adapterPrograms.Fill(ds, "Programs");

            ds.Tables["Programs"].Columns["ProgId"].AllowDBNull = false;
            ds.Tables["Programs"].Columns["ProgName"].AllowDBNull = false;

            ds.Tables["Programs"].PrimaryKey = new DataColumn[1]
            { ds.Tables["Programs"].Columns["ProgId"]};

        }

        private static void loadStudents(DataSet ds)
        {
            adapterStudents.Fill(ds, "Students");

            ds.Tables["Students"].Columns["StId"].AllowDBNull = false;
            ds.Tables["Students"].Columns["StName"].AllowDBNull = false;
            ds.Tables["Students"].Columns["ProgId"].AllowDBNull = false;

            ds.Tables["Students"].PrimaryKey = new DataColumn[1]
            { ds.Tables["Students"].Columns["StId"]};

            ForeignKeyConstraint myFK = new ForeignKeyConstraint("MyFK",
                new DataColumn[] { ds.Tables["Programs"].Columns["ProgId"] },
                new DataColumn[] { ds.Tables["Students"].Columns["ProgId"] });

            myFK.DeleteRule = Rule.Cascade;
            myFK.UpdateRule = Rule.None;
            ds.Tables["Students"].Constraints.Add(myFK);
        }

        private static void loadEnrollments(DataSet ds)
        {
            adapterEnrollments.Fill(ds, "Enrollments");

            ds.Tables["Enrollments"].Columns["StId"].AllowDBNull = false;
            ds.Tables["Enrollments"].Columns["CId"].AllowDBNull = false;
            ds.Tables["Enrollments"].Columns["FinalGrade"].AllowDBNull = true;

            ds.Tables["Enrollments"].PrimaryKey = new DataColumn[2] {
                ds.Tables["Enrollments"].Columns["StId"],
                ds.Tables["Enrollments"].Columns["CId"]
            };

            ForeignKeyConstraint myFKSt = new ForeignKeyConstraint("MyFKST",
                new DataColumn[] { ds.Tables["Students"].Columns["StId"] },
                new DataColumn[] { ds.Tables["Enrollments"].Columns["StId"] });

            myFKSt.DeleteRule = Rule.None;
            myFKSt.UpdateRule = Rule.Cascade;
            ds.Tables["Enrollments"].Constraints.Add(myFKSt);

            ForeignKeyConstraint myFKC = new ForeignKeyConstraint("MyFKC",
             new DataColumn[] { ds.Tables["Courses"].Columns["CId"] },
             new DataColumn[] { ds.Tables["Enrollments"].Columns["CId"] });

            myFKSt.DeleteRule = Rule.None;
            myFKSt.UpdateRule = Rule.None;
            ds.Tables["Enrollments"].Constraints.Add(myFKC);
        }

        private static void loadCourses(DataSet ds)
        {
            adapterCourses.Fill(ds, "Courses");

            // Set AllowDBNull property to false for required columns
            ds.Tables["Courses"].Columns["CId"].AllowDBNull = false;
            ds.Tables["Courses"].Columns["CName"].AllowDBNull = false;
            ds.Tables["Courses"].Columns["ProgId"].AllowDBNull = false;

            // Set the primary key for the table
            ds.Tables["Courses"].PrimaryKey = new DataColumn[1]
                { ds.Tables["Courses"].Columns["CId"]};

            // Add foreign key constraint to ensure all courses belong to one and only one program
            ForeignKeyConstraint coursesProgFK = new ForeignKeyConstraint("CoursesProgFK",
                new DataColumn[] { ds.Tables["Programs"].Columns["ProgId"] },
                new DataColumn[] { ds.Tables["Courses"].Columns["ProgId"], });

            coursesProgFK.DeleteRule = Rule.Cascade;
            coursesProgFK.UpdateRule = Rule.Cascade;
            ds.Tables["Courses"].Constraints.Add(coursesProgFK);

        }

        internal static SqlDataAdapter getAdapterStudents()
        {
            return adapterStudents;
        }

        internal static SqlDataAdapter getAdapterEnrollments()
        {
            return adapterEnrollments;
        }

        internal static SqlDataAdapter getAdapterCourses()
        {
            return adapterCourses;
        }
        internal static SqlDataAdapter getAdapterPrograms()
        {
            return adapterPrograms;
        }
        internal static DataSet getDataSet()
        {
            return ds;
        }
        internal static void ReInitDS()
        {
            ds = InitDataSet();
        }
    }
    internal class Students
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterStudents();
        private static DataSet ds;

        internal static DataTable GetStudents()
        {
            ds = DataTables.getDataSet();
            return ds.Tables["Students"];
        }
        internal static int UpdateStudents()
        {
            ds = DataTables.getDataSet();
            if (!ds.Tables["Students"].HasErrors)
            {
                return adapter.Update(ds.Tables["Students"]);
            }
            else
            {
                return -1;
            }
        }
    }
    internal class Enrollments
    {

        private static SqlDataAdapter adapter = DataTables.getAdapterEnrollments();
        private static DataSet ds = DataTables.getDataSet();
        private static DataTable displayEnrl = null;

        internal static DataTable GetDisplayEnrl()
        {
            /* 
             * next line is needed to ensure "delete row"
             * due to the cascade are actually removed.
             */
            ds.Tables["Enrollments"].AcceptChanges();

            var query = (
                   from assign in ds.Tables["Enrollments"].AsEnumerable()
                   from st in ds.Tables["Students"].AsEnumerable()
                   from cr in ds.Tables["Courses"].AsEnumerable()
                   from pr in ds.Tables["Programs"].AsEnumerable()
                   where assign.Field<string>("StId") == st.Field<string>("StId")
                   where assign.Field<string>("CId") == cr.Field<string>("CId")
                   where cr.Field<string>("ProgId") == pr.Field<string>("ProgId")

                   select new
                   {
                       StId = st.Field<string>("StId"),
                       StName = st.Field<string>("StName"),
                       CId = cr.Field<string>("CId"),
                       CName = cr.Field<string>("CName"),
                       Pid = cr.Field<string>("ProgId"),
                       PName = pr.Field<string>("ProgName"),
                       FinalGrade = assign.Field<int?>("FinalGrade"),
                   });
            DataTable result = new DataTable();
            result.Columns.Add("StId", typeof(string));
            result.Columns.Add("StName");
            result.Columns.Add("CId", typeof(string));
            result.Columns.Add("CName");
            result.Columns.Add("FinalGrade");
            result.Columns.Add("ProgId");
            result.Columns.Add("ProgName");

            foreach (var x in query)
            {
                object[] allFields = { x.StId, x.StName, x.CId, x.CName, x.FinalGrade, x.Pid, x.PName };
                result.Rows.Add(allFields);
            }
            displayEnrl = result;
            return displayEnrl;
        }
        private static string ExecuteScalarQuery(string query, SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                return (string)command.ExecuteScalar();
            }
        }
    

        internal static int InsertData(string[] a)
        {
            var stId = a[0];
            var cId = a[1];

            using (SqlConnection connection = new SqlConnection(Connect.ConnectionString))
            {
                connection.Open();

                var stProgramIdQuery = $"SELECT ProgId FROM Students WHERE StId = '{stId}'";
                var stProgramId = ExecuteScalarQuery(stProgramIdQuery, connection);

                var courseProgramIdQuery = $"SELECT ProgId FROM Courses WHERE CId = '{cId}'";
                var courseProgramId = ExecuteScalarQuery(courseProgramIdQuery, connection);

                if (stProgramId != courseProgramId)
                {
                    MessageBox.Show("A student can only enroll in courses in the student's program");
                    return -1;
                }

                var test = (
                    from assign in ds.Tables["Enrollments"].AsEnumerable()
                    where assign.Field<string>("StId") == stId
                    where assign.Field<string>("CId") == cId
                    select assign);

                if (test.Count() > 0)
                {
                    MessageBox.Show("This Enrollment already exists");
                    return -1;
                }

                try
                {
                    DataRow line = ds.Tables["Enrollments"].NewRow();
                    line.SetField("StId", stId);
                    line.SetField("CId", cId);
                    ds.Tables["Enrollments"].Rows.Add(line);

                    adapter.Update(ds.Tables["Enrollments"]);

                    if (displayEnrl != null)
                    {
                        Console.WriteLine("In here");
                        var query = (
                            from st in ds.Tables["Students"].AsEnumerable()
                            from cr in ds.Tables["Courses"].AsEnumerable()
                            where st.Field<string>("StId") == stId
                            where cr.Field<string>("CId") == cId

                            select new
                            {
                                StId = st.Field<string>("StId"),
                                StName = st.Field<string>("StName"),
                                CId = cr.Field<string>("CId"),
                                CName = cr.Field<string>("CName"),

                            });

                        var q2 = (
                                from cr in ds.Tables["Courses"].AsEnumerable()
                                where cr.Field<string>("CId") == cId
                                select new
                                {
                                    CcID = cr.Field<string>("CId"),
                                    Pid = cr.Field<string>("ProgId")
                                });
                        var x = q2.Single();
                        var q3 = (
                                from pr in ds.Tables["Programs"].AsEnumerable()
                                where pr.Field<string>("ProgId") == x.Pid
                                select new
                                {
                                    Pname = pr.Field<string>("ProgName")
                                });
                        var y = q3.Single();

                        var r = query.Single();
                        displayEnrl.Rows.Add(new object[] { r.StId, r.StName, r.CId, r.CName, null, x.Pid, y.Pname });

                        adapter.Update(ds.Tables["Enrollments"]);
                        Data.DataTables.ReInitDS();
                    }
                    return 0;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    MessageBox.Show("Insertion / Update rejected");
                    return -1;
                }
            }
        }

        internal static int ManageFinalGrade(string[] a)
        {
            var test = (
                   from assign in ds.Tables["Enrollments"].AsEnumerable()
                   where assign.Field<string>("StId") == a[0]
                   where assign.Field<string>("CId") == a[1]
                   where assign.Field<string>("FinalGrade") == a[2]
                   select assign);
            if (test.Count() > 0)
            {
                Project_2210708.Form1.UIMessage("This Enrollment already exists");
                return -1;
            }
            try
            {
                DataRow line = ds.Tables["Enrollments"].NewRow();
                line.SetField("StId", a[0]);
                line.SetField("CId", a[1]);
                line.SetField("FinalGrade", a[2]);
                ds.Tables["Enrollments"].Rows.Add(line);

                adapter.Update(ds.Tables["Enrollments"]);

                if (displayEnrl == null)
                {
                    var query = (
                           from st in ds.Tables["Students"].AsEnumerable()
                           from cr in ds.Tables["Courses"].AsEnumerable()
                           from en in ds.Tables["Enrollments"].AsEnumerable()
                           where st.Field<string>("StId") == a[0]
                           where cr.Field<string>("CId") == a[1]
                           where en.Field<string>("FinalGrade") == a[2]
                           select new
                           {
                               StId = st.Field<string>("StId"),
                               StName = st.Field<string>("StName"),
                               CId = cr.Field<string>("CId"),
                               CName = cr.Field<string>("CName"),
                               FinalGrade = en.Field<string>("FinalGrade")
                           });
                    var r = query.Single();
                    displayEnrl.Rows.Add(new object[] { r.StId, r.StName, r.CId, r.CName, r.FinalGrade });
                }
                return 0;
            }
            catch (Exception)
            {
                Project_2210708.Form1.UIMessage("Insertion / Update rejected");
                return -1;
            }
        }

        internal static int UpdateData(string stid, string courseid, int? grade)
        {
            using (SqlConnection connection = new SqlConnection(Connect.ConnectionString))
            {
                connection.Open();

                Enrollments.adapter.UpdateCommand = new SqlCommand(
                    "UPDATE Enrollments SET FinalGrade = @FinalGrade WHERE StId = @StId AND CId = @CId",
                    connection);

                Enrollments.adapter.UpdateCommand.Parameters.AddWithValue("@StId", stid);
                Enrollments.adapter.UpdateCommand.Parameters.AddWithValue("@CId", courseid);
                Enrollments.adapter.UpdateCommand.Parameters.AddWithValue("@FinalGrade", (object)grade ?? DBNull.Value);

                int r = Enrollments.adapter.UpdateCommand.ExecuteNonQuery();

                Enrollments.adapter.Update(ds.Tables["Enrollments"]);
                Enrollments.adapter.Fill(ds.Tables["Enrollments"]);

                var q1 = (
                           from st in ds.Tables["Students"].AsEnumerable()
                           where st.Field<string>("StId") == stid
                           select new
                           {
                               Sname = st.Field<string>("StName")
                           }
                    );
                var z = q1.Single();

                var q2 = (
                            from cr in ds.Tables["Courses"].AsEnumerable()
                            where cr.Field<string>("CId") == courseid
                            select new
                            {
                                CName = cr.Field<string>("CName"),
                                Pid = cr.Field<string>("ProgId")
                            });
                var x = q2.Single();
                var q3 = (
                        from pr in ds.Tables["Programs"].AsEnumerable()
                        where pr.Field<string>("ProgId") == x.Pid
                        select new
                        {
                            Pname = pr.Field<string>("ProgName")
                        });
                var y = q3.Single();

                var abc = displayEnrl.AsEnumerable()
                        .Where(s => (s.Field<string>("StId") == stid && s.Field<string>("CId") == courseid))
                        .SingleOrDefault();

                if (abc != null || abc == null)
                {
                    displayEnrl.Rows.Remove(abc);
                }

                if (grade.HasValue)
                {
                    displayEnrl.Rows.Add(new object[] { stid, z.Sname, courseid, x.CName, grade, x.Pid, y.Pname });
                }
                else
                {
                    displayEnrl.Rows.Add(new object[] { stid, z.Sname, courseid, x.CName, DBNull.Value, x.Pid, y.Pname });
                }

                return r;
            }

        }

        internal static int DeleteData(List<string[]> lId)
        {
            try
            {
                
                foreach (var id in lId)
                {
                    var enrollment = ds.Tables["Enrollments"].AsEnumerable()
                        .FirstOrDefault(s => lId.Any(x => (x[0] == s.Field<string>("StId") && x[1] == s.Field<string>("CId"))));

                    if (enrollment != null)
                    {
                        if (enrollment.IsNull("FinalGrade"))
                        {
                            enrollment.Delete();
                        }
                        else
                        {
                            Project_2210708.Form1.UIMessage($"Cannot delete enrollment for student {id[0]} in course {id[1]} as final grade is assigned.");
                            GetDisplayEnrl();
                        }
                    }
                }
                if (displayEnrl != null)
                {
                    foreach (var p in lId)
                    {
                        var r = displayEnrl.AsEnumerable()
                                .Where(s => (s.Field<string>("StId") == p[0] && s.Field<string>("CId") == p[1]))
                                .Single();
                        displayEnrl.Rows.Remove(r);

                    }
                }
                adapter.Update(ds.Tables["Enrollments"]);


                return 0;
            }
            catch (Exception e)
            {
                Project_2210708.Form1.UIMessage(e.Message);
                return -1;
            }
        }
    }



    



    internal class Courses
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterCourses();
        private static DataSet ds;

        internal static DataTable GetCourses()
        {
            ds = DataTables.getDataSet();
            return ds.Tables["Courses"];
        }
        internal static int UpdateCourses()
        {
            ds = DataTables.getDataSet();
            if (!ds.Tables["Courses"].HasErrors)
            {
                return adapter.Update(ds.Tables["Courses"]);
            }
            else
            {
                return -1;
            }
        }
    }
    internal class Programs
    {
        private static SqlDataAdapter adapter = DataTables.getAdapterPrograms();
        private static DataSet ds;

        internal static DataTable GetPrograms()
        {
            ds = DataTables.getDataSet();
            return ds.Tables["Programs"];
        }
        internal static int UpdatePrograms()
        {
            ds = DataTables.getDataSet();
            if (!ds.Tables["Programs"].HasErrors)
            {
                return adapter.Update(ds.Tables["Programs"]);
            }
            else
            {
                return -1;
            }
        }
    }
}

