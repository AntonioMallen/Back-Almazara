using Back_Almazara.DTOS;
using Back_Almazara.Models;
using AutoMapper;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Back_Almazara.Utility.TypeConverter;
using Back_Almazara.ViewModel;

namespace ProyectoJWT.Mapper
{
    public class AutoMapperApp : Profile
    {
        public AutoMapperApp() 
        {
            CreateMap<string, byte[]>().ConvertUsing<StringToByteArrayConverter>();
            CreateMap<byte[], string>().ConvertUsing<ByteArrayToStringConverter>();


            CreateMap<TUser, UsuarioDTO>().ReverseMap();
            CreateMap<TUser, LoginDTO>().ReverseMap();
            CreateMap<TUser, RegisterDTO>().ReverseMap();
            CreateMap<TUser, ChangePassDTO>()
                .ForMember(dest => dest.emailNv, opt => opt.MapFrom(src => src.EmailNv))
                .ForMember(dest => dest.newPassword, opt => opt.MapFrom(src => src.PasswordNv))
                .ReverseMap();

            
            CreateMap<TRole, RoleDTO>().ReverseMap();
            CreateMap<TNotice, NoticeDTO>()
                .ReverseMap();
            CreateMap<TNoticesDetail, NoticeDetailDTO>().ReverseMap();
            CreateMap<TNoticesDetail, NoticeEditDTO>().ReverseMap();
            CreateMap<NoticeDetailViewModel, NoticeEditDTO>().ReverseMap();
            CreateMap<TNotice, NoticeCreateDTO>()
                .ForMember(dest => dest.NameNv, opt => opt.MapFrom(src => src.NameNv))
                .ForMember(dest => dest.DescriptionNv, opt => opt.MapFrom(src => src.DescriptionNv))
                .ForMember(dest => dest.ImageNv, opt => opt.MapFrom(src => src.ImageNv))
                .ReverseMap();
            CreateMap<TFavorite, FavoriteDTO>()
                .ForMember(dest => dest.notice_id_i, opt => opt.MapFrom(src => src.NoticeIdI))
                .ForMember(dest => dest.user_id_i, opt => opt.MapFrom(src => src.UserIdI))
                .ReverseMap();

            CreateMap<TComment, CommentDTO>().ReverseMap();
            CreateMap<CommentViewModel, CommentDTO>().ReverseMap();

        }
    }
}
