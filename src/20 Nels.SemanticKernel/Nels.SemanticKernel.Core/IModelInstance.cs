using Nels.SemanticKernel.Enums;
using System;

namespace Nels.SemanticKernel;

public interface IModelInstance
{
    Guid Id { get; set; }
    string Name { get; set; }
    string DeploymentName { get; set; }
    string Endpoint { get; set; }
    string AccessKey { get; set; }
    string SecretKey { get; set; }
    bool IsDefault { get; set; }
    ModelProvider Provider { get; set; }
    ModelType Type { get; set; }
}
