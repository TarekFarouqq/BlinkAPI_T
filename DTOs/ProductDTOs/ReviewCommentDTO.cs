namespace Blink_API.DTOs.Product
{
    public class ReviewCommentDTO
    {
        public string Username { get; set; }
        public int Rate { get; set; }
        public ICollection<string> ReviewComment { get; set; }

    }
}
