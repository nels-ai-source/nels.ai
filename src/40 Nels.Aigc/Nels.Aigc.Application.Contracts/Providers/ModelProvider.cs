using Volo.Abp.DependencyInjection;

namespace Nels.Aigc.Providers
{
    public abstract class ModelProvider : IModelProvider, ITransientDependency
    {
        public virtual void PreDefine(IModelDefinitionContext context) { }
        public abstract void Define(IModelDefinitionContext context);
        public virtual void PostDefine(IModelDefinitionContext context) { }
    }
}
