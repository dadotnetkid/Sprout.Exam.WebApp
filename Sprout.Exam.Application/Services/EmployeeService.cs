using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprout.Exam.Application.Interfaces;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Domain.Entities;

namespace Sprout.Exam.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee, int> _employeeRepo;

        public EmployeeService(IRepository<Employee, int> employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }
        public async Task<Employee> Insert(CreateEmployeeDto input)
        {
            if (string.IsNullOrEmpty(input.FullName))
            {
                throw new ArgumentNullException(nameof(input.FullName));
            }
            var item = await _employeeRepo.InsertAsync(new Employee()
            {
                Birthdate = input.Birthdate,
                FullName = input.FullName,
                Tin = input.Tin,
                EmployeeTypeId = input.TypeId
            });
            return item;
        }

        public async Task<Employee> Update(EditEmployeeDto input)
        {
            var item = await _employeeRepo.FindAsync(x => x.Id == input.Id);
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            item.FullName = input.FullName;
            item.Tin = input.Tin;
            item.Birthdate = input.Birthdate;
            item.EmployeeTypeId = input.TypeId;
            await _employeeRepo.UpdateAsync(item);
            return item;
        }

        public async Task<bool> Delete(int id)
        {
            if (id == 0)
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (!_employeeRepo.Exist(x => x.Id == id))
                return false;
            return await _employeeRepo.DeleteAsync(id);
        }
    }
}
