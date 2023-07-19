using AutoMapper;
using Rest.Dto;
using Rest.Models;

namespace Rest.Helper;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Pokemon, PokemonDto>();
        CreateMap<Category, CategoryDto>();
        CreateMap<Country, CountryDto>();
        CreateMap<Owner, OwnerDto>();
        CreateMap<Review, ReviewDto>();
        CreateMap<Reviewer, ReviewerDto>();
        CreateMap<PokemonDto, Pokemon>();
        CreateMap<CategoryDto, Category>();
        CreateMap<CountryDto, Country>();
        CreateMap<OwnerDto, Owner>();
        CreateMap<ReviewDto, Review>();
        CreateMap<ReviewerDto, Reviewer>();
    }
}