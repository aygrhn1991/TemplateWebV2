namespace TemplateWeb.Models.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class module_course
    {
        public int id { get; set; }

        public int? grade_id { get; set; }

        public int? subject_id { get; set; }

        public int? edition_id { get; set; }

        public int? price { get; set; }

        public string name { get; set; }

        public string description { get; set; }

        public string content { get; set; }

        public string cover_img_url { get; set; }

        public string attachment_url { get; set; }

        public bool? top { get; set; }

        public bool? delete { get; set; }

        public DateTime? sysdatetime { get; set; }
    }
}
