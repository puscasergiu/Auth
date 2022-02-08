namespace Auth.API.Models
{
    public class ValidationErrorDetailModel
    {
        public ValidationErrorDetailModel(string propertyName, string errorMessage)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }

        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
    }
}
