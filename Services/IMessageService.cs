using BackEnd_2024_Project.Models;

namespace BackEnd_2024_Project.Services
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageDTO>> GetMessagesAsync();
        Task<IEnumerable<MessageDTO>> GetMyReceivedMessagesAsync(string username);
        Task<IEnumerable<MessageDTO>> GetMySentMessagesAsync(string username);
        Task<MessageDTO?> GetMessageAsync(long id);
        Task<MessageDTO> NewMessageAsync(MessageDTO message);
        Task<bool> UpdateMessageAsync(MessageDTO message);
        Task<bool> DeleteMessageAsync(long id);

    }
}
