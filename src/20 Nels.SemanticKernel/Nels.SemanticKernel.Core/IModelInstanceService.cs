using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nels.SemanticKernel
{
    public interface IModelInstanceService
    {
        Task<List<IModelInstance>> GetAllInstances();
    }
}
