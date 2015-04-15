namespace RestDoc.Models.Data
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ApiParameter
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(4000)]
        public string Description { get; set; }
        [Required, MaxLength(100)]
        public string DataType { get; set; }
        public bool Required { get; set; }
        [MaxLength(150)]
        public string DefaultValue { get; set; }

        public Guid ApiVerbId { get; set; }
        [ForeignKey("ApiVerbId")]
        public virtual ApiVerb ApiVerb { get; set; }
    }
}