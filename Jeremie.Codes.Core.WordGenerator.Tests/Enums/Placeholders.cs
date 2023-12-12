using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeremie.Codes.Core.WordGenerator.Tests.Enums
{
    public static class Placeholders
    {
        public static string FIRSTNAME = "{{FirstName}}";
        public static string LASTNAME = "{{LastName}}";
        public static string ADDRESSLANE = "{{AddressLane}}";
        public static string ADDRESSPOSTALCODE = "{{AddressPostalCode}}";
        public static string ADDRESSCITY = "{{AddressCity}}";
        public static string RECIPIENT_FIRSTNAME = "{{RecipientFirstName}}";
        public static string RECIPIENT_LASTNAME = "{{RecipientLastName}}";
        public static string RECIPIENT_ADDRESSLANE = "{{RecipientAddressLane}}";
        public static string RECIPIENT_ADDRESSPOSTALCODE = "{{RecipientAddressPostalCode}}";
        public static string RECIPIENT_ADDRESSCITY = "{{RecipientAddressCity}}";

        public static string LETTERSUBJECT = "{{LetterSubject}}";

        public static string GENERATIONDATE = "{{GenerationDate}}";

        public static string INLINEPLACEHOLDER1WITHSTYLE = "{{InlinePlaceholder1WithStyle}}";
        public static string INLINEPLACEHOLDER2WITHSTYLE = "{{InlinePlaceholder2WithStyle}}";
    }
}
