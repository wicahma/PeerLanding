using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Req
{
    public class ReqLoginUserDto
    {
        [Required(ErrorMessage = "Email is required")]
        [MaxLength(50, ErrorMessage = "Email can't be more than 50 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [MaxLength(50, ErrorMessage = "Password can't be more than 50 characters")]
        public string Password { get; set; }
     }
}