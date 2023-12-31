﻿using _90Zone.BusinessObjects.Models;


namespace _90Zone.Repositories {
    public interface ICountryRepository {
        ICollection<Country> GetCountries();
        Country GetCountry(int id);
        ICollection<League> GetLeagueByCountry(int countryId);
        bool CreateCountry(Country country);
        bool EditCountry(int id, Country updateCountryRequest);
        bool DeleteCountry(int id);
        bool CountryExist(int id);
    }
}
