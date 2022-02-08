namespace Auth.API.Models
{
    public class DomainErrorModel
    {
        public DomainErrorModel(string error)
        {
            Error = error;
        }

        public string Error { get; set; }

    }
}
