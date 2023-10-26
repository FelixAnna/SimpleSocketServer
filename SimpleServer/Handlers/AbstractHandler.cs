using Models.dto;
using Serialization;

namespace SimpleServer.Handlers
{
    public abstract class AbstractHandler<TRequest, TResponse, TService> 
        where TRequest : ReqProtoBase 
        where TResponse : RespProtoBase
        where TService : class
    {
        protected readonly TService service;
        protected readonly ISerializeService serializeService;

        public AbstractHandler(TService service, ISerializeService serializeService)
        {
            this.service = service;
            this.serializeService = serializeService;
        }

        public abstract TResponse Handle(TRequest request);
    }
}
