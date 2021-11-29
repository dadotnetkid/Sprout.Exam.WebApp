using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Sprout.Exam.Common.Enums;


namespace Sprout.Exam.Domain.Models.InputModels
{
    public class CalculateEmployeeSalaryModel
    {
        [JsonPropertyName("id")]
        public int EmployeeId { get; set; }
        public decimal MonthlySalary { get; set; }
        public decimal AbsentDays { get; set; }
        public decimal RatePerDay { get; set; }
        public decimal WorkedDays { get; set; }

    }
}
