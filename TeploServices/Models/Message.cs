namespace TeploServices.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Message
    {
        public int MessageId { get; set; }

        [StringLength(50)]
        public string SNP { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public string Text { get; set; }

        public int? ServiceId { get; set; }

        [StringLength(50)]
        public string CallTime { get; set; }

        [StringLength(255)]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        public virtual Service Service { get; set; }

    }
}
