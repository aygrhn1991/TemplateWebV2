namespace TemplateWeb.Models.DB
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class EntityDB : DbContext
    {
        public EntityDB()
            : base("name=EntityDB")
        {
        }

        public virtual DbSet<account_admin> account_admin { get; set; }
        public virtual DbSet<account_code> account_code { get; set; }
        public virtual DbSet<account_member> account_member { get; set; }
        public virtual DbSet<lay_banner> lay_banner { get; set; }
        public virtual DbSet<lay_link_link> lay_link_link { get; set; }
        public virtual DbSet<lay_link_sublink> lay_link_sublink { get; set; }
        public virtual DbSet<lay_nav_nav> lay_nav_nav { get; set; }
        public virtual DbSet<lay_nav_subnav> lay_nav_subnav { get; set; }
        public virtual DbSet<lay_notice> lay_notice { get; set; }
        public virtual DbSet<lay_page> lay_page { get; set; }
        public virtual DbSet<lay_partner> lay_partner { get; set; }
        public virtual DbSet<lay_setting> lay_setting { get; set; }
        public virtual DbSet<member_message> member_message { get; set; }
        public virtual DbSet<module_employ> module_employ { get; set; }
        public virtual DbSet<module_employ_type> module_employ_type { get; set; }
        public virtual DbSet<module_messageaboard> module_messageaboard { get; set; }
        public virtual DbSet<module_news> module_news { get; set; }
        public virtual DbSet<module_news_type> module_news_type { get; set; }
        public virtual DbSet<module_product> module_product { get; set; }
        public virtual DbSet<module_product_type> module_product_type { get; set; }
        public virtual DbSet<pay_order> pay_order { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<module_product>()
                .Property(e => e.price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<pay_order>()
                .Property(e => e.price)
                .HasPrecision(19, 4);
        }
    }
}
