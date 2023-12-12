using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeremie.Codes.Core.WordGenerator.Validators
{
    /// <summary>
    /// Inspired by <see href="https://tech.trailmax.info/2014/04/validating-of-openxml-generated-documents-or-the-file-cannot-be-opened-because-there-are-problems-with-contents/"/>.
    /// </summary>
    internal static class DocxValidator
    {
        /// <summary>
        /// Validate gracefully the Word document.
        /// </summary>
        /// <param name="doc">The document to validate</param>
        /// <param name="warnings">The list of warnings</param>
        /// <returns>True if validation returns no error, False otherwise.</returns>
        internal static bool ValidateWordDocument(WordprocessingDocument doc, List<string> warnings)
        {
            try
            {
                var validator = new OpenXmlValidator();
                var validationErrors = validator.Validate(doc).ToList();
                if (!validationErrors.Any())
                {
                    return true;
                }
                
                foreach (var error in validationErrors)
                {
                    var validationErrorDetailBuilder = new StringBuilder();
                    validationErrorDetailBuilder.Append("Description: ").Append(error.Description).Append(" ; ");
                    validationErrorDetailBuilder.Append("ErrorType: ").Append(error.ErrorType).Append(" ; ");
                    validationErrorDetailBuilder.Append("Node: ").Append(error?.Node).Append(" ; ");
                    validationErrorDetailBuilder.Append("Path: ").Append(error?.Path?.XPath).Append(" ; ");
                    validationErrorDetailBuilder.Append("Part: ").Append(error?.Part?.Uri).Append(" ; ");
                    if (error?.RelatedNode != null)
                    {
                        validationErrorDetailBuilder.Append("Related Node: ").Append(error.RelatedNode).Append(" ; ");
                        validationErrorDetailBuilder.Append("Related Node Inner Text: ").Append(error.RelatedNode.InnerText).Append(" ; ");
                    }
                }

            }
            catch (Exception ex)
            {
                warnings.Add($"Failed validating the document: the error was <<{ex.Message}>> with the StackTrace: <<{ex.StackTrace}>>");
                return false;
            }
            return false;
        }
    }
}
