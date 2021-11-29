using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.Domain.Models.InputModels;

namespace Sprout.Exam.Application.Interfaces
{
    public interface IEmployeeSalaryCalculationFactory
    {
        public Task<decimal> Calculate(CalculateEmployeeSalaryModel model);
    }
}
