using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using mmDailyPlanner.Server.Constants;

namespace mmDailyPlanner.Server.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly ILogger<PasswordService> _logger;

        public PasswordService(ILogger<PasswordService> logger)
        {
            _logger = logger;
        }

        public (string, byte[]) HashPassword(string password)
        {
            try
            {
                _logger.LogInformation(AuthMessages.CheckingAuthentication);
                // generate a 128-bit salt using a secure PRNG
                byte[] salt = new byte[128 / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }

                // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));

                _logger.LogInformation(AuthMessages.UserRegistered);
                return (hashed, salt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.ErrorOccuredWhileActionDescription, AuthMessages.Registration);
                throw;
            }
        }

        public bool VerifyPassword(string password, string storedHash, byte[] storedSalt)
        {
            try
            {
                _logger.LogInformation(AuthMessages.Login);
                // Hash the input password with the stored salt
                string hashedInputPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: storedSalt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));

                bool isVerified = hashedInputPassword == storedHash;

                if (isVerified)
                {
                    _logger.LogInformation(AuthMessages.UserLoggedIn);
                }
                else
                {
                    _logger.LogWarning(AuthMessages.Login);
                }

                return isVerified;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.ErrorOccuredWhileActionDescription, AuthMessages.Login);
                throw;
            }
        }
    }
}
