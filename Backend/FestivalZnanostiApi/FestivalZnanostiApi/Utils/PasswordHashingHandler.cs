using System.Security.Cryptography;
using System.Text;

namespace FestivalZnanostiApi.Utils
{
    public class PasswordHashingHandler
    {
        private readonly string _password;

        public PasswordHashingHandler(string password)
        {
            _password = password;
        }

        public void CreatePasswordHash(out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA256())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(_password));
            }
        }

        public bool VerifyPasswordHash(byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA256(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(_password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
