namespace Nels.Abp.SysMng.FunctionPage;

public class BusinessUnitGetDto 
{
    public virtual string Name { get; set; } = string.Empty;

    public virtual string DisplayName { get; set; } = string.Empty;

    public virtual bool IsEnable { get; set; } = true;

}
