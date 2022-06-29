namespace DogFactsWeb.Models
{
    public class GenericResponseVM
    {
        public GenericResponseVM()
        {
            Errors = new List<string>();
        }

        public string Message { get; set; }

        public IList<string> Errors { get; set; }

        public bool Ok => Errors.Count == 0;

        public GenericResponseVM AddError(string errorMessage)
        {
            Errors.Add(errorMessage);

            return this;
        }

        public GenericResponseVM AddMessage(string message)
        {
            this.Message = message;

            return this;
        }
    }
}
