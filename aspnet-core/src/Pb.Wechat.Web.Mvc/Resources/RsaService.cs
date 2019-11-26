using System.Security.Cryptography;

namespace Pb.Wechat.Web.Resources
{
    public static class RsaService
    {
        private static int KeyLength = 1024;

        private static RSACryptoServiceProvider RsaCrypto = new RSACryptoServiceProvider(KeyLength);

        public static RSAParameters GetRsaPublicKey()
        {
            return RsaService.RsaCrypto.ExportParameters(false);
        }

        public static byte[] Decrypt(byte[] encryptBytes)
        {
            byte[] plainBytes = RsaService.RsaCrypto.Decrypt(encryptBytes, false);

            return plainBytes;
        }
    }
}
