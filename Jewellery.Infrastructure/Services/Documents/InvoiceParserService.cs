
using Jewellery.Application.DTOs.Documents;
using Jewellery.Application.Interfaces.Services.Documents;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Jewellery.Infrastructure.Services.Documents
{
    public class InvoiceParserService : IInvoiceParserService
    {
        public InvoiceDataDto Parse(string extractedText)
        {
            var result = new InvoiceDataDto();

            if (string.IsNullOrWhiteSpace(extractedText))
                return result;

            result.InvoiceNumber = ExtractValue(
                extractedText,
                @"Invoice\s*(?:No\.?|Number|#)\s*[:#-]?\s*([A-Za-z0-9/-]+)");

            result.InvoiceDate = ExtractDate(extractedText);

            result.CustomerName = ExtractValue(
                extractedText,
                @"(?:Customer|Client|Buyer)\s*[:\-]?\s*([^\r\n]+)");

            result.SellerName = ExtractValue(
                extractedText,
                @"(?:Seller|Vendor|Company)\s*[:\-]?\s*([^\r\n]+)");

            if (string.IsNullOrWhiteSpace(result.SellerName))
            {
                result.SellerName = ExtractValue(
                    extractedText,
                    @"(J\.?\s*Goldsmith\s+manufacture)");
            }

            result.DeliveryAddress = ExtractAddress(
                extractedText,
                new[] { "Delivery address", "Delivery Address", "Shipping Address", "Ship To" },
                new[] { "Billing Address", "Billing address", "Invoice no", "Invoice Number", "J.Goldsmith" });

            result.BillingAddress = ExtractAddress(
                extractedText,
                new[] { "Billing Address", "Billing address", "Bill To" },
                new[] { "Invoice no", "Invoice Number", "Invoice Date" });

            result.SellerAddress = ExtractAddress(
                extractedText,
                new[] { "Seller Address", "Vendor Address" },
                new[] { "Invoice no", "Billing Address" });

            // Extract financial summary
            var summaryMatch = Regex.Match(
                extractedText,
                @"\b\d+\s+\S+\s+[0-9]+(?:\.[0-9]+)?\s+" +
                @"([0-9,]+\.\d{2})\s+" +
                @"([0-9,]+\.\d{2})\s+" +
                @"([0-9,]+\.\d{2})",
                RegexOptions.IgnoreCase);

            if (summaryMatch.Success)
            {
                result.SubTotal = ParseDecimal(summaryMatch.Groups[1].Value);
                result.TaxAmount = ParseDecimal(summaryMatch.Groups[2].Value);
                result.TotalAmount = ParseDecimal(summaryMatch.Groups[3].Value);
            }

            result.SubTotal ??= ExtractAmount(
                extractedText,
                @"(?:Sub\s*Total|Subtotal|المجموع الفرعي)\s*[:\-]?\s*([0-9,]+(?:\.[0-9]{1,2})?)");

            result.TaxAmount ??= ExtractAmount(
                extractedText,
                @"(?:Sales\s*tax\s*total|VAT|Tax|Sales Tax|الضريبة)\s*[:\-]?\s*([0-9,]+(?:\.[0-9]{1,2})?)");

            result.TotalAmount ??= ExtractAmount(
                extractedText,
                @"(?:Grand\s*Total|Invoice\s*Total|Total Amount|الإجمالي|المجموع الكلي)\s*[:\-]?\s*([0-9,]+(?:\.[0-9]{1,2})?)");

            result.Currency = ExtractValue(
                extractedText,
                @"(?:Currency|العملة)\s*[:\-]?\s*([A-Za-z]{3})");

            if (string.IsNullOrWhiteSpace(result.Currency))
            {
                var currencyMatch = Regex.Match(
                    extractedText,
                    @"\b(CHF|USD|EUR|JOD|GBP)\b",
                    RegexOptions.IgnoreCase);

                if (currencyMatch.Success)
                    result.Currency = currencyMatch.Groups[1].Value.ToUpperInvariant();
            }

            result.Items = ExtractItems(extractedText);

            return result;
        }

        private static string ExtractValue(string text, string pattern)
        {
            var match = Regex.Match(
                text,
                pattern,
                RegexOptions.IgnoreCase);

            return match.Success
                ? match.Groups[1].Value.Trim()
                : string.Empty;
        }

        private static string ExtractAddress(
            string text,
            string[] labels,
            string[] stopLabels)
        {
            var lines = text.Replace("\r", "")
                .Split('\n')
                .Select(x => x.Trim())
                .ToList();

            for (int i = 0; i < lines.Count; i++)
            {
                if (!labels.Any(label =>
                    lines[i].Contains(
                        label,
                        StringComparison.OrdinalIgnoreCase)))
                    continue;

                var addressLines = new List<string>();

                for (int j = i + 1; j < lines.Count; j++)
                {
                    if (string.IsNullOrWhiteSpace(lines[j]))
                        continue;

                    if (stopLabels.Any(label =>
                        lines[j].Contains(
                            label,
                            StringComparison.OrdinalIgnoreCase)))
                        break;

                    addressLines.Add(lines[j]);
                }

                return string.Join(", ", addressLines);
            }

            return string.Empty;
        }

        private static DateTime? ExtractDate(string text)
        {
            var match = Regex.Match(
                text,
                @"(?:Invoice\s*Date|Date|التاريخ)\s*[:#-]?\s*" +
                @"([0-9]{1,4}[./-][0-9]{1,2}[./-][0-9]{1,4})",
                RegexOptions.IgnoreCase);

            if (!match.Success)
                return null;

            string value = match.Groups[1].Value;

            string[] formats =
            {
                "dd.MM.yyyy",
                "dd/MM/yyyy",
                "dd-MM-yyyy",
                "yyyy-MM-dd",
                "yyyy/MM/dd"
            };

            if (DateTime.TryParseExact(
                value,
                formats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var date))
            {
                return date;
            }

            return null;
        }

        private static decimal? ExtractAmount(string text, string pattern)
        {
            var value = ExtractValue(text, pattern);
            return ParseDecimal(value);
        }


        private static List<InvoiceItemDataDto> ExtractItems(string text)
        {
            var items = new List<InvoiceItemDataDto>();

            var lines = text.Replace("\r", "")
                .Split('\n')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            var descriptionLines = new List<string>();
            bool insideItemsTable = false;

            foreach (var line in lines)
            {
                if (Regex.IsMatch(
                    line,
                    @"Sales\s+tax\s+summary|Sales\s+tax\s+total",
                    RegexOptions.IgnoreCase))
                    break;

                if (line.Contains("Reference", StringComparison.OrdinalIgnoreCase) &&
                    line.Contains("Designation", StringComparison.OrdinalIgnoreCase))
                {
                    insideItemsTable = true;
                    descriptionLines.Clear();
                    continue;
                }

                if (!insideItemsTable)
                    continue;

                // Ignore reference numbers
                if (Regex.IsMatch(line, @"^\d+$"))
                    continue;

                // Remove supplier information, but do not skip the line
                string cleanedLine = Regex.Replace(
                    line,
                    @"^AB\.DO\.GRAPHSITE\s+Graphic\s+designer\s+honoraries\s*",
                    "",
                    RegexOptions.IgnoreCase).Trim();

                // Detect unit, optional quantity and price
                var match = Regex.Match(
                    cleanedLine,
                    @"\b(?:Uni|U)\s+(?:(\d+)\s+)?([0-9,]+\.\d{2})\s*$",
                    RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    int quantity = 1;

                    if (int.TryParse(match.Groups[1].Value, out int qty))
                        quantity = qty;

                    decimal? price = ParseDecimal(match.Groups[2].Value);

                    // Keep description before the unit
                    string currentDescription = cleanedLine
                        .Substring(0, match.Index)
                        .Trim();

                    if (!string.IsNullOrWhiteSpace(currentDescription))
                        descriptionLines.Add(currentDescription);

                    string description = string.Join(
                        " ",
                        descriptionLines).Trim().TrimEnd(':');

                    if (!string.IsNullOrWhiteSpace(description))
                    {
                        items.Add(new InvoiceItemDataDto
                        {
                            Description = description,
                            Quantity = quantity,
                            UnitPrice = price,
                            TotalPrice = price * quantity
                        });
                    }

                    descriptionLines.Clear();
                    continue;
                }

                // Ignore supplier-only lines
                if (Regex.IsMatch(
                    cleanedLine,
                    @"^AB\.DO\.GRAPHSITE\b",
                    RegexOptions.IgnoreCase))
                    continue;

                descriptionLines.Add(cleanedLine);
            }

            return items;
        }


        private static decimal? ParseDecimal(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

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
    }
}
