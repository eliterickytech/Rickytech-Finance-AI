using System.Security.Cryptography;
using System.Text;
using FinanceControl.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace FinanceControl.Infrastructure.Authentication;

/// <summary>
/// AES-256-GCM com master key vinda do appsettings/UserSecrets/KeyVault.
/// Formato do payload: base64(nonce(12) | tag(16) | ciphertext).
/// </summary>
public sealed class AesCryptoCipher : ICryptoCipher
{
    private readonly byte[] _masterKey;

    public AesCryptoCipher(IConfiguration configuration)
    {
        var raw = configuration["KeyVault:MasterKey"]
            ?? "FinanceControlDefaultMasterKey_32By!!"; // 32 bytes
        // SHA-256 garante 32 bytes mesmo se a string vier menor
        _masterKey = SHA256.HashData(Encoding.UTF8.GetBytes(raw));
    }

    public string Encrypt(string plaintext)
    {
        if (string.IsNullOrEmpty(plaintext)) return string.Empty;

        Span<byte> nonce = stackalloc byte[12];
        RandomNumberGenerator.Fill(nonce);

        var plain = Encoding.UTF8.GetBytes(plaintext);
        var cipher = new byte[plain.Length];
        var tag = new byte[16];

        using var aes = new AesGcm(_masterKey, 16);
        aes.Encrypt(nonce, plain, cipher, tag);

        var result = new byte[nonce.Length + tag.Length + cipher.Length];
        nonce.CopyTo(result.AsSpan(0, 12));
        tag.CopyTo(result, 12);
        cipher.CopyTo(result, 12 + 16);
        return Convert.ToBase64String(result);
    }

    public string Decrypt(string cipherText)
    {
        if (string.IsNullOrEmpty(cipherText)) return string.Empty;

        var raw = Convert.FromBase64String(cipherText);
        var nonce = raw.AsSpan(0, 12);
        var tag = raw.AsSpan(12, 16);
        var cipher = raw.AsSpan(28);
        var plain = new byte[cipher.Length];

        using var aes = new AesGcm(_masterKey, 16);
        aes.Decrypt(nonce, cipher, tag, plain);
        return Encoding.UTF8.GetString(plain);
    }
}
