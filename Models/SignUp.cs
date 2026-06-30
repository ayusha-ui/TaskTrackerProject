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

        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Address { get; set; }
        public required string Phone { get; set; }
        public required string DOB { get; set; }
    }
}