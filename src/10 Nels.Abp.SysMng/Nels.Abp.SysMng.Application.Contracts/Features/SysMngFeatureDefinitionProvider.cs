using Volo.Abp.Features;
using Volo.Abp.Validation.StringValues;

namespace Nels.Abp.SysMng.Features
{
    public class SysMngFeatureDefinitionProvider : FeatureDefinitionProvider
    {
        public override void Define(IFeatureDefinitionContext context)
        {
            var sysMngGroup = context.AddGroup("SysMng");
            sysMngGroup.AddFeature(name: "Area", valueType: new SelectionStringValueType());
        }
    }
}
