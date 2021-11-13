namespace Api.Contracts.V1.Request
{
    public class UpdatePostRequest
    {
        public string Title { get; }
        public string Content { get; }
        public string[] Tags { get; }
    }
}