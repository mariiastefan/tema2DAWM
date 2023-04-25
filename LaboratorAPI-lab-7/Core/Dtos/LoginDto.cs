using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class LoginDto
    {
        [Required, MaxLength(100)]
        public string Username { get; set; }

        [Required, MaxLength(100)]
        public string Password { get; set; }
    }
}
