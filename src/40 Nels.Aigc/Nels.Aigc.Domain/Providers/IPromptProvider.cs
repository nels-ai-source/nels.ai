namespace Nels.Aigc.Providers
{
    public interface IPromptProvider
    {
        void PreDefine(IPromptDefinitionContext context);

        void Define(IPromptDefinitionContext context);

        void PostDefine(IPromptDefinitionContext context);
    }
}
