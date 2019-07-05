using System.Threading.Tasks;

namespace GrainInterfaces
{
    public interface IHello : Orleans.IGrainWithStringKey
    {
        Task<string> SayHello(string greeting);
    }
}
