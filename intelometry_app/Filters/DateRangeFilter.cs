namespace intelometry_app.Filters
{
    public class DateRangeFilter
    {
        public string Start { get; set; } = string.Empty;
        public string End { get; set; } = string.Empty;
        public string? Type { get; set; } = string.Empty;
        public bool IsApplied { get; set; }

        public DateRangeFilter(string start, string end, string? type, bool isApplied)
        {
            Start = start;
            End = end;
            Type = type;
            IsApplied = isApplied;
        }
    }
}
