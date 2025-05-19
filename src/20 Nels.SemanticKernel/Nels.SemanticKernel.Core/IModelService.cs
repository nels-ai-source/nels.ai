using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nels.SemanticKernel
{
    public interface IModelService
    {
        Task<List<IModel>> GetAllModels();
    }
}
