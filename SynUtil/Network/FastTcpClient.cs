using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SynUtil.Network
{
    public class FastTcpClient
    {
        public string Hostname { get; set; }
        public int Port { get; set; }
        public int SendTimeout { get; set; }
        public int ReadTimeout { get; set; }
        public long BufferSize { get; set; }
        public bool ExpectResponse{ get; set; }
        public Encoding SendEncoding { get; set; }
        public TcpClient Client { get; set; }
        public NetworkStream Stream { get; set; }

        public FastTcpClient()
        {
        }
        public FastTcpClient(string hostname, int port, Encoding sendEncoding, int sendTimeout = 5000, int readTimeout = 5000, long bufferSize = 32768, bool expectResponse = true)
        {
            Hostname = hostname;
            Port = port;
            SendEncoding = sendEncoding;
            SendTimeout = sendTimeout;
            ReadTimeout = readTimeout;
            BufferSize = bufferSize;
            ExpectResponse = expectResponse;
        }

        public void Connect()
        {
            Client = new TcpClient();
            Client.ReceiveTimeout = ReadTimeout;
            Client.SendTimeout = SendTimeout;
            //Client.Connect(Hostname, Port);

            if (!Client.ConnectAsync(Hostname, Port).Wait(5000))
            {
                throw new Exception("Failed to connect to Host " + Hostname + " over port " + Port);
            }
        }
        public void Disconnect()
        {
            if (Stream != null)
            {
                Stream.Close(); //close network stream
            }
            if (Client != null)
            {
                Client.Close();
            }
        }
        public string SendData(string message)
        {
            string rtn = String.Empty;

            Connect();
            if (Client.Connected)
            {
                Stream = Client.GetStream();
                Stream.WriteTimeout = SendTimeout;
                Stream.ReadTimeout = ReadTimeout;

                byte[] data = SendEncoding.GetBytes(message);
                Stream.Write(data, 0, data.Length);

                if (ExpectResponse)
                {
                    data = new byte[BufferSize];
                    MemoryStream memStream = new MemoryStream();

                    int bytes = Stream.Read(data, 0, data.Length);

                    while (bytes > 0)
                    {
                        try
                        {
                            memStream.Write(data, 0, bytes);
                            bytes = Stream.Read(data, 0, data.Length);
                        }
                        catch (IOException ex)
                        {
                            var socketExept = ex.InnerException as SocketException;
                            if (socketExept == null || socketExept.ErrorCode != 10060)
                            {
                                // if it's not the "expected" exception, let's not hide the error
                                throw ex;
                            }

                            bytes = 0; //Found success message, end reading
                        }
                    }

                    rtn = SendEncoding.GetString(memStream.ToArray());
                    memStream.Close();
                    memStream.Dispose();
                }

                Stream.Close();
            }
            Disconnect();

            return rtn;
        }
    }
}
