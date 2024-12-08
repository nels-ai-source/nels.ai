using System.Collections.Generic;
using System;
using Volo.Abp;
using Nels.Aigc.Entities;

namespace Nels.Aigc.Providers;

public class ModelDefinitionContext(IServiceProvider serviceProvider) : IModelDefinitionContext
{
    public IServiceProvider ServiceProvider { get; } = serviceProvider;
    public List<Model> Models { get; } = [];
    public List<ModelInstance> ModelInstances { get; } = [];

    public void AddModel(Model model)
    {
        Check.NotNull(model, nameof(model));
        Models.Add(model);
    }

    public void AddModelInstance(ModelInstance modelInstance)
    {
        Check.NotNull(modelInstance, nameof(modelInstance));
        ModelInstances.Add(modelInstance);
    }
}
