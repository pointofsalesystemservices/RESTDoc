using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestDoc.Models.View.Api
{
    public class RestApiViewModel
    {
        public RestApiViewModel()
        {
            PathGroups = new List<ObjectDescriptionViewModel>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Creator { get; set; }
        public string Version { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset LastModified { get; set; }
        public List<ObjectDescriptionViewModel> PathGroups { get; set; }
    }
}
