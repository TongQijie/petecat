using System;

using Petecat.Network.Sockets;
using Petecat.Extension;

namespace Petecat.Service
{
    public class ServiceTcpConnection
    {
        public ServiceTcpConnection(int bufferSize, ITcpClientObject tcpClientObject)
        {
            InternalBuffer = new byte[bufferSize];
            TcpClientObject = tcpClientObject;
            TcpClientObject.ReceivedData += TcpClientObject_ReceivedData;
        }

        public byte[] InternalBuffer { get; private set; }

        public int AvailableCount { get; private set; }

        public ITcpClientObject TcpClientObject { get; private set; }

        public event ServiceRequestArrivalHandlerDelegate ServiceRequestArrival = null;

        private void TcpClientObject_ReceivedData(ISocketObject socketObject, byte[] data, int offset, int count)
        {
            if (count > (InternalBuffer.Length - AvailableCount))
            {
                if (count < InternalBuffer.Length)
                {
                    Array.Copy(data, offset, InternalBuffer, 0, count);
                }
                else
                {
                    throw new Exception("out of buffer size.");
                }
            }
            else
            {
                Array.Copy(data, offset, InternalBuffer, AvailableCount, count);
            }

            AvailableCount += count;

            var request = GenerateRequest();
            if (request != null && ServiceRequestArrival != null)
            {
                ServiceRequestArrival.Invoke(request);
            }
        }

        public ServiceTcpRequest GenerateRequest()
        {
            if (AvailableCount >= 8)
            {
                if (InternalBuffer[0] == 0xFF && InternalBuffer[1] == 0xFE)
                {
                    var size = InternalBuffer[2] + (InternalBuffer[3] << 8) + (InternalBuffer[4] << 16) + (InternalBuffer[5] << 24);

                    if (InternalBuffer[size + 6] == 0xEF && InternalBuffer[size + 7] == 0xFF)
                    {
                        return new ServiceTcpRequest(new Datagram.ServiceTcpRequestDatagram(InternalBuffer.SubArray(0, 8 + size)), this);
                    }
                    else
                    {
                        AvailableCount = 0;
                    }
                }
                else
                {
                    AvailableCount = 0;
                }
            }

            return null;
        }
    }
}
