namespace Homedish.Core.Cryptography
{
    public interface IEncryptor
    {
        string Encrypt(string plaintext);
        string Decrypt(string encryptedText);
    }
}