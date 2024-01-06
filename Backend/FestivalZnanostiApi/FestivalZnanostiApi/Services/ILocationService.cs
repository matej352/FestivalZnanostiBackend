using FestivalZnanostiApi.DTOs;

namespace FestivalZnanostiApi.Services
{
    public interface ILocationService
    {
        public Task<IEnumerable<LocationDto>> GetLocations();

        public Task<IEnumerable<LocationDto>> GetParentLocations();

        public Task<IEnumerable<LocationDto>> GetChildLocations(int parentId);

        public Task<LocationDto> GetLocation(int id);

        public Task<LocationDto> CreateParentLocation(string locationName);

        public Task<LocationDto> CreateChildLocation(string locationName, int parentId);

        public Task MergeLocations(List<int> locationIds, int mergeIntoLocationId);
    }
}
