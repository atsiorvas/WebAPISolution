using AutoMapper;
using Common;
using System.Collections.Generic;

namespace Mapper {
    public class NotesMapper : Profile {

        public NotesMapper() {
            this.Apply();
        }

        public override string ProfileName {
            get { return "NotesMappings"; }
        }

        public void Apply() {

            CreateMap<Notes, NotesModel>()
            .ForMember(dest => dest.Text, src => src.MapFrom<string>(srcNote => srcNote.Text))
            .ForMember(dest => dest.Lang, opt => opt.MapFrom<int>(src => src.Lang))
            .ReverseMap()
            .ForMember(dest => dest.Id, src => src.Ignore())
            .ForMember(dest => dest.Text, src => src.MapFrom<string>(srcNote => srcNote.Text))
            .ForMember(dest => dest.Lang, opt => opt.MapFrom<int>(src => src.Lang));

            CreateMap<Notes, long>().ConvertUsing(new NoteToLongConverter());

        }
    }

    public class NoteToLongConverter : ITypeConverter<Notes, long> {

        public long Convert(Notes source, long destination, ResolutionContext context) {
            var noteId = source.Id;
            return System.Convert.ToInt64(noteId);
        }
    }
}

