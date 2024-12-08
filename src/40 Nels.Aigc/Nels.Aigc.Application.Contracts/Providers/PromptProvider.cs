using Volo.Abp.DependencyInjection;

namespace Nels.Aigc.Providers
{
    public abstract class PromptProvider : IPromptProvider, ITransientDependency
    {
        public virtual void PreDefine(IPromptDefinitionContext context) { }
        public abstract void Define(IPromptDefinitionContext context);
        public virtual void PostDefine(IPromptDefinitionContext context) { }
    }
}
