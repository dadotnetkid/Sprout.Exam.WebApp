using System;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sprout.Exam.Application.Repositories;
using Sprout.Exam.Application.Services;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Domain.Entities;
using Xunit;

namespace Sprout.Exam.Tests
{
    public class CRUDTest
    {
        private DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder;
        OperationalStoreOptions storeOptions = new OperationalStoreOptions
        {
            //populate needed members
        };
        public CRUDTest()
        {
            optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase("SproutExamDb");
        }
        [Fact]
        public async Task Ensure_Insert_Employee_Get_Error_When_FullNameIsEmpty()
        {

            var repo = new Repository<Employee, int>(new ApplicationDbContext(optionsBuilder.Options, Options.Create(storeOptions)));
            var service = new EmployeeService(repo);
            var dto = new CreateEmployeeDto()
            {
                FullName = "",

            };
            Func<Task> _act = () => service.Insert(dto);
            var res = await Assert.ThrowsAnyAsync<ArgumentNullException>(_act);
            Assert.Equal(new ArgumentNullException(nameof(dto.FullName)).Message, res.Message);
        }
      
        [Fact]
        public async Task Ensure_Delete_Employee_Get_Error_When_IdIsNull()
        {

            var repo = new Repository<Employee, int>(new ApplicationDbContext(optionsBuilder.Options, Options.Create(storeOptions)));
            var service = new EmployeeService(repo);
            Func<Task> _act = () => service.Delete(0);
            var res = await Assert.ThrowsAnyAsync<ArgumentNullException>(_act);
            Assert.Equal(new ArgumentNullException("id").Message, res.Message);
        }
        [Fact]
        public async Task Ensure_Update_Employee_Get_Error_When_ItemIsNull()
        {

            var repo = new Repository<Employee, int>(new ApplicationDbContext(optionsBuilder.Options, Options.Create(storeOptions)));
            var service = new EmployeeService(repo);
            Func<Task> _act = () => service.Update(new EditEmployeeDto() { Id = 0 });
            var res = await Assert.ThrowsAnyAsync<ArgumentNullException>(_act);
            Assert.Equal(new ArgumentNullException("item").Message, res.Message);
        }
        [Fact]
        public async Task Ensure_Delete_Employee_ReturnFalseWhenNoRecordFound()
        {

            var repo = new Repository<Employee, int>(new ApplicationDbContext(optionsBuilder.Options, Options.Create(storeOptions)));
            var service = new EmployeeService(repo);
            var res = await service.Delete(1234);
            Assert.False(res);
        }
    }
}
