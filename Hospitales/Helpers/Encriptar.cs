
using System.Security.Cryptography;
using System.Text;

namespace Hospitales.Helpers
{
    public class Encriptar
    {
        public static string EncriptarPass(string text)
        {
            SHA256 sha = SHA256.Create();
            byte[] textSinCifrar = Encoding.Default.GetBytes(text);
            byte[] textCifrado = sha.ComputeHash(textSinCifrar);

            return BitConverter.ToString(textCifrado).Replace("-", "");
        }
    }
}
