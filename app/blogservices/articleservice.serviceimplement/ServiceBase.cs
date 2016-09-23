using ArticleService.ServiceModel.Infrastructure;
using Petecat.Data.Formatters;
using Petecat.Extension;
using Petecat.Logging;

using System;
using System.Text;

namespace ArticleService.ServiceImplement
{
    public class ServiceBase
    {
        static ServiceBase()
        {
            // init
        }

        protected virtual TResponse Sandbox<TRequest, TResponse>(TRequest request, Func<TRequest, TResponse> handler)
            where TRequest : ServiceRequest
            where TResponse : ServiceResponse
        {
            var response = Activator.CreateInstance<TResponse>();

            try
            {
                return handler(request);
            }
            catch (Exception e)
            {
                LoggerManager.GetLogger().LogEvent(handler.Method.Name, LoggerLevel.Error, "unknown error.", 
                    new DataContractJsonFormatter().WriteString(request, Encoding.UTF8), e);
                response.AppendError("999999", "unknown error.");
            }

            return response;
        }
    }
}
