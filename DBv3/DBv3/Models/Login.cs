using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DBv3.Models
{
    public class Login
    {
        [Required(ErrorMessage ="Please Enter Username")]
        public string username { get; set; }
       
        [Required(ErrorMessage ="Please Enter Password")]
        public string password { get; set; }
    }
}