namespace PlanitAutomation.Utils;

/// <summary>
/// Utility helpers for price/currency calculations used in cart assertions.
/// </summary>
public static class PriceUtils
{
    /// <summary>Rounds a value to 2 decimal places using mid-point-away-from-zero (HALF_UP) rounding.</summary>
    public static double Round2Dp(double value) =>
        Math.Round(value, 2, MidpointRounding.AwayFromZero);

    /// <summary>
    /// Strips currency symbols / whitespace and parses the remaining numeric string.
    /// Returns 0 if the input is null, empty, or non-numeric.
    /// </summary>
    public static double ParsePrice(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return 0.0;
        var cleaned = System.Text.RegularExpressions.Regex.Replace(raw, @"[^0-9.]", "");
        return double.TryParse(cleaned, System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture, out var result)
            ? result
            : 0.0;
    }
}
