
using System.Globalization;
using System.Text.RegularExpressions;
using Jewellery.Application.DTOs.Documents;
using Jewellery.Application.Interfaces.Services;

namespace Jewellery.Infrastructure.Services
{
    public class CertificateParserService : ICertificateParserService
    {
        public CertificateDataDto Parse(string extractedText)
        {
            var result = new CertificateDataDto();

            if (string.IsNullOrWhiteSpace(extractedText))
                return result;

            result.CertificateNumber = ExtractText(
                extractedText,
                @"Certificate\s*No\s*[:#]?\s*(\S+)"
            );

            result.ProductType = ExtractText(
                extractedText,
                @"Product\s*[:#]?\s*([A-Za-z]+)"
            );

            result.Weight = ExtractDecimal(
                extractedText,
                @"Weight\s*[:#]?\s*([\d.,]+)"
            );

            result.Purity = ExtractText(
                extractedText,
                @"Purity\s*[:#]?\s*([\d.,]+\s*(?:KT|Karat)?)"
            );

            result.GoldPercentage = ExtractDecimal(
                extractedText,
                @"Gold\s*[:#]?\s*([\d.,]+)"
            );

            result.SilverPercentage = ExtractDecimal(
                extractedText,
                @"Silver\s*[:#]?\s*([\d.,]+)"
            );

            result.CopperPercentage = ExtractDecimal(
                extractedText,
                @"Copper\s*[:#]?\s*([\d.,]+)"
            );

            result.OtherPercentage = ExtractDecimal(
                extractedText,
                @"Other\s*[:#]?\s*([\d.,]+)"
            );

            return result;
        }

        private static string ExtractText(string text, string pattern)
        {
            var match = Regex.Match(
                text,
                pattern,
                RegexOptions.IgnoreCase
            );

            return match.Success ? match.Groups[1].Value.Trim() : string.Empty;
        }

        private static decimal? ExtractDecimal(string text, string pattern)
        {
            var value = ExtractText(text, pattern);

            if (string.IsNullOrWhiteSpace(value))
                return null;

            value = value.Replace(',', '.');

            if (decimal.TryParse(
                value,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out decimal result))
            {
                return result;
            }

            return null;
        }
    }
}
