using Sprout.Exam.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprout.Exam.Domain.Entities;
using Sprout.Exam.Domain.Models.InputModels;
using EmployeeType = Sprout.Exam.Common.Enums.EmployeeType;

namespace Sprout.Exam.Application.Factories
{
    public class EmployeeSalaryCalculationFactory : IEmployeeSalaryCalculationFactory
    {
        private readonly IRepository<Employee, int> _employeeRepo;

        public EmployeeSalaryCalculationFactory(IRepository<Employee, int> employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }
        public async Task<decimal> Calculate(CalculateEmployeeSalaryModel model)
        {
            var employee = await _employeeRepo.FindAsync(x => x.Id == model.EmployeeId);
            switch (employee.EmployeeTypeId)
            {
                case 1:
                    return CalculateRegularEmployee(model);
                case 2:
                    return CalculateContractualEmployee(model);
                default:
                    return 0.0M;
            }
        }

        private decimal CalculateContractualEmployee(CalculateEmployeeSalaryModel model)
        {

            if (model.RatePerDay == 0)
                throw new ArgumentNullException(nameof(model.RatePerDay));
            var result = model.RatePerDay * model.WorkedDays;
            return Convert.ToDecimal(result.ToString("n2"));
        }

        private decimal CalculateRegularEmployee(CalculateEmployeeSalaryModel model)
        {
            if (model.MonthlySalary == 0)
                throw new ArgumentNullException(nameof(model.MonthlySalary));
            var workPerDay = (model.MonthlySalary / 22);
            var result = model.MonthlySalary - (workPerDay * model.AbsentDays) - (model.MonthlySalary * 0.12M);
            return Convert.ToDecimal(result.ToString("n2"));
        }
    }
}
