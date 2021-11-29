using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sprout.Exam.Application.Factories;
using Sprout.Exam.Application.Repositories;
using Sprout.Exam.Domain.Entities;
using Xunit;

namespace Sprout.Exam.Tests
{
    public class SalaryFactoryTest
    {
        private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;
        OperationalStoreOptions storeOptions = new OperationalStoreOptions
        {
            //populate needed members
        };

        private Repository<Employee, int> _repo;
        private EmployeeSalaryCalculationFactory _factory;

        public SalaryFactoryTest()
        {
            var cons =
                "Data Source=localhost;Initial Catalog=SproutExamDb;Integrated Security=True;MultipleActiveResultSets=True";
            optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(cons);
            _repo = new Repository<Employee, int>(new ApplicationDbContext(optionsBuilder.Options, Options.Create(storeOptions)));
            _factory = new EmployeeSalaryCalculationFactory(_repo);

        }
        [Fact]
        public async Task Ensure_Factory_Regular_Employee_Get_Error_WhenMonthlyIsZero()
        {
            Func<Task> _act = () => _factory.Calculate(new Domain.Models.InputModels.CalculateEmployeeSalaryModel()
            {
                EmployeeId = 2
            });
            var res = await Assert.ThrowsAnyAsync<ArgumentNullException>(_act);
            Assert.Equal(new ArgumentNullException("MonthlySalary").Message, res.Message);
        }
        [Fact]
        public async Task Ensure_Factory_Regular_Employee_Should_Return16Thousand()
        {
            var res = await _factory.Calculate(new Domain.Models.InputModels.CalculateEmployeeSalaryModel()
            {
                EmployeeId = 2,
                MonthlySalary = 20000,
                AbsentDays = 1
            });
            Assert.Equal(16690.91M, res);

        }
        [Fact]
        public async Task Ensure_Factory_Contractual_Employee_Get_Error_WhenRatePerDayIsZero()
        {
            Func<Task> _act = () => _factory.Calculate(new Domain.Models.InputModels.CalculateEmployeeSalaryModel()
            {
                EmployeeId = 1
            });
            var res = await Assert.ThrowsAnyAsync<ArgumentNullException>(_act);

            Assert.Equal(new ArgumentNullException("RatePerDay").Message, res.Message);
        }
        [Fact]
        public async Task Ensure_Factory_Regular_Employee_Should_Return7Thousand()
        {
            var res = await _factory.Calculate(new Domain.Models.InputModels.CalculateEmployeeSalaryModel()
            {
                EmployeeId = 1,
                RatePerDay = 500,
                WorkedDays = 15.5M
            });
            Assert.Equal(7750M, res);

        }
    }
}
