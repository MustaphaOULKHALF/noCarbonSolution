using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;

namespace noCarbon.Core.Helpers;
/// <summary>
/// Hash helper class
/// </summary>
public partial class HashHelper
{
    /// <summary>
    /// Create a data hash
    /// </summary>
    /// <param name="data">The data for calculating the hash</param>
    /// <param name="hashAlgorithm">Hash algorithm</param>
    /// <param name="trimByteCount">The number of bytes, which will be used in the hash algorithm; leave 0 to use all array</param>
    /// <returns>Data hash</returns>
    public static string CreateHash(byte[] data, string hashAlgorithm, int trimByteCount = 0)
    {
        if (string.IsNullOrEmpty(hashAlgorithm))
            throw new ArgumentNullException(nameof(hashAlgorithm));

        if (CryptoConfig.CreateFromName(hashAlgorithm) is not HashAlgorithm algorithm)
            throw new ArgumentException("Unrecognized hash name");

        if (trimByteCount > 0 && data.Length > trimByteCount)
        {
            var newData = new byte[trimByteCount];
            Array.Copy(data, newData, trimByteCount);

            return BitConverter.ToString(algorithm.ComputeHash(newData)).Replace("-", string.Empty);
        }

        return BitConverter.ToString(algorithm.ComputeHash(data)).Replace("-", string.Empty);
    }
    
    public static string Sha256_hash(string value)
    {
        StringBuilder Sb = new();
        using (var hash = SHA256.Create())
        {
            Encoding enc = Encoding.UTF8;
            byte[] result = hash.ComputeHash(enc.GetBytes(value));
            foreach (byte b in result)
                Sb.Append(b.ToString("x2"));
        }
        return Sb.ToString();
    }
}