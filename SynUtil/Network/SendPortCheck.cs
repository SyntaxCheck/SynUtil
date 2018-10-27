using System;
using System.Net.Sockets;

namespace SynUtil.Network
{
    public class SendPortCheck
    {
        public string CheckPort(string hostName, int port)
        {
            string rtnTcp = String.Empty;
            string rtnUdp = String.Empty;

            //Check TCP
            rtnTcp = CheckTCP(hostName, port);

            return rtnTcp;

            //Check UDP
            //rtnUdp = CheckUDP(hostName, port);

            //if (rtnTcp.StartsWith("Fail") && rtnUdp.StartsWith("Fail"))
            //{
            //    return "Port Closed";
            //}
            //else
            //{
            //    string rtn = String.Empty;

            //    if (rtnTcp.StartsWith("Success"))
            //    {
            //        rtn += "TCP";
            //    }
            //    if (rtnUdp.StartsWith("Success"))
            //    {
            //        if (!String.IsNullOrEmpty(rtn))
            //            rtn += " and ";

            //        rtn += "UDP";
            //    }

            //    rtn += " Open";

            //    return rtn;
            //}
        }

        private string CheckUDP(string hostName, int port)
        {
            string rtn = String.Empty;

            UdpClient udpClient = new UdpClient(hostName, port);

            try
            {
                udpClient.Connect(hostName, port);
                udpClient.Close();
                rtn = "Success: UDP Port Open";
            }
            catch (Exception ex)
            {
                rtn = "Failure: UDP Port Closed. Exception: " + ex.Message;
            }

            return rtn;
        }
        private string CheckTCP(string hostName, int port)
        {
            string rtn = String.Empty;
            TcpClient tcpClient = new TcpClient();

            try
            {
                tcpClient.ReceiveTimeout = 30000;
                tcpClient.SendTimeout = 30000;
                tcpClient.Connect(hostName, port);
                tcpClient.Close();
                rtn = "Success: TCP Port Open";
            }
            catch (Exception ex)
            {
                rtn = "Failure: TCP Port Closed. Exception: " + ex.Message;
            }

            return rtn;
        }
    }
}
