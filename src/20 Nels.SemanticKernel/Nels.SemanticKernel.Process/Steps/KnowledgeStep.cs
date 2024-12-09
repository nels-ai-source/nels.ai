using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nels.SemanticKernel.Process.Steps;

public class KnowledgeStep : NelsKernelProcessStep<LlmStepState>
{
    private Kernel _kernel { get; set; }
    [KernelFunction(StepTypeConst.Knowledge)]
    public async ValueTask ExecuteAsync(KernelProcessStepContext context, Dictionary<string, object> content, Kernel kernel, CancellationToken cancellationToken)
    {
        _kernel = kernel;
        await base.StepExecuteAsync(context, content, cancellationToken);
    }
}
