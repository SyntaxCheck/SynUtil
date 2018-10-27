using System;
using System.IO;

namespace SynUtil.FileSystem
{
    public static class Base64Serializer
    {
        public static string Serialize(string filename)
        {
            var bytes = File.ReadAllBytes(filename);
            return Convert.ToBase64String(bytes);
        }
        public static void DeserializeToFile(string base64String, string outputFilePath)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            File.WriteAllBytes(outputFilePath, bytes);
        }

        public static byte[] Deserialize(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            return bytes;
        }
    }
}
