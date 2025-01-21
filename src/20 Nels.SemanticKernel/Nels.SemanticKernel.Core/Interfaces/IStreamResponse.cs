using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nels.SemanticKernel.Interfaces;

public interface IStreamResponse
{
    void EnableStream();
    Task WriteMessagAsync(Guid messageId, object content);

    Task WriteDataAsync(string eventType, dynamic data);
}
