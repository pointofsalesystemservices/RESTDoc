using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestDoc.Models.Data
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Net;
    using System.Threading;

    public class RestApi
    {
        public RestApi()
        {
            PathGroups = new List<ApiPathGroup>();
            Created = DateTimeOffset.Now;
            LastModified = DateTimeOffset.Now;
            Creator = Thread.CurrentPrincipal != null && Thread.CurrentPrincipal.Identity != null &&
                      Thread.CurrentPrincipal.Identity.IsAuthenticated
                          ? Thread.CurrentPrincipal.Identity.Name
                          : null;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [MaxLength(400), Required]
        public string Name { get; set; }
        [MaxLength(4000)]
        public string Description { get; set; }
        public string Creator { get; set; }
        public string Version { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset LastModified { get; set; }

        public virtual List<ApiPathGroup> PathGroups { get; set; }
    }
}
