namespace TemplateWeb.Models.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class member_message
    {
        public int id { get; set; }

        public int? member_id { get; set; }

        public string title { get; set; }

        public string content { get; set; }

        public bool? state_read { get; set; }

        public DateTime? sys_datetime { get; set; }
    }
}
