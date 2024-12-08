using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Nels.SemanticKernel.Process.Interfaces;
using Nels.SemanticKernel.Process.Services;

namespace Nels.SemanticKernel.Process;

public class KernelProcessOptions(IServiceCollection services, IProceessSerializer proceessSerializer, ProcessStateTypeResolver processStateType)
{
    private readonly ProcessStateTypeResolver _processStateTypeResolver = processStateType;


    public IServiceCollection Services { get; } = services;
    public IProceessSerializer ProceessSerializer { get; } = proceessSerializer;

    public void RegisterKernelProcessStepType<T>() where T : KernelProcessStep
    {
        _processStateTypeResolver.RegisterKernelProcessStepType<T>();
    }
}
