using Petecat.Data.Formatters;
using Petecat.Service.Datagram;

using System.Text;

namespace Petecat.Service.Client
{
    public class ServiceTcpClientBase
    {
        static ServiceTcpClientBase()
        {
            _Formatter = new JsonFormatter();
        }

        static IObjectFormatter _Formatter = null;

        public ServiceTcpClientBase(string resourceName)
        {
            ResourceName = resourceName;
        }

        public string ResourceName { get; private set; }

        public TResponse Call<TResponse>(object requestBody = null)
        {
            return InternalCall<TResponse>(requestBody, Encoding.UTF8);
        }

        private TResponse InternalCall<TResponse>(object requestBody, Encoding encoding)
        {
            Configuration.ServiceResourceConfig serviceResourceConfig;
            if (!ServiceResourceManager.Instance.TryGetResource(ResourceName, out serviceResourceConfig))
            {
                throw new Errors.ServiceResourceNotFoundException(ResourceName);
            }

            using (var client = new ServiceTcpClientObject(serviceResourceConfig.Address, serviceResourceConfig.Port))
            {
                var request = new ServiceTcpRequestDatagram(requestBody == null ? null : _Formatter.WriteBytes(requestBody),
                    encoding.GetBytes(serviceResourceConfig.ServiceName ?? string.Empty),
                    encoding.GetBytes(serviceResourceConfig.MethodName ?? string.Empty),
                    encoding.GetBytes(serviceResourceConfig.ContentType ?? string.Empty)).Wrap();

                var response = new ServiceTcpResponseDatagram(client.GetResponse(request.Bytes)).Unwrap() as ServiceTcpResponseDatagram;

                if (response.Status == (byte)ServiceTcpResponseStatus.Succeeded)
                {
                    return _Formatter.ReadObject<TResponse>(response.Body, 0, response.Body.Length);
                }
                else
                {
                    throw new Errors.ServiceClientCallingFailedException(ResourceName, response.Status.ToString(),
                        encoding.GetString(response.Body, 0, response.Body.Length));
                }
            }
        }
    }
}
