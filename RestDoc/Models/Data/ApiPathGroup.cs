namespace RestDoc.Models.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ApiPathGroup
    {
        public ApiPathGroup()
        {
            ApiPaths = new List<ApiPath>();
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required, MaxLength(400)]
        public string BasePath { get; set; }
        [MaxLength(4000)]
        public string Description { get; set; }

        public virtual List<ApiPath> ApiPaths { get; set; }

        public Guid RestApiId { get; set; }
        [ForeignKey("RestApiId")]
        public virtual RestApi RestApi { get; set; }
    }
}