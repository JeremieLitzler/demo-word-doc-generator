using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeremie.Codes.Core.WordGenerator.Dto
{
    public class ProcessedDocumentInputs
    {
        public string? AbsolutePathTemplateDocument { get; set; }
        /// <summary>
        /// Define the list of placeholder (Key) and value to use instead of the placeholder (Value).
        /// </summary>
        public Dictionary<string, string>? InputKeyValues { get; set; }
    }
}
