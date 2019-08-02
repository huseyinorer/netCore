﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace core2.Model.Security
{
    public class ResetPasswordViewModel
    {
       
        public string Code { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Passwors { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
