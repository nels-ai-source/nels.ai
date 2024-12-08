using AutoMapper;
using Nels.Abp.SysMng.FunctionPage;
using Nels.Abp.SysMng.Identity;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;
using IdentityRoleCreateDto = Nels.Abp.SysMng.Identity.IdentityRoleCreateDto;
using IdentityRoleDto = Nels.Abp.SysMng.Identity.IdentityRoleDto;
using IdentityRoleUpdateDto = Nels.Abp.SysMng.Identity.IdentityRoleUpdateDto;
using IdentityUserCreateDto = Nels.Abp.SysMng.Identity.IdentityUserCreateDto;
using IdentityUserDto = Nels.Abp.SysMng.Identity.IdentityUserDto;
using IdentityUserUpdateDto = Nels.Abp.SysMng.Identity.IdentityUserUpdateDto;

namespace Nels.Abp.SysMng;

public class SysMngApplicationAutoMapperProfile : Profile
{
    public SysMngApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        #region functionPage
        CreateMap<App, AppDto>().ReverseMap();
        CreateMap<App, BusinessUnitOutputDto>()
            .Ignore(dest => dest.ParentId)
            .Ignore(dest => dest.Children);

        CreateMap<BusinessUnit, BusinessUnitDto>().ReverseMap();
        CreateMap<BusinessUnit, BusinessUnitOutputDto>()
            .ForMember(dest => dest.ParentId, opt => opt.MapFrom(x => x.ApplicationId))
            .Ignore(dest => dest.Children);



        CreateMap<BusinessUnit, MenuDto>()
           .ForMember(dest => dest.Meta, opt => opt.MapFrom(x => x))
           .ForMember(dest => dest.Component, opt => opt.MapFrom(x => x.Name))
           .ForMember(dest => dest.Path, opt => opt.MapFrom(x => x.Name))
           .Ignore(dest => dest.ParentId)
           .Ignore(dest => dest.Children);

        CreateMap<BusinessUnit, MenuMetaDto>()
           .ForMember(dest => dest.Title, opt => opt.MapFrom(x => x.DisplayName))
           .Ignore(dest => dest.Hidden)
           .Ignore(dest => dest.Affix)
           .Ignore(dest => dest.Type)
           .Ignore(dest => dest.HiddenBreadcrumb)
           .Ignore(dest => dest.Active)
           .Ignore(dest => dest.Fullpage)
           .Ignore(dest => dest.Color)
           .Ignore(dest => dest.Role);

        CreateMap<Page, PageDto>().ReverseMap();

        CreateMap<Page, MenuDto>()
          .ForMember(dest => dest.Meta, opt => opt.MapFrom(x => x))
          .ForMember(dest => dest.ParentId, opt => opt.MapFrom(x => x.BusinessUnitId))
          .ForMember(dest => dest.Component, opt => opt.MapFrom(x => x.Path.Substring(1)))
          .Ignore(dest => dest.Children);

        CreateMap<Page, MenuMetaDto>()
           .ForMember(dest => dest.Title, opt => opt.MapFrom(x => x.DisplayName))
           .Ignore(dest => dest.Color)
           .Ignore(dest => dest.Role);


        CreateMap<OrganizationUnit, OrganizationUnitDto>()
             .ForMember(dest => dest.Children, opt => opt.Ignore())
             .ReverseMap();

        CreateMap<IdentityUser, IdentityUserCreateDto>()
            .Ignore(x => x.Password)
            .Ignore(x => x.RoleNames);
        CreateMap<IdentityUser, IdentityUserUpdateDto>()
            .Ignore(x => x.Password)
            .Ignore(x => x.RoleNames);
        CreateMap<IdentityUser, IdentityUserDto>().ReverseMap();

        CreateMap<IdentityRole, IdentityRoleCreateDto>().ReverseMap();
        CreateMap<IdentityRole, IdentityRoleUpdateDto>().ReverseMap();
        CreateMap<IdentityRole, IdentityRoleDto>().ReverseMap();

        CreateMap<BizParameter, BizParameterDto>().ReverseMap();
        CreateMap<BizParameterValue, BizParameterValueDto>().ReverseMap();
        #endregion
    }
}
