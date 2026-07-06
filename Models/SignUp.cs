using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskTrackerProject.Models
{
    public class SignUp
    {
        [Key]
        public Guid Id { get; set; }

        public  string Name { get; set; }
        public  string Email { get; set; }
        public  string Address { get; set; }
        public  string Phone { get; set; }
        public DateTime DOB { get; set; }
    }
}
