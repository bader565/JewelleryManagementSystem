
using Jewellery.Application.DTOs.Documents;
using Jewellery.Application.Interfaces.Services.Documents;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Jewellery.Infrastructure.Services.Documents
{
    public class AppraisalParserService : IAppraisalParserService
    {
        public AppraisalDataDto Parse(string extractedText)
        {
            var result = new AppraisalDataDto();

            if (string.IsNullOrWhiteSpace(extractedText))
                return result;

            result.ReportNumber = ExtractValue(
                extractedText,
                @"(?:APPRAISAL\s*REPORT|Report\s*(?:No|Number|#))\s*[:#-]?\s*([A-Za-z0-9/-]+)");

            result.AppraiserName = ExtractValue(
                extractedText,
                @"(?:Appraiser|Evaluator|المقيّم|المثمن)\s*[:\-]?\s*([^\r\n]+)");
            result.AppraisalDate = ExtractDate(
                extractedText,
                @"(?:DATE|Appraisal Date|Report Date|تاريخ التقييم)\s*[:\-]?\s*([0-9]{1,2}[/-][0-9]{1,2}[/-][0-9]{4})");

            result.Description = ExtractValue(
                extractedText,
                @"(?:DESCRIPTION OF ARTICLES|Description|Details|الوصف)\s*[:\-]?\s*([^\r\n]+)");

            result.ProductType = ExtractValue(
                extractedText,
                @"(?:Product Type|Jewellery Type|Item Type|المنتج|النوع)\s*[:\-]?\s*([^\r\n]+)");

            result.Material = ExtractValue(
         extractedText,
         @"(?:Material|Metal|المعدن)\s*[:\-]?\s*([^\r\n]+)");

            if (string.IsNullOrWhiteSpace(result.Material))
            {
                var materialMatch = Regex.Match(
                    extractedText,
                    @"\b(14\s*K(?:L)?|18\s*K(?:L)?|22\s*K(?:L)?|24\s*K(?:L)?)\b[.,]?\s*(Yellow Gold|White Gold|Rose Gold|Gold|Platinum)",
                    RegexOptions.IgnoreCase);

                if (materialMatch.Success)
                    result.Material = materialMatch.Groups[2].Value.Trim();
            }

            result.Purity = ExtractValue(
       extractedText,
       @"(?:Purity|Karat|عيار|النقاوة)\s*[:\-]?\s*([^\r\n]+)");

            if (string.IsNullOrWhiteSpace(result.Purity))
            {
                var purityMatch = Regex.Match(
                    extractedText,
                    @"\b(14|18|21|22|24)\s*K(?:L)?\b",
                    RegexOptions.IgnoreCase);

                if (purityMatch.Success)
                    result.Purity = purityMatch.Groups[1].Value + "K";
            }
            result.Weight = ExtractWeight(extractedText);

            var shapeMatch = Regex.Match(
             extractedText,
             @"\b(Round\s+Brilliant|Princess|Oval|Pear|Emerald|Marquise|Cushion|Radiant|Heart)\b",
             RegexOptions.IgnoreCase);

            result.DiamondShape = shapeMatch.Success
                ? shapeMatch.Groups[1].Value.Trim()
                : string.Empty;

            result.CutGrade = ExtractValue(
                extractedText,
                @"(?:Cut Grade|Cut)\s*[:\-]?\s*([^\r\n]+)");

            result.ColorGrade = ExtractValue(
                extractedText,
                @"(?:Color Grade|Colour Grade)\s*[:\-]?\s*([^\r\n]+)");

            result.ClarityGrade = ExtractValue(
                extractedText,
                @"(?:Clarity Grade|Clarity)\s*[:\-]?\s*([^\r\n]+)");

            var symmetryMatch = Regex.Match(
       extractedText,
       @"Symmetry\s*[.:]*\s*([^\r\n]+)",
       RegexOptions.IgnoreCase);

            result.Symmetry = string.Empty;

            if (symmetryMatch.Success)
            {
                var symmetryText = symmetryMatch.Groups[1].Value;

                if (Regex.IsMatch(symmetryText, @"Excell?ant|Excall?ant|Excellent",
                    RegexOptions.IgnoreCase))
                {
                    result.Symmetry = "Excellent";
                }
                else if (Regex.IsMatch(symmetryText, @"Very Good",
                    RegexOptions.IgnoreCase))
                {
                    result.Symmetry = "Very Good";
                }
                else if (Regex.IsMatch(symmetryText, @"Good",
                    RegexOptions.IgnoreCase))
                {
                    result.Symmetry = "Good";
                }
            }

            if (Regex.IsMatch(result.Symmetry, @"Excellent", RegexOptions.IgnoreCase))
                result.Symmetry = "Excellent";

            result.Fluorescence = ExtractValue(
                extractedText,
                @"Fluorescence\s*[.:]*\s*([^\r\n]+)");

            if (Regex.IsMatch(result.Fluorescence, @"None", RegexOptions.IgnoreCase))
                result.Fluorescence = "None";

            result.EstimatedValue = ExtractAmount(
                extractedText,
                @"(?:ESTIMATED VALUE(?: COMPARISON)?|Estimated Value|Appraised Value|Valuation)\s*[:\-]?\s*\$?\s*([0-9,]+(?:\.[0-9]{1,2})?)");

            result.Currency = ExtractCurrency(extractedText);

            return result;
        }

        private static decimal? ExtractWeight(string text)
        {
            var weightMatch = Regex.Match(
                text,
                @"(?:Weight|Wight|Weigh|Wgt|الوزن)\s*[:.\-]?\s*([0-9]+(?:[.,][0-9]+)?)\s*(?:Carats?|Cars?|Ct|Cts)?",
                RegexOptions.IgnoreCase);

            if (weightMatch.Success &&
                TryParseWeight(weightMatch.Groups[1].Value, out var weight))
            {
                return weight;
            }

            var caratMatch = Regex.Match(
                text,
                @"\b([0-9]+(?:[.,][0-9]+)?)\s*(?:Carats?|Cars?|Ct|Cts)\b",
                RegexOptions.IgnoreCase);

            if (caratMatch.Success &&
                TryParseWeight(caratMatch.Groups[1].Value, out weight))
            {
                return weight;
            }

            return null;
        }

        private static bool TryParseWeight(string value, out decimal weight)
        {
            value = value.Replace(",", ".");

            return decimal.TryParse(
                value,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out weight);
        }

        private static string ExtractValue(string text, string pattern)
        {
            var match = Regex.Match(
                text,
                pattern,
                RegexOptions.IgnoreCase);

            if (!match.Success)
                return string.Empty;

            var value = match.Groups[1].Value.Trim();

            string[] invalidValues =
            {
                "Measurements",
                "CUT GRADE",
                "COLOR GRADE",
                "CLARITY GRADE",
                "SHAPE AND CUT",
                "SYMMETRY",
                "FLUORESCENCE"
            };

            if (invalidValues.Any(x =>
                value.Equals(x, StringComparison.OrdinalIgnoreCase)))
            {
                return string.Empty;
            }

            return value;
        }

        private static decimal? ExtractAmount(string text, string pattern)
        {
            var value = ExtractValue(text, pattern);

            if (decimal.TryParse(
                value.Replace(",", ""),
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out var amount))
            {
                return amount;
            }

            return null;
        }

        private static DateTime? ExtractDate(string text, string pattern)
        {
            var value = ExtractValue(text, pattern);

            if (DateTime.TryParse(
                value,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var date))
            {
                return date;
            }

            return null;
        }

        private static string ExtractCurrency(string text)
        {
            if (Regex.IsMatch(text, @"\$"))
                return "USD";

            return ExtractValue(
                text,
                @"(?:Currency|العملة)\s*[:\-]?\s*([A-Za-z]{3,})");
        }
    }
}
