using System.Threading.Tasks;

namespace SIS.HTTP
{

    public interface IHttpServer
    {
        Task StartAsynk();
        Task ResetAsynk();
        void Stop();
    }
}
