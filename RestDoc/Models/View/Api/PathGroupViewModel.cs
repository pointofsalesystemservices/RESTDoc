namespace RestDoc.Models.View.Api
{
    using System;
    using System.Collections.Generic;

    public class PathGroupViewModel
    {
        public Guid Id { get; set; }
        public string BasePath { get; set; }
        public string Description { get; set; }

        public ObjectDescriptionViewModel RestApi { get; set; }

        public virtual List<ObjectDescriptionViewModel> ApiPaths { get; set; }
    }
}