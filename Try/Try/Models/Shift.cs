using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Web;
namespace Try.Models
{
    public class Shift
    {
        public int id { get; set; }
        public string date { get; set; }
        public string sunday { get; set; }
        public string monday { get; set; }
        public string tuesday { get; set; }
        public string wednesday { get; set; }
        public string thursday { get; set; }
        public string friday { get; set; }
        public string saturday { get; set; }
    }
}
