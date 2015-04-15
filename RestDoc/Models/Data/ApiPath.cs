namespace RestDoc.Models.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ApiPath
    {
        public ApiPath()
        {
            ApiVerbs = new List<ApiVerb>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [MaxLength(400), Required]
        public string Path { get; set; }
        public string Description { get; set; }

        public Guid ApiPathGroupId { get; set; }
        [ForeignKey("ApiPathGroupId")]
        public virtual ApiPathGroup ApiPathGroup { get; set; }

        public virtual List<ApiVerb> ApiVerbs { get; set; }

    }
}