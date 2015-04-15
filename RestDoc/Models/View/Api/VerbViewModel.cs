namespace RestDoc.Models.View.Api
{
    using System;
    using System.Collections.Generic;

    public class VerbViewModel
    {
        public Guid Id { get; set; }
        public string Verb { get; set; }
        public virtual List<ObjectDescriptionViewModel> Parameters { get; set; }
        public virtual List<ObjectDescriptionViewModel> StatusCodes { get; set; }
        public BodyViewModel RequestBody { get; set; }
        public BodyViewModel ResponseBody { get; set; }
        public ObjectDescriptionViewModel Path { get; set; }
    }

    public class BodyViewModel
    {
        public Guid Id { get; set; }

        public string Example { get; set; }
        public string Description { get; set; }

    }
}