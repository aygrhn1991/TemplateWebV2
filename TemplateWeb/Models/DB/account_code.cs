namespace TemplateWeb.Models.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class account_code
    {
        [Key]
        [StringLength(11)]
        public string phone { get; set; }

        [StringLength(4)]
        public string code { get; set; }

        public DateTime? sys_datetime { get; set; }
    }
}
