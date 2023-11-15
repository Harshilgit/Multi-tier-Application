using Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    internal class Programs
    {
        internal static int UpdatePrograms()
        {
            return Data.Programs.UpdatePrograms();
        }
    }

    internal class Courses
    {
        internal static int UpdateCourses()
        {
            return Data.Courses.UpdateCourses();
        }
    }

    internal class Students
    {
        internal static int UpdateStudents()
        {
            return Data.Students.UpdateStudents();
        }
    }

    internal class Enrollments
    {
        internal static int UpdateEnr(string table_enr, string stid, string courseid, int? grade)
        {
            return Data.Enrollments.UpdateData(stid, courseid, grade);
        }
    }

    

}
