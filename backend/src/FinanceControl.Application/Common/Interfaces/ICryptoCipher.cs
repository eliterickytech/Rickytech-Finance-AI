namespace FinanceControl.Application.Common.Interfaces;

/// <summary>
/// Abstração de criptografia simétrica usada para guardar chaves API at-rest.
/// </summary>
public interface ICryptoCipher
{
    string Encrypt(string plaintext);
    string Decrypt(string cipherText);
}
