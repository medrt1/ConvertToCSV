using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAutomapper
{
    public class ConvertToCSV
    {
        public static readonly IMapper mapper;

        static ConvertToCSV()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<Member, PresenceCalendar>()
                .ForMember(dest => dest.MemberID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Monday, opt => opt.MapFrom(src => src.DaysOfPresence.Contains(DayOfWeek.Monday) ? "P" : ""))
                .ForMember(dest => dest.Tuesday, opt => opt.MapFrom(src => src.DaysOfPresence.Contains(DayOfWeek.Tuesday) ? "P" : ""))
                .ForMember(dest => dest.Wednesday, opt => opt.MapFrom(src => src.DaysOfPresence.Contains(DayOfWeek.Wednesday) ? "P" : ""))
                .ForMember(dest => dest.Thursday, opt => opt.MapFrom(src => src.DaysOfPresence.Contains(DayOfWeek.Thursday) ? "P" : ""))
                .ForMember(dest => dest.Friday, opt => opt.MapFrom(src => src.DaysOfPresence.Contains(DayOfWeek.Friday) ? "P" : ""))
                .ForMember(dest => dest.Saturday, opt => opt.MapFrom(src => src.DaysOfPresence.Contains(DayOfWeek.Saturday) ? "P" : ""))
                .ForMember(dest => dest.Sunday, opt => opt.MapFrom(src => src.DaysOfPresence.Contains(DayOfWeek.Sunday) ? "P" : ""));

                cfg.CreateMap<List<Team>, List<PresenceCalendar>>().ConvertUsing<TeamsPresenceConverter>();

            });

            mapper = mapperConfiguration.CreateMapper();
        }

        private class TeamsPresenceConverter : ITypeConverter<List<Team>, List<PresenceCalendar>>
        {
            public List<PresenceCalendar> Convert(List<Team> source, List<PresenceCalendar> destination, ResolutionContext context)
            {
                destination = new List<PresenceCalendar>();
                foreach (Team team in source)
                {
                    foreach (Member member in team.Members)
                    {
                        PresenceCalendar memberPresence = context.Mapper.Map<PresenceCalendar>(member);
                        memberPresence.TeamID = team.Id;
                        memberPresence.TeamDescription = team.Description;

                        destination.Add(memberPresence);
                    }
                }

                return destination;
            }
        }

        public string TransformToCSV(List<Team> teams)
        {
            StringBuilder sbCSV = new StringBuilder();
            List<PresenceCalendar> calenderDTO = mapper.Map<List<PresenceCalendar>>(teams);
            
            string titleLine = string.Join(",", "TeamID","Team Description", "MemberID", "First Name", "Last Name"
            , "Monday", "Tuesday", "Wednesday", "Friday", "Saturday", "Sunday");
            sbCSV.AppendLine(titleLine);

            foreach (PresenceCalendar calendar in calenderDTO)
            {
                string line = string.Join(",",calendar.TeamID, calendar.TeamDescription,calendar.MemberID,calendar.FirstName,calendar.LastName
                ,calendar.Monday, calendar.Tuesday, calendar.Wednesday, calendar.Friday,calendar.Saturday,calendar.Sunday);
                sbCSV.AppendLine(line);
            }
            return sbCSV.ToString();
        }
    }

}
