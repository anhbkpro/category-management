using AutoMapper;


  public class MappingProfile : Profile
  {
      public MappingProfile()
      {
          // Domain to DTO mappings
          CreateMap<Category, CategoryDto>();
          CreateMap<CategoryCondition, CategoryConditionDto>();
          CreateMap<Session, SessionDto>()
              .ForMember(dest => dest.Tags, opt => opt.MapFrom(src =>
                  src.SessionTags.Select(st => st.Tag.Name)))
              .ForMember(dest => dest.Speakers, opt => opt.MapFrom(src =>
                  src.SessionSpeakers.Select(ss => ss.Speaker)));
          CreateMap<Speaker, SpeakerDto>();

          // DTO to Domain mappings
          CreateMap<CategoryDto, Category>();
          CreateMap<CategoryConditionDto, CategoryCondition>();
          CreateMap<SessionDto, Session>();
          CreateMap<SpeakerDto, Speaker>();
          CreateMap<Tag, TagDto>();
          CreateMap<TagDto, Tag>();
      }
  }
