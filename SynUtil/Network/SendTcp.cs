using SynUtil.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SynUtil.Network
{
    public class SendTcp
    {
        public SendTcp()
        {
        }

        public string Send(LogInfo logInfo, string protocol, string ipAddress, int port, string message, int timeoutMilliseconds, int readTimeoutMilliseconds, Encoding sendEncoding, long bufferSize, bool expectResponse)
        {
            string rtn = String.Empty;
            TcpClient client = new TcpClient(ipAddress, port); //Class built into .Net.Sockets
            NetworkStream stream = client.GetStream(); //Get the client stream, we can read/write to it similar to files

            try
            {
                Logger.WriteDbg(logInfo, "Send TCP Message settings");
                Logger.WriteDbg(logInfo, "Protocol: " + protocol + ", IP Address: " + ipAddress + ", Port: " + port.ToString() + ", Message: " + message + ", Timeout: " + timeoutMilliseconds.ToString() + ", Read Timeout: " + readTimeoutMilliseconds.ToString() + ", Encoding: " + sendEncoding.ToString() + ", Expect Response: " + expectResponse.ToString());

                client.ReceiveTimeout = timeoutMilliseconds;
                client.SendTimeout = timeoutMilliseconds;

                //Get the encoding bytes from the message passed in, TCP transmits bytes
                byte[] data = sendEncoding.GetBytes(message);

                //write the message to the stream
                //TODO Send Should Respect Buffer Size
                Logger.WriteDbg(logInfo, "Writing data to the network stream...");
                stream.WriteTimeout = timeoutMilliseconds;
                stream.Write(data, 0, data.Length);
                Logger.WriteDbg(logInfo, "Data successfully written to the network stream");

                if (expectResponse)
                {
                    Logger.WriteDbg(logInfo, "TCP Sender Expect response enabled");
                    //Now we need to receive the response
                    data = new byte[bufferSize];

                    string response = String.Empty;

                    stream.ReadTimeout = readTimeoutMilliseconds; //set timeout, on the final iteration we will timeout. This is how we know the transmission is complete
                    MemoryStream memStream = new MemoryStream();
                    Logger.WriteDbg(logInfo, "Reading data from the network stream...");
                    int bytes = stream.Read(data, 0, data.Length);
                    if (bytes > 0)
                        Logger.WriteDbg(logInfo, "Data successfully read from the network stream. Number of bytes read: " + bytes.ToString());

                    while (bytes > 0)
                    {
                        try
                        {
                            memStream.Write(data, 0, bytes);
                            bytes = stream.Read(data, 0, data.Length);

                            if (bytes > 0)
                                Logger.WriteDbg(logInfo, "Data successfully read from the network stream. Number of bytes read: " + bytes.ToString());

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
                    Logger.WriteDbg(logInfo, "Finished reading response");

                    response = sendEncoding.GetString(memStream.ToArray());

                    rtn = response;
                }
                else
                {
                    Logger.WriteDbg(logInfo, "TCP Sender not set to Expect response");
                }
            }
            catch (IOException e)
            {
                var socketExept = e.InnerException as SocketException;
                if (socketExept == null || socketExept.ErrorCode != 10060)
                {
                    string exConcat = "IO Exception. Message: " + e.Message + Environment.NewLine + "Inner Exception: " + e.InnerException + Environment.NewLine + "Stack Trace: " + e.StackTrace;
                    Logger.WriteDbg(logInfo, exConcat);
                    rtn = exConcat;
                }
                else
                {
                    string exConcat = "IO Timeout Exception. Message: " + e.Message + Environment.NewLine + "Inner Exception: " + e.InnerException + Environment.NewLine + "Stack Trace: " + e.StackTrace;
                    Logger.WriteDbg(logInfo, exConcat);
                    rtn = exConcat;
                }
            }
            catch (ArgumentNullException e)
            {
                string exConcat = "Argument Null Exception. Message: " + e.Message + Environment.NewLine + "Inner Exception: " + e.InnerException + Environment.NewLine + "Stack Trace: " + e.StackTrace;
                Logger.WriteDbg(logInfo, exConcat);
                rtn = exConcat;
            }
            catch (SocketException e)
            {
                string exConcat = "Socket Exception. Message: " + e.Message + Environment.NewLine + "Inner Exception: " + e.InnerException + Environment.NewLine + "Stack Trace: " + e.StackTrace;
                Logger.WriteDbg(logInfo, exConcat);
                rtn = exConcat;
            }
            catch (Exception e)
            {
                string exConcat = "Generic Exception. Message: " + e.Message + Environment.NewLine + "Inner Exception: " + e.InnerException + Environment.NewLine + "Stack Trace: " + e.StackTrace;
                Logger.WriteDbg(logInfo, exConcat);
                rtn = exConcat;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close(); //close network stream
                }
                if (client != null)
                {
                    client.Close(); //close the TcpClient
                }
            }

            Logger.WriteDbg(logInfo, "End Sending TCP Message");

            return rtn;
        }
    }
}
