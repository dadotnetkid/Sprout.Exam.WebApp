using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Sprout.Exam.Application.Interfaces;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Domain.Entities;
using Sprout.Exam.Domain.Models.InputModels;
using EmployeeType = Sprout.Exam.Common.Enums.EmployeeType;

namespace Sprout.Exam.WebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepository<Employee, int> _employeeRepo;
        private readonly IEmployeeService _employeeService;
        private readonly IEmployeeSalaryCalculationFactory _employeeSalaryCalculationFactory;

        public EmployeesController(IRepository<Employee, int> employeeRepo, IEmployeeService employeeService, IEmployeeSalaryCalculationFactory employeeSalaryCalculationFactory)
        {
            _employeeRepo = employeeRepo;
            _employeeService = employeeService;
            _employeeSalaryCalculationFactory = employeeSalaryCalculationFactory;
        }
        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _employeeRepo.GetAllAsync("EmployeeType");
            return Ok(result.Select(x => new EmployeeDto()
            {
                Birthdate = x.Birthdate.ToString("yyyy-MM-dd"),
                FullName = x.FullName,
                Id = x.Id,
                Tin = x.Tin,
                TypeId = x.EmployeeTypeId,
                EmployeeType = x.EmployeeType.TypeName
            }));
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _employeeRepo.FindAsync(id);
            if (result == null) return NotFound();
            var dto = new EmployeeDto()
            {
                Birthdate = result.Birthdate.ToString("yyyy-MM-dd"),
                FullName = result.FullName,
                Id = result.Id,
                Tin = result.Tin,
                TypeId = result.EmployeeTypeId

            };
            return Ok(dto);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and update changes to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(EditEmployeeDto input)
        {
            var item = await _employeeService.Update(input);
            if (item == null) return NotFound();

            return Ok(item);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and insert employees to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateEmployeeDto input)
        {

            var item = await _employeeService.Insert(input);
            return Created($"/api/employees/{item.Id}", item.Id);
        }


        /// <summary>
        /// Refactor this method to go through proper layers and perform soft deletion of an employee to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _employeeService.Delete(id);
            if (result == false) return NotFound();
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
        public async Task<IActionResult> Calculate(CalculateEmployeeSalaryModel model)
        {
            var result = _employeeRepo.Exist(x => x.Id == model.EmployeeId);
            if (!result) return NotFound();
            var income = await _employeeSalaryCalculationFactory.Calculate(model);
            return Ok(income);
        }

    }
}
