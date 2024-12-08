using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nels.Aigc.Dtos;

public class AgentStartRequestDto
{
    public virtual Guid AgentId { get; set; }
    public virtual string UserInput { get; set; } = string.Empty;
    public virtual bool Streaming { get; set; }
}
