namespace Nels.Aigc.Providers;

public interface IModelProvider
{
    void PreDefine(IModelDefinitionContext context);

    void Define(IModelDefinitionContext context);

    void PostDefine(IModelDefinitionContext context);
}
