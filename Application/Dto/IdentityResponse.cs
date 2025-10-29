namespace Application
{
    public class IdentityResponse
    {
        public object Data { get; set; } = new object();
        public bool Success { get; set; }
        public string[] Errors { get; set; } = [];
    }
}
