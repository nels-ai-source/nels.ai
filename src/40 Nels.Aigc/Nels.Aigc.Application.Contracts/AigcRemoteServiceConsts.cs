using Nels.Abp.Ddd.Application.Contracts;

namespace Nels.Aigc;

public class AigcRemoteServiceConsts : RemoteServiceConsts
{
    public const string RemoteServiceName = "Aigc";


    public const string promptRoute = "api/prompt";
    public const string modelInstanceRoute = "api/modelInstance";
    public const string modelRoute = "api/model";

    public const string agentRoute = "api/agent";

    public const string spaceRoute = "api/space";
    public const string knowledgeRoute = "api/knowledge";
    public const string knowledgeDocumentRoute = "api/knowledgeDocument";
}
