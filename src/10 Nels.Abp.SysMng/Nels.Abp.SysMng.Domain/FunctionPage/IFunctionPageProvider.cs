namespace Nels.Abp.SysMng.FunctionPage
{
    public interface IFunctionPageProvider
    {
        void PreDefine(IFunctionPageDefinitionContext context);

        void Define(IFunctionPageDefinitionContext context);

        void PostDefine(IFunctionPageDefinitionContext context);
    }
}
