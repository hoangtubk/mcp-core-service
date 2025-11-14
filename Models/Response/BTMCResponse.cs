namespace mcp_core_service.Models.Response
{
    public class BTMCResponse
    {
        public DataList? DataList { get; set; }
    }

    public class DataList
    {
        public List<Data>? data { get; set; }
    }

    public class Data
    {
        public string? row { get; set; }
        public string? n { get; set; }
        public string? k { get; set; }
        public string? h { get; set; }
        public string? pb { get; set; }
        public string? ps { get; set; }
        public string? pt { get; set; }
        public DateTime? d { get; set; }
    }
}
