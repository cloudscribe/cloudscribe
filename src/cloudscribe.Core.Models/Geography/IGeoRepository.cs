using System;
using System.Collections.Generic;

namespace cloudscribe.Core.Models.Geography
{
    public interface IGeoRepository
    {
        bool DeleteCountry(Guid guid);
        bool DeleteGeoZone(Guid guid);
        IGeoCountry FetchCountry(Guid guid);
        IGeoZone FetchGeoZone(Guid guid);
        List<IGeoCountry> GetAllCountries();
        List<IGeoCountry> GetCountriesPage(int pageNumber, int pageSize, out int totalPages);
        int GetCountryCount();
        int GetGeoZoneCount(Guid countryGuid);
        List<IGeoZone> GetGeoZonePage(Guid countryGuid, int pageNumber, int pageSize, out int totalPages);
        List<IGeoZone> GetGeoZonesByCountry(Guid countryGuid);
        void Save(IGeoCountry geoCountry);
        void Save(IGeoZone geoZone);
    }
}
