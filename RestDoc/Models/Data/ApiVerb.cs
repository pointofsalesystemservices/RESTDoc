namespace RestDoc.Models.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ApiVerb
    {
        public ApiVerb()
        {
            Parameters = new List<ApiParameter>();
            ApiStatusCodes = new List<ApiStatusCode>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required, MaxLength(15)]
        public string Verb { get; set; }
        public virtual List<ApiParameter> Parameters { get; set; }
        public virtual List<ApiStatusCode> ApiStatusCodes { get; set; } 

        public Guid? RequestBodyId { get; set; }
        [ForeignKey("RequestBodyId")]
        public virtual ApiBody RequestBody { get; set; }

        public Guid ResponseBodyId { get; set; }
        [ForeignKey("ResponseBodyId")]
        public virtual ApiBody ResponseBody { get; set; }

        public Guid ApiPathId { get; set; }
        [ForeignKey("ApiPathId")]
        public virtual ApiPath ApiPath { get; set; }
    }
}