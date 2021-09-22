using AutoMapper;
namespace Server.Mapper
{
    public static class MapperFactory
    {
        static AutoMapper.Mapper map;
        static MapperFactory()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Server.Models.Comment, DAL.Models.Comment>();
                cfg.CreateMap<Server.Models.Like, DAL.Models.Like>();
                cfg.CreateMap<Server.Models.Topic, DAL.Models.Topic>();
                cfg.CreateMap<Server.Models.User, DAL.Models.ApplicationUser>();
                cfg.CreateMap<DAL.Models.ApplicationUser, Server.Models.User>();
                cfg.CreateMap<DAL.Models.Comment, Server.Models.Comment>();
                cfg.CreateMap<DAL.Models.Topic, Server.Models.Topic>();
                cfg.CreateMap<DAL.Models.Like, Server.Models.Like>();
            });
            map = new AutoMapper.Mapper(config);
        }

        public static AutoMapper.Mapper GetMapper()
        {
            return map;
        }
    }
}