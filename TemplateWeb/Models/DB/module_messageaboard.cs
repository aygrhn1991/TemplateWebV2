namespace TemplateWeb.Models.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class module_messageaboard
    {
        public int id { get; set; }

        public string contact_name { get; set; }

        public string contact_phone { get; set; }

        public string contact_other { get; set; }

        public string content { get; set; }

        public bool? state_mark { get; set; }

        public bool? state_read { get; set; }

        public bool? state_solve { get; set; }

        public DateTime? sys_datetime { get; set; }
    }
}
