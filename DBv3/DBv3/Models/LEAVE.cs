//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBv3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class LEAVE
    {
        public int NO { get; set; }
        public long EMID { get; set; }

        [Required]
        public System.DateTime DATELEAVE { get; set; }

        [Required]
        [StringLength(10,MinimumLength =5)]
        public string TIME { get; set; }

        [Required]
        public string DETAIL { get; set; }
        public string STATUS { get; set; }
    
        public virtual PERSON PERSON { get; set; }
    }
}