namespace RestDoc.Models.View.Api
{
    using System;
    using System.Collections.Generic;

    public class PathViewModel
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        
        public ObjectDescriptionViewModel PathGroup { get; set; }

        public virtual List<ObjectDescriptionViewModel> Verbs { get; set; }
    }
}