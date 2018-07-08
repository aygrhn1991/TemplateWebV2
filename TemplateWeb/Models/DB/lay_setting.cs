namespace TemplateWeb.Models.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class lay_setting
    {
        public int id { get; set; }

        public string key { get; set; }

        public string value { get; set; }
    }
}
