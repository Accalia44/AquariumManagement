using System;
using System.Security.Cryptography;
using System.Text;

//source: https://code-maze.com/csharp-hashing-salting-passwords-best-practices/

namespace DAL
{
	public class PasswordHasher
	{
		public PasswordHasher(){}

        const int keySize = 64;
        const int iterations = 350000;

        static HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        static public string HashPasword(string password, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(keySize);

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);
            return Convert.ToHexString(hash);
        }


        static public bool VerifyPassword(string password, string hash, byte[] salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);
            return hashToCompare.SequenceEqual(Convert.FromHexString(hash));
        }

        
    }
}

