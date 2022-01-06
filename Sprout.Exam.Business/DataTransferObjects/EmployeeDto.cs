using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sprout.Exam.Business.DataTransferObjects
{
    [Table("Employee")]
    public class EmployeeDto
    {
        [Key]
        public int Id { get; set; }

        [Column("FullName", TypeName = "varchar(100)")]
        public string FullName { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Birthdate { get; set; }

        [Column("Tin", TypeName = "varchar(100)")]
        public string Tin { get; set; }
       
        [Column("EmployeeTypeId", TypeName = "int")]
        public int TypeId { get; set; }

        //[Column("IsDeleted", TypeName = "bit")]
        public bool IsDeleted { get; set; }
    }
}
