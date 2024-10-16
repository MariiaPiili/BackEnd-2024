using System.ComponentModel.DataAnnotations;

namespace BackEnd_2024_Project.Models
{
    public class Message
    {
        public long Id { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string? Body { get; set; }
        public User Sender { get; set; }
        public User? Recipient { get; set; }
        public Message? PrevMessage { get; set; }

    }
    public class MessageDTO
    {
        public long Id { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string? Body { get; set; }
        public string Sender { get; set; }
        public string? Recipient { get; set; }
        public long? PrevMessageID { get; set; }

    }
}
