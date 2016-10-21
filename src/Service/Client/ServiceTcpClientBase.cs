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

        public TResponse Call<TResponse>()
        {
            Configuration.ServiceResourceConfig serviceResourceConfig;
            if (!ServiceResourceManager.Instance.TryGetResource(ResourceName, out serviceResourceConfig))
            {
                throw new Errors.ServiceResourceNotFoundException(ResourceName);
            }

            using(var client = new ServiceTcpClientObject(serviceResourceConfig.Host, serviceResourceConfig.Port))
            {
                var request = new ServiceTcpRequestDatagram(null,
                    EncodeString(serviceResourceConfig.ServiceName ?? string.Empty),
                    EncodeString(serviceResourceConfig.MethodName ?? string.Empty),
                    EncodeString(serviceResourceConfig.ContentType ?? string.Empty));
                request.Wrap();

                var response = new ServiceTcpResponseDatagram(client.GetResponse(request.Bytes));
                response.Unwrap();

                if (response.Status == 0x66)
                {
                    return _Formatter.ReadObject<TResponse>(response.Body, 0, response.Body.Length);
                }
                else
                {
                    throw new Errors.ServiceClientCallingFailedException(ResourceName, response.Status.ToString(), 
                        Encoding.UTF8.GetString(response.Body, 0, response.Body.Length));
                }
            }
        }

        public TResponse Call<TResponse>(object requestBody)
        {
            Configuration.ServiceResourceConfig serviceResourceConfig;
            if (!ServiceResourceManager.Instance.TryGetResource(ResourceName, out serviceResourceConfig))
            {
                throw new Errors.ServiceResourceNotFoundException(ResourceName);
            }

            using (var client = new ServiceTcpClientObject(serviceResourceConfig.Host, serviceResourceConfig.Port))
            {
                var request = new ServiceTcpRequestDatagram(EncodeObject(requestBody),
                    EncodeString(serviceResourceConfig.ServiceName ?? string.Empty),
                    EncodeString(serviceResourceConfig.MethodName ?? string.Empty),
                    EncodeString(serviceResourceConfig.ContentType ?? string.Empty));
                request.Wrap();

                var response = new ServiceTcpResponseDatagram(client.GetResponse(request.Bytes));
                response.Unwrap();

                if (response.Status == 0x66)
                {
                    return _Formatter.ReadObject<TResponse>(response.Body, 0, response.Body.Length);
                }
                else
                {
                    throw new Errors.ServiceClientCallingFailedException(ResourceName, response.Status.ToString(),
                        Encoding.UTF8.GetString(response.Body, 0, response.Body.Length));
                }
            }
        }

        public byte[] EncodeString(string stringValue)
        {
            return Encoding.UTF8.GetBytes(stringValue);
        }

        public byte[] EncodeObject(object objectValue)
        {
            return _Formatter.WriteBytes(objectValue);
        }
    }
}
