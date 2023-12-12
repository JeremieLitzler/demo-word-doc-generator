using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeremie.Codes.Core.WordGenerator.Dto
{
    public class ProcessedDocumentOutput
    {
        public byte[]? NewContent { get; set; }
        public List<string>? ReplacementWarnings { get; set; }
    }
}
