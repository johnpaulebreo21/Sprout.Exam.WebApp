using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.WebApp.Models;
using Sprout.Exam.DataAccess;

namespace Sprout.Exam.WebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {


        protected DatabaseContext db;

        public EmployeesController(DatabaseContext _db)
        {
            db = _db;
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //var result = await Task.FromResult(StaticEmployees.ResultList);
             var result = await Task.FromResult(db.Employee.ToList()); 
          // var result = await Task.FromResult(db.Employee.Where(e => e.IsDeleted == false).ToList());
            return Ok(result);
             

        }
        [HttpGet("{id}/getallactive")]
        public async Task<IActionResult> GetAllActive()
        {
            //var result = await Task.FromResult(StaticEmployees.ResultList);
            var result = await Task.FromResult(db.Employee.Where(e => e.IsDeleted == false).ToList());
            return Ok(result); 
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            //var result = await Task.FromResult(StaticEmployees.ResultList.FirstOrDefault(m => m.Id == id));
            var result = await Task.FromResult(db.Employee.FirstOrDefault(m => m.Id == id));
            return Ok(result);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and update changes to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(EditEmployeeDto input)
        {
            //var item = await Task.FromResult(StaticEmployees.ResultList.FirstOrDefault(m => m.Id == input.Id));
            var item = await Task.FromResult(db.Employee.FirstOrDefault(m => m.Id == input.Id));
            if (item == null) return NotFound();
            item.FullName = input.FullName;
            item.Tin = input.Tin;
            item.Birthdate = input.Birthdate;
            item.TypeId = input.TypeId;

            db.Employee.Update(item);
            db.SaveChanges();
            return Ok(item);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and insert employees to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateEmployeeDto input)
        {

            //var id = await Task.FromResult(StaticEmployees.ResultList.Max(m => m.Id) + 1);

            //StaticEmployees.ResultList.Add(new EmployeeDto
            //{
            //    Birthdate = input.Birthdate.ToString("yyyy-MM-dd"),
            //    FullName = input.FullName,
            //    Id = id,
            //    Tin = input.Tin,
            //    TypeId = input.TypeId
            //});

            var id = await Task.FromResult(db.Employee.Max( e => (int?)e.Id).GetValueOrDefault(0) + 1);


            var emp = new EmployeeDto() {
              //  Id = (int)id,
                Birthdate = input.Birthdate,
                FullName = input.FullName,
                Tin = input.Tin,
                TypeId = input.TypeId,
                IsDeleted = false

            };

            db.Employee.Add(emp);
            db.SaveChanges();
            return Created($"/api/employees/{id}", id);
        }


        /// <summary>
        /// Refactor this method to go through proper layers and perform soft deletion of an employee to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            //var result = await Task.FromResult(StaticEmployees.ResultList.FirstOrDefault(m => m.Id == id));
            var result = await Task.FromResult(db.Employee.FirstOrDefault(m => m.Id == id));
            if (result == null) return NotFound();
            //StaticEmployees.ResultList.RemoveAll(m => m.Id == id);
            result.IsDeleted = true;
            db.Employee.Update(result);
            db.SaveChanges();
            return Ok(id);
        }



        /// <summary>
        /// Refactor this method to go through proper layers and use Factory pattern
        /// </summary>
        /// <param name="id"></param>
        /// <param name="absentDays"></param>
        /// <param name="workedDays"></param>
        /// <returns></returns>
        [HttpPost("{id}/calculate")]
        public async Task<IActionResult> Calculate(int id,[FromBody]Employee employee)
        {
            //var result = await Task.FromResult(StaticEmployees.ResultList.FirstOrDefault(m => m.Id == id));
            var result = await Task.FromResult(db.Employee.FirstOrDefault(m => m.Id == id));
            if (result == null) return NotFound();
            var type = (EmployeeType)result.TypeId;


            if (type == EmployeeType.Regular)
            {
                //• Has 20,000 basic monthly salary
                //• 1 day absent
                //• 12 % tax
                //• = 20,000 - (20, 000 / 22) - (20, 000 * 0.12)
                //• = 16,690.91
                var salary = 20000;
                var absentDeduction = (salary / 22) * employee.absentDays;
                var taxDeduction = salary * .12;
                var netSalary = (decimal)salary - absentDeduction - (decimal)taxDeduction; 
                return Ok(String.Format("{0:0.00}",netSalary));
            }
            else if (type == EmployeeType.Contractual)
            {
                //• Has 500 per day rate
                //• Reported to work for 15.5 days
                //• = 500 * 15.5
                //• = 7, 750.00
                var salary = 500;
                var netSalary = (decimal)salary * employee.workedDays;
                return Ok(String.Format("{0:0.00}", netSalary));
            }
            else {
                return NotFound("Employee Type not found");
            }
             


        }
         
    }

  
}
 