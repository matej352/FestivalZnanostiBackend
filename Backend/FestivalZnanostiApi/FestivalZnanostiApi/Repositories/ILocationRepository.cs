using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Models;

namespace FestivalZnanostiApi.Repositories
{
    public interface ILocationRepository
    {
        public Task<IEnumerable<Location>> GetLocations();

        public Task<IEnumerable<Location>> GetLocationsWithTrackedTimeslots();

        public Task<IEnumerable<Location>> GetParentLocations();

        public Task<IEnumerable<Location>> GetChildLocations(int parentId);

        //public Task<IEnumerable<Location>> GetTrackableChildLocations();

        public Task<int> SaveParentLocation(string locationName);

        public Task<int> SaveChildLocation(string locationName, int parentId);

        public Task<Location?> FindById(int locationId);

        public Task MergeLocations(List<int> locationIds, int mergeIntoLocationId);

    }
}
