using Nels.Aigc.Entities;

namespace Nels.Aigc.Providers;

public interface IModelDefinitionContext
{
    void AddModel(Model model);
    void AddModelInstance(ModelInstance modelInstance);
}
