namespace Auth.API.Models.Options
{
    public class ConnectionStringsOptions
    {
        public const string SectionName = "ConnectionStrings";

        public string Database { get; set; }
        public string Redis { get; set; }
    }
}
