using System.Collections.Generic;

namespace Auth.API.Models
{
    public class ValidationErrorModel
    {
        public ValidationErrorModel(string error, IList<ValidationErrorDetailModel> details)
        {
            Error = error;
            Details = details;
        }

        public string Error { get; set; }
        public IList<ValidationErrorDetailModel> Details { get; set; }
    }
}
