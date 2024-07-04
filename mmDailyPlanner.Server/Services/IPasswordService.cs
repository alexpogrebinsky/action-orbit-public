namespace mmDailyPlanner.Server.Services
{
    public interface IPasswordService
    {
        public (string, byte[]) HashPassword(string password);
        public bool VerifyPassword(string password, string storedHash, byte[] storedSalt);

    }
}