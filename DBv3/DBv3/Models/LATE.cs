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
    
    public partial class LATE
    {
        public int NO { get; set; }
        public long EMID { get; set; }
        public System.DateTime DATELATE { get; set; }
        public string TIMEATTENDANCE { get; set; }
    
        public virtual PERSON PERSON { get; set; }
    }
}
