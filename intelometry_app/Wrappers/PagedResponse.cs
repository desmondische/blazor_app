namespace intelometry_app.Wrappers
{
    public class PagedResponse<T>
    {
        public int TotalRecords { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Columns { get; set; }
        public T Data { get; set; } = default!;

        public PagedResponse(T data, int totalRecords, string message, List<string> columns)
        {
            TotalRecords = totalRecords;
            Data = data;
            Message = message;
            Columns = columns;
        }
    }
}
