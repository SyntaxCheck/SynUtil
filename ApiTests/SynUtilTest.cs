using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SynUtil.Network;
using SynUtil.Crypto;
using System.Diagnostics;
using SynUtil.FileSystem;
using System.IO;
using System.Collections.Generic;
using SynUtil.Extensions;

namespace ApiTests
{
    [TestClass]
    public class SynUtilTest
    {
        private const string ALL_CHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-=!@#$%^&*()_+\\][|}{';:/.,?><\"";

        [TestMethod]
        public void TestLogger()
        {
            TestLogger(false);
            TestLogger(true);
        }
        [TestMethod]
        public void TestSendPortCheck()
        {
            SendPortCheck spc = new SendPortCheck();

            string rtn = spc.CheckPort("www.google.com", 12345);
            Assert.IsTrue(rtn.Contains("Failure:"), "google.com Blocked port check");

            rtn = spc.CheckPort("www.google.com", 80);
            Assert.IsTrue(rtn.Contains("Success:"), "google.com Open port check");
        }
        [TestMethod]
        public void TestHexConversion()
        {
            string dataToHex = ALL_CHARS;
            string hex = HexConversion.TextToHex(dataToHex);
            string plainText = HexConversion.HexToText(hex);

            Assert.AreEqual(hex, "61 62 63 64 65 66 67 68 69 6a 6b 6c 6d 6e 6f 70 71 72 73 74 75 76 77 78 79 7a 41 42 43 44 45 46 47 48 49 4a 4b 4c 4d 4e 4f 50 51 52 53 54 55 56 57 58 59 5a 31 32 33 34 35 36 37 38 39 30 2d 3d 21 40 23 24 25 5e 26 2a 28 29 5f 2b 5c 5d 5b 7c 7d 7b 27 3b 3a 2f 2e 2c 3f 3e 3c 22", "HexConversion the hex value does not match predetermined hex value");
            Assert.AreEqual(dataToHex, plainText, "Inpux Hex does not match initial value");
        }
        [TestMethod]
        public void TestEncryption()
        {
            AesEncryption aesEncryption = new AesEncryption();

            string dataToEncrypt = ALL_CHARS;
            string encrypted = aesEncryption.Encrypt(dataToEncrypt, "SomeT3st P@55w0rd");
            if (String.IsNullOrEmpty(encrypted))
                encrypted = null;
            Assert.IsNotNull(encrypted, "Encryption returned null");

            string decrypted = aesEncryption.Decrypt(encrypted, "SomeT3st P@55w0rd");
            if (String.IsNullOrEmpty(decrypted))
                decrypted = null;
            Assert.IsNotNull(decrypted, "Decryption returned null");
            Assert.AreEqual(dataToEncrypt, decrypted, "Initial encryption string and decrypted value do not match");
        }
        [TestMethod]
        public void TestHash()
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            string hashTextLooped = Hash.LoopedSHA512(ALL_CHARS);
            stopwatch.Stop();
            long elapsedLoop = stopwatch.ElapsedMilliseconds;
            stopwatch.Reset();
            stopwatch.Start();
            string hashMD5 = Hash.MD5(ALL_CHARS);
            stopwatch.Stop();
            long elapsedMD5 = stopwatch.ElapsedMilliseconds;
            stopwatch.Reset();
            stopwatch.Start();
            string hashSHA1 = Hash.SHA1(ALL_CHARS);
            stopwatch.Stop();
            long elapsedSHA1 = stopwatch.ElapsedMilliseconds;
            stopwatch.Reset();
            stopwatch.Start();
            string hashSHA256 = Hash.SHA256(ALL_CHARS);
            stopwatch.Stop();
            long elapsedSHA256 = stopwatch.ElapsedMilliseconds;
            stopwatch.Reset();
            stopwatch.Start();
            string hashSHA512 = Hash.SHA512(ALL_CHARS);
            stopwatch.Stop();
            long elapsedSHA512 = stopwatch.ElapsedMilliseconds;
            stopwatch.Reset();
            stopwatch.Start();
            string hashBcrypt = Hash.BCryptHash(ALL_CHARS);
            stopwatch.Stop();
            long elapsedBcrypt = stopwatch.ElapsedMilliseconds;
            stopwatch.Reset();
            stopwatch.Start();
            string hashScrypt = Hash.Scrypt(ALL_CHARS);
            stopwatch.Stop();
            long elapsedScrypt = stopwatch.ElapsedMilliseconds;

            bool bcryptIsMatch = Hash.BCryptHashIsMatch(ALL_CHARS, hashBcrypt);
            bool scryptIsMatch = Hash.ScryptIsMatch(ALL_CHARS, hashScrypt);

            Assert.AreEqual(hashTextLooped, "8CACF7DBEFE5CDA74EA6079830B3F02710C9F7E29EC263B7EC0B9A6D18654120C24491207E402F213D499C52930C018B871B0DFD549E150FD7F1FBEE46862187", "Hash Test LoopedSHA512 does not match predetermined value");
            Assert.AreEqual(hashMD5, "B4FD6F2E146E08446E8EB5BC997B907C", "Hash Test MD5 does not match predetermined value");
            Assert.AreEqual(hashSHA1, "3BC725C81C8E053C8DC296E8FA00F710BDF012B9", "Hash Test SHA1 does not match predetermined value");
            Assert.AreEqual(hashSHA256, "169A297DC085700F542FC7CAF0B83B20C28C80764A42FA5998C717AC78686388", "Hash Test SHA256 does not match predetermined value");
            Assert.AreEqual(hashSHA512, "BDB6E7C0AB15497A7769F70AB786EF9BE3EBA7E6AC431BD1294739003B8DF6DB58911755B3EBAD153DCD2F2A34FB549F91AAE0A1B738E883BBBD65985B79DEE6", "Hash Test SHA512 does not match predetermined value");
            Assert.AreEqual(bcryptIsMatch, true, "BCrypt isMatch not returning a match");
            Assert.AreEqual(scryptIsMatch, true, "Scrypt isMatch not returning a match");
        }
        [TestMethod]
        public void TestExtensionMethods()
        {
            Random rand = new Random(0);
            LinkedList<TestClass> ll = new LinkedList<TestClass>();
            ll.AddLast(new TestClass() { Value1 = 2, Value2 = 6 });
            ll.AddLast(new TestClass() { Value1 = 8, Value2 = 1 });
            ll.AddLast(new TestClass() { Value1 = 4, Value2 = 0 });
            ll.AddLast(new TestClass() { Value1 = 0, Value2 = 9 });
            ll.AddLast(new TestClass() { Value1 = 7, Value2 = 5 });
            ll.AddLast(new TestClass() { Value1 = 1, Value2 = 1 });
            ll.AddLast(new TestClass() { Value1 = 5, Value2 = 0 });

            ll.QuickSort(new SortDescending());
            Assert.AreEqual(ll.First.Value.Value1, 8, "LinkedList QuickSort did not sort in Descending order");
            ll.QuickSort(new SortAscending());
            Assert.AreEqual(ll.First.Value.Value1, 0, "LinkedList QuickSort did not sort in Ascending order");

            List<TestClass> lst = new List<TestClass>();
            foreach (TestClass tc in ll)
                lst.Add(tc);

            lst.Shuffle(rand);
            Assert.AreNotEqual(ll.First.Value.Value1, 2, "List Shuffle did not change the order");
        }

        //Private Functions
        private void TestLogger(bool appendDateTime)
        {
            bool subFolderWasBuilt = false;
            LogInfo logInfo = new LogInfo();

            logInfo.RootFolder = Directory.GetCurrentDirectory();
            logInfo.LogFolder = "LogsFolder";
            logInfo.IsDebug = true;
            logInfo.AppendDateTime = appendDateTime;
            logInfo.AppendDateTimeFormat = "yyyy MM dd";
            logInfo.LogFileName = "TestLog.txt";

            //Additional checks for AppendDateTime test
            if (appendDateTime)
            {
                Assert.AreEqual(logInfo.LogFileName, "TestLog " + DateTime.Now.ToString("yyyy MM dd") + ".txt");
            }

            string logFolderFullPath = Path.Combine(logInfo.RootFolder, logInfo.LogFolder);
            if (!Directory.Exists(logFolderFullPath))
                subFolderWasBuilt = true;

            Logger.ValidateLogPath(ref logInfo);

            if (logInfo.PathValidated)
                Logger.Write(logInfo, ALL_CHARS);

            if (File.Exists(logInfo.FullPath))
            {
                string logtext = File.ReadAllText(logInfo.FullPath);

                Assert.IsTrue(logtext.Contains(ALL_CHARS));

                File.Delete(logInfo.FullPath);

                if (subFolderWasBuilt)
                {
                    Directory.Delete(logFolderFullPath);
                }
            }
            else
            {
                Assert.Fail("Full Log Path does not exist. Path: " + logInfo.FullPath);
            }
        }
    }
}
