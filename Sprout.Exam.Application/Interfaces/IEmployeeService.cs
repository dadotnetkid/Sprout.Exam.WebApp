using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Domain.Entities;

namespace Sprout.Exam.Application.Interfaces
{
    public interface IEmployeeService
    {
        public Task<Employee> Insert(CreateEmployeeDto input);
        public Task<Employee> Update(EditEmployeeDto input);
        public Task<bool> Delete(int id);
    }
}
