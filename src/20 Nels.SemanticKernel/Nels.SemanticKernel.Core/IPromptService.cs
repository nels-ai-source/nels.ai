using System;
using System.Threading.Tasks;

namespace Nels.SemanticKernel;

public interface IPromptService
{
    Task<IPrompt> Get(Guid id);
}
