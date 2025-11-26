using HotelAPI.Models.Dto;
using HotelAPI.Models.Entities;
using Mapster;
using System.Reflection;

namespace HotelAPI.Models
{
    public static class MapsterConfig
    {
        public static void RegisterMapsterConfiguration(this IServiceCollection services)
        {
            TypeAdapterConfig<Hotel, GetHotelDto>
                .NewConfig()
                .Map(dest => dest.MjestoNaziv, src => src.Mjesto != null ? src.Mjesto.Naziv : null)
                .Map(dest => dest.StatusHotela, src => src.StatusHotela != null ? src.StatusHotela.Naziv : null);

            TypeAdapterConfig<Soba, GetSobaDto>
                .NewConfig()
                .Map(dest => dest.HotelNaziv, src => src.Hotel != null ? src.Hotel.Naziv : null)
                .Map(dest => dest.UrlSlike, src => src.UrlSlike != null ? src.UrlSlike : null);

            TypeAdapterConfig<Korisnik, GetKorisnikDto>
                .NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Username, src => src.UserName)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.Ime, src => src.Ime)
                .Map(dest => dest.Prezime, src => src.Prezime);

            TypeAdapterConfig<SlikaSobe, GetSlikaSobeDto>
                .NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.UrlSlike, src => src.UrlSlike)
                .Map(dest => dest.NaslovSlike, src => src.NaslovSlike)
                .Map(dest => dest.OpisSlike, src => src.OpisSlike)
                .Map(dest => dest.SobaId, src => src.Id)
                .Map(dest => dest.Id, src => src.SobaId);

            TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
        } 
    }
}
