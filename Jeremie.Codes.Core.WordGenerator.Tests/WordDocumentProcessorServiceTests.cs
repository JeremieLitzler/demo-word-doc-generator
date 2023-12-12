using Jeremie.Codes.Core.WordGenerator.Dto;
using Jeremie.Codes.Core.WordGenerator.Services;
using Jeremie.Codes.Core.WordGenerator.Tests.DemoData;
using Jeremie.Codes.Core.WordGenerator.Tests.Helpers;

namespace Jeremie.Codes.Core.WordGenerator.Tests
{
    public class Tests
    {
        private PersonDto _authorUser;
        private PersonDto _recipientUser;

        [SetUp]
        public void Setup()
        {
            _authorUser = new PersonDto()
            {
                FirstName = "Annot",
                LastName = "Compagnon",
                AddressLane = "26, rue Descartes",
                AddressPostalCode = "92150",
                AddressCity = "SURESNES"
            };
            _recipientUser = new PersonDto()
            {
                FirstName = "Thierry",
                LastName = "Grandpré",
                AddressLane = "70, rue Petite Fusterie",
                AddressPostalCode = "62200",
                AddressCity = "BOULOGNE-SUR-MER",
            };
        }

        [Test]
        public void GenerateDocx()
        {
            var dico = new Dictionary<string, string>();
            dico.Add(Enums.Placeholders.FIRSTNAME, _authorUser.FirstName);
            dico.Add(Enums.Placeholders.LASTNAME, _authorUser.LastName);
            dico.Add(Enums.Placeholders.ADDRESSLANE, _authorUser.AddressLane);
            dico.Add(Enums.Placeholders.ADDRESSPOSTALCODE, _authorUser.AddressPostalCode);
            dico.Add(Enums.Placeholders.ADDRESSCITY, _authorUser.AddressCity);

            dico.Add(Enums.Placeholders.RECIPIENT_FIRSTNAME, _recipientUser.FirstName);
            dico.Add(Enums.Placeholders.RECIPIENT_LASTNAME, _recipientUser.LastName);
            dico.Add(Enums.Placeholders.RECIPIENT_ADDRESSLANE, _recipientUser.AddressLane);
            dico.Add(Enums.Placeholders.RECIPIENT_ADDRESSPOSTALCODE, _recipientUser.AddressPostalCode);
            dico.Add(Enums.Placeholders.RECIPIENT_ADDRESSCITY, _recipientUser.AddressCity);

            dico.Add(Enums.Placeholders.GENERATIONDATE, DateTime.Today.ToLocaleFormattedDate());
            dico.Add(Enums.Placeholders.LETTERSUBJECT, "Lorem ipsum dolor sit amet");

            dico.Add(Enums.Placeholders.INLINEPLACEHOLDER1WITHSTYLE, "adipiscing");
            dico.Add(Enums.Placeholders.INLINEPLACEHOLDER2WITHSTYLE, "scelerisque");

            WordDocumentProcessorService service = new();

            var documentTemplateFileName = "template.docx";
            var documentTemplatePath = Path.Combine(Environment.CurrentDirectory, @"DemoDocx\", documentTemplateFileName); ;
            var directoryPathForTempFiles = Path.Combine(Environment.CurrentDirectory, @"GeneratedDocx\");
            var temporaryFile = service.CreateTemporaryDocument(documentTemplatePath, documentTemplateFileName, directoryPathForTempFiles);
            var inputs = new ProcessedDocumentInputs()
            {
                InputKeyValues = dico,
                AbsolutePathTemplateDocument = temporaryFile.FilePath
            };
            var output = service.ReplacePlaceholders(inputs);

            Assert.That(output.NewContent, Is.Not.Null);
            Assert.That(output.ReplacementWarnings, Is.Empty);

        }
    }
}