namespace RestDoc.Models.Data
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ApiStatusCode
    {
       

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public int StatusCode { get; set; }
        [MaxLength(300), Required]
        public string Description { get; set; }

        public Guid ApiVerbId { get; set; }
        [ForeignKey("ApiVerbId")]
        public ApiVerb ApiVerb { get; set; }
    }
}