using AutoMapper;
using Ex04.API.DTO;
using Ex04.Models;

namespace Ex04.API.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryDTO, Category>().ForMember(des => des.CreatedAt, opt => opt.Ignore()).ReverseMap();
            CreateMap<PostDTO, Post>().ForMember(des => des.CreatedAt, opt => opt.Ignore()).ReverseMap();
            CreateMap<User, RegisterDTO>().ReverseMap();
            CreateMap<Image, ImageDTO>()
                .ForMember(des => des.ImageCateIds, opt => opt.Ignore())
                .ForMember(des => des.UploadImage, opt => opt.Ignore());
            CreateMap<ImageDTO, Image>().ForMember(des=>des.CreatedAt, opt => opt.Ignore());
            CreateMap<ImageCategoryDTO, ImageCategory>().ForMember(des=>des.CreatedAt, opt=>opt.Ignore()).ReverseMap();
            CreateMap<CommentDTO, Comment>().ForMember(des => des.CreatedAt, opt => opt.Ignore()).ReverseMap();
            CreateMap<RateDTO, Rate>().ForMember(des => des.CreatedAt, opt => opt.Ignore()).ReverseMap();
        }
    }
}
