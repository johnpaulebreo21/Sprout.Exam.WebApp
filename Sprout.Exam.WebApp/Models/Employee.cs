﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp.Models
{
    public class Employee
    {
        public int id { get; set; }

        public string fullname { get; set; }

        public string birthdate { get; set; }

        public string typeId { get; set; }
        public decimal absentDays { get; set; }
        public decimal workedDays { get; set; }


    }
}
