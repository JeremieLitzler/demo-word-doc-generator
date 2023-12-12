using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;
using Jeremie.Codes.Core.WordGenerator.Dto;
using Jeremie.Codes.Core.WordGenerator.Interfaces;
using Jeremie.Codes.Core.WordGenerator.Validators;

namespace Jeremie.Codes.Core.WordGenerator.Services
{
    public class WordDocumentProcessorService : IWordDocumentProcessorService
    {
        /// <summary>
        /// Create a tempory copy of the template provided
        /// </summary>
        /// <param name="documentTemplatePath">The directory containing the template document.</param>
        /// <param name="documentTemplateFileName">The name of the template document</param>
        /// <param name="directoryPathForTempFiles">Where is the copy stored</param>
        /// <returns>The TemporyFileInfo object with the absolute path and filename (contains a GUID to make it unique) of the new file.</returns>
        public TemporaryFileInfo CreateTemporaryDocument(
            string? documentTemplatePath,
            string documentTemplateFileName,
            string directoryPathForTempFiles)
        {
            CheckTemplateDocument(documentTemplatePath);
            CreateFolder(directoryPathForTempFiles);

            var fileExtension = Path.GetExtension(documentTemplateFileName);
            var newFileName = documentTemplateFileName.Replace(fileExtension, $"-{Guid.NewGuid()}{fileExtension}");
            var newFilePath = $"{directoryPathForTempFiles}\\{newFileName}";

            File.Copy(documentTemplatePath!, newFilePath);

            WasDocumentTemplateCopySuccessful(directoryPathForTempFiles, newFileName, newFilePath);

            return new TemporaryFileInfo()
            {
                FileName = newFileName,
                FilePath = newFilePath,
            };
        }

        /// <summary>
        /// Process the document by replacing the placeholders (inputs.InputKeyValues) in the document (inputs.AbsolutePathTemplateDocument).
        ///
        /// The OpenDocument library is used to read the document along side the OpenXmlPowerTools to replace the placeholders.
        ///
        /// Inspired by: <see href="https://github.com/OpenXmlDev/Open-Xml-PowerTools/blob/vNext/OpenXmlPowerToolsExamples/TextReplacer02/TextReplacer02.cs"/>
        /// </summary>
        /// <param name="inputs">The inputs used to generate a document</param>
        /// <returns>The output object with the content and warnings to log.</returns>
        /// <exception cref="HttpResponseException">Return a HTTP 404 is the document to process doesn't exist. If it does the CreateTemporaryDocument method wasn't called.</exception>
        public ProcessedDocumentOutput ReplacePlaceholders(ProcessedDocumentInputs inputs)
        {
            if (!File.Exists(inputs.AbsolutePathTemplateDocument))
            {
                throw new FileNotFoundException($"Check your path... {inputs.AbsolutePathTemplateDocument} doesn't seem to be valid. Or have you forgot to call CreateTemporaryDocument method?");
            }

            var warnings = new List<string>();
            HasPlaceholdersPresent(inputs, warnings);

            var allPlaceholders = inputs.InputKeyValues?.Keys.ToList();
            using (WordprocessingDocument doc = WordprocessingDocument.Open(inputs.AbsolutePathTemplateDocument, true))
            {
                foreach (var placeholder in allPlaceholders!)
                {
                    try
                    {
                        TextReplacer.SearchAndReplace(doc, placeholder, inputs?.InputKeyValues![placeholder], true);
                    }
                    catch (Exception ex)
                    {
                        //The catch is there mainly to keep going.
                        //Some placeholders may run into "Index was outside the bounds of the array."
                        warnings.Add($"On <{placeholder}> placeholder, the error was <<{ex.Message}>> with the StackTrace: <<{ex.StackTrace}>>");
                    }
                }
                DocxValidator.ValidateWordDocument(doc, warnings);
            }

            return new ProcessedDocumentOutput()
            {
                NewContent = File.ReadAllBytes(inputs.AbsolutePathTemplateDocument),
                ReplacementWarnings = warnings,
            };
        }

        /// <summary>
        /// Create a folder if it doesn't exist.
        /// </summary>
        /// <param name="absoluteFolderPath"></param>
        internal static void CreateFolder(string absoluteFolderPath)
        {
            if (!Directory.Exists(absoluteFolderPath))
            {
                Directory.CreateDirectory(absoluteFolderPath);
            }
        }

        /// <summary>
        /// Check that the inputs contain at least one placeholders.
        /// If it doesn't we add a warning to log the fact.
        /// </summary>
        /// <param name="inputs">The inputs used to generate a document</param>
        /// <param name="warnings">The list of warnings</param>
        /// <returns>True if the check passes.</returns>
        internal static bool HasPlaceholdersPresent(ProcessedDocumentInputs inputs, List<string> warnings)
        {
            if (inputs.InputKeyValues == null)
            {
                warnings.Add($"No placeholder found in the request. Please fill InputKeyValues dictionnary.");
                return false;
            }
            if (inputs.InputKeyValues.Count == 0)
            {
                warnings.Add($"No placeholder found in the request. Please fill InputKeyValues dictionnary.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Was the unique copy of the template document successful?
        /// </summary>
        /// <param name="directoryPathForTempFiles">The absolute directory path where the file was copied</param>
        /// <param name="newFileName">The file name of the copy</param>
        /// <param name="newFilePath">The absolute file path of the copy</param>
        /// <returns>True if it did</returns>
        /// <exception cref="HttpResponseException">Return a HTTP 500 if the copy failed.</exception>
        internal static bool WasDocumentTemplateCopySuccessful(string directoryPathForTempFiles, string newFileName, string newFilePath)
        {
            if (!File.Exists(newFilePath))
            {
                throw new FileNotFoundException($"The new file <{newFileName}> wasn't not create in folder {directoryPathForTempFiles}.");
            }

            return true;
        }

        /// <summary>
        /// Check if the template document really exists
        /// </summary>
        /// <param name="documentTemplatePath">The absolute path to the document template</param>
        /// <returns>True if it exists</returns>
        /// <exception cref="HttpResponseException">Return HTTP 400 (path absent) or HTTP 404 (template not found)</exception>
        internal static bool CheckTemplateDocument(string? documentTemplatePath)
        {
            if (documentTemplatePath == null)
            {
                throw new ArgumentNullException($"the path to the document model <{documentTemplatePath}> is not provided.");
            }

            if (!File.Exists(documentTemplatePath))
            {
                throw new FileNotFoundException($"The file <{documentTemplatePath}> doesn't exist in the file system.");
            }

            return true;
        }
    }
}
