using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Web;
namespace Try.Models
{
    public class EmployeeShift
    {
        public int ID { get; set; }
        public string shift { get; set; }
        public string date { get; set; }
        public string day { get; set; }
        public string Name { get; set; }
        
    }
}
