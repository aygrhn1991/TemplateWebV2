namespace TemplateWeb.Models.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class divide_grade
    {
        public int id { get; set; }

        public string name { get; set; }

        public DateTime? sys_datetime { get; set; }
    }
}
