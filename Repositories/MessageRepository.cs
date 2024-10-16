using BackEnd_2024_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd_2024_Project.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly MessageServiceContext _context;
        public MessageRepository(MessageServiceContext context)
        {
            _context = context;
        }
        public async Task<bool> DeleteMessageAsync(Message message)
        {
            if (message == null)
            {
                return false;
            }
            else
            {
                _context.Messages.Remove(message);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<Message?> GetMessageAsync(long id)
        {            
            return await _context.Messages.Include(s=>s.Sender).Include(s=>s.Recipient).FirstOrDefaultAsync(i=>i.Id == id);
        }

        //Get public messages
        public async Task<IEnumerable<Message>> GetMessagesAsync()
        {            
            return await _context.Messages.Include(s => s.Sender).Where(x => x.Recipient==null).OrderByDescending(x=>x.Id).Take(10).ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetMyReceivedMessagesAsync(User user)
        {            
            return await _context.Messages.Include(s => s.Sender).Where(x => x.Recipient == user).OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetMySentMessagesAsync(User user)
        {            
            return await _context.Messages.Include(x => x.Recipient).Include(x=>x.Sender).Where(x => x.Sender == user).OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task<Message?> NewMessageAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return message;

        }

        public async Task<bool> UpdateMessageAsync(Message message)
        {
            _context.Entry(message).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
