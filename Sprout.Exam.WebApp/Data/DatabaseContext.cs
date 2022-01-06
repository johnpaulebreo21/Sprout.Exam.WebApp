using Microsoft.EntityFrameworkCore;
using Sprout.Exam.Business.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.DataAccess
{
    public class DatabaseContext  :  DbContext 
    {

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
        public DbSet<EmployeeDto> Employee { get; set; }
    }
}
