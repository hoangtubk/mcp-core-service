namespace mcp_core_service.Helpper
{
    public class BasicAuthOptions
    {
        public const string SectionName = "BasicAuthOptions";
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
