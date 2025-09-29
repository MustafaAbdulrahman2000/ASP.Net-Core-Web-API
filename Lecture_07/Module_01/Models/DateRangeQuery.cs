using System.Diagnostics.CodeAnalysis;

namespace Module_01.Models;

public class DateRangeQuery: IParsable<DateRangeQuery>
{
    public DateOnly FromDate { get; init; }
    public DateOnly ToDate { get; init; }
    
    public static DateRangeQuery Parse(string s, IFormatProvider? provider)
    {
        if (!TryParse(s, provider, out var result))
        {
            throw new ArgumentException("could not parse supplied values.");
        }

        return result;
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out DateRangeQuery result)
    {
        var segments = s?.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (segments?.Length == 2 && DateOnly.TryParse(segments[0], provider, out var fromDate) && DateOnly.TryParse(segments[1], provider, out var toDate))
        {
            result = new DateRangeQuery
            {
                FromDate = fromDate,
                ToDate = toDate
            };

            return true;
        }

        result = new DateRangeQuery
        {
            FromDate = default,
            ToDate = default
        };

        return false;
    }
}
