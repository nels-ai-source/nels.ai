using System;

namespace Nels.Aigc.Dtos;

public class GetSpaceUserRequestDto
{
    public Guid SpaceId { get; set; }
    public string? UserName { get; set; }
}
public class GetSpaceUserResponseDto
{
    public Guid Id { get; set; }
    public Guid SpaceId { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string UserName { set; get; }
    public DateTime CreationTime { get; set; }
    public bool IsOwner { get; set; }
}
