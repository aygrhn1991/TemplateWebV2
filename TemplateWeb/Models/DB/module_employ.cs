namespace TemplateWeb.Models.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class module_employ
    {
        public int id { get; set; }

        public int? type_id { get; set; }

        public string position_name { get; set; }

        public string salary { get; set; }

        public string education { get; set; }

        public string experience { get; set; }

        public string work_place { get; set; }

        public int? employ_number { get; set; }

        public string position_description_1 { get; set; }

        public string position_description_2 { get; set; }

        public string position_description_3 { get; set; }

        public string position_description_4 { get; set; }

        public string position_requirement_1 { get; set; }

        public string position_requirement_2 { get; set; }

        public string position_requirement_3 { get; set; }

        public string position_requirement_4 { get; set; }

        public string benefit { get; set; }

        public string remark { get; set; }

        public DateTime? sys_datetime { get; set; }
    }
}
