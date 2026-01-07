namespace EventMarketplace.Application.Utils;

public static class DurationUtils
{
    public static string DurationCalculate(DateTime start, DateTime end)
    {
        var duration = end - start;
        
        return duration.Days == 0 ? $"{duration.Hours}h {duration.Minutes}m" : $"{duration.Days}d {duration.Hours}h";
    }
}