namespace SafeScribe_cp05.Models
{
    public class Note
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        //chave estrangeira
        public string UserId { get; set; }
    }
}
