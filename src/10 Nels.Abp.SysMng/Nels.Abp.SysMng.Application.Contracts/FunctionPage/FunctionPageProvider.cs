using Volo.Abp.DependencyInjection;

namespace Nels.Abp.SysMng.FunctionPage
{
    public abstract class FunctionPageProvider : IFunctionPageProvider, ITransientDependency
    {
        public virtual void PreDefine(IFunctionPageDefinitionContext context) { }
        public abstract void Define(IFunctionPageDefinitionContext context);
        public virtual void PostDefine(IFunctionPageDefinitionContext context) { }
    }
}
