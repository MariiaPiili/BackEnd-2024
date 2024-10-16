using BackEnd_2024_Project.Models;
using BackEnd_2024_Project.Repositories;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace BackEnd_2024_Project.Middleware
{
    public interface IUserAuthenticationService
    {
        Task<User?> Authenticate(string username, string password);
        public User CreateUserCredentials(User user);
        Task<bool> isMyMessage(string username, long messageID);
    }

    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;

        public UserAuthenticationService(IUserRepository userRepository, IMessageRepository messageRepository)
        {
            _userRepository = userRepository;
            _messageRepository = messageRepository;
        }

        public async Task<User?> Authenticate(string username, string password)
        {
            User? user;

            user = await _userRepository.GetUserAsync(username);
            if (user == null)
            {
                return null;
            }

            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: user.Salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 258 / 8));

            if (hashedPassword != user.Password)
            {
                return null;
            }
            return user;
        }

        public User CreateUserCredentials(User user)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: user.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 258 / 8));

            user.Password = hashedPassword;
            user.Salt = salt;
            //user.JoinDate = user.JoinDate != null ? user.JoinDate : DateTime.Now;
            //user.LastLoginDate = DateTime.Now;

            return user;
        }

        public async Task<bool> isMyMessage(string username, long messageID)
        {
            User? user = await _userRepository.GetUserAsync(username);
            if (user == null)
            {
                return false;
            }
            Message? message = await _messageRepository.GetMessageAsync(messageID);
            if (message == null)
            {
                return false;
            }
            if (message.Sender == user)
            {
                return true;
            }
            return false;
        }
    }
}
