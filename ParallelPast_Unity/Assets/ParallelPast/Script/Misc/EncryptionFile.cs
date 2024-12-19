using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class EncryptionFile
{
    private static readonly string key = "zFmn*2yLBD*iiZ@Z";
    private static readonly string iv = "zFmn*2yLBD*iiZ@Z"; 

    public static void EncryptToFile(string inputText, string filePath)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = Encoding.UTF8.GetBytes(iv);

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            MemoryStream msEncrypt = new MemoryStream();

            using (msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(inputText);
                    }
                }
            }

            File.WriteAllBytes(filePath, msEncrypt.ToArray());
        }
    }

    public static string DecryptFromFile(string filePath)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = Encoding.UTF8.GetBytes(iv);

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (FileStream fsDecrypt = new FileStream(filePath, FileMode.Open))
            {
                using (CryptoStream csDecrypt = new CryptoStream(fsDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }
}
