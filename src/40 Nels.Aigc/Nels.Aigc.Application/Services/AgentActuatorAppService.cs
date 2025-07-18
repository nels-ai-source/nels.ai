using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nels.SemanticKernel.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Nels.Aigc.Services;

[Route(AigcRemoteServiceConsts.agentActuatorRoute)]
public class AgentActuatorAppService(AgentActuator agentActuator) : AigcAppService
{
    [HttpPost]
    [Route("[action]")]
    public virtual async Task InvokeStreamingAsync([FromBody] ChatRequest request, CancellationToken cancellation = default)
    {
        await agentActuator.InvokeStreamingAsync(request, cancellation);
    }
}
