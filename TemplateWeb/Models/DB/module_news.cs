namespace TemplateWeb.Models.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class module_news
    {
        public int id { get; set; }

        public int? type_id { get; set; }

        public string title { get; set; }

        public string author { get; set; }

        public DateTime? datetime { get; set; }

        public string path { get; set; }

        public string description { get; set; }

        public string content { get; set; }

        public bool? top { get; set; }

        public int? views { get; set; }

        public DateTime? sys_datetime { get; set; }
    }
}
