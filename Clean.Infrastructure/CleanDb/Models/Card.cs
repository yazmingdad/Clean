﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Infrastructure.CleanDb.Models
{
    public class Card
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //[Required]
        public int EmployeeId { get; set; }

        //[ForeignKey("EmployeeId")]
        //public virtual Employee Employee { get; set; }

        //[Required]
        //[Column(TypeName = "nvarchar(40)")]
        public string Number { get; set; }

    }
}
