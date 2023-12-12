using Jeremie.Codes.Core.WordGenerator.Dto;

namespace Jeremie.Codes.Core.WordGenerator.Interfaces
{
    public interface IWordDocumentProcessorService
    {
        /// <summary>
        /// Replace placeholders using a KeyValue pair dictionary.
        /// </summary>
        /// <param name="inputs">The object containing the path of the template and the list of placeholders and associated remplacement value.</param>
        /// <returns>The content of the file after replacement of the placeholders.</returns>
        ProcessedDocumentOutput ReplacePlaceholders(ProcessedDocumentInputs inputs);
        TemporaryFileInfo CreateTemporaryDocument(
            string? documentTemplatePath,
            string documentTemplateFileName,
            string directoryPathForTempFiles);
    }
}
