using Nels.SemanticKernel.Enums;
using System;

namespace Nels.SemanticKernel;

public interface IModel
{
    Guid Id { get; set; }
    string Name { get; set; }
    string DeploymentName { get; set; }
    string Endpoint { get; set; }
    string AccessKey { get; set; }
    string SecretKey { get; set; }
    bool IsEnabled { get; set; }
    int? MaxTokens { get; set; }
    ModelProvider Provider { get; set; }
    ModelType Type { get; set; }
    ModelConnector Connector { get; set; }
}
