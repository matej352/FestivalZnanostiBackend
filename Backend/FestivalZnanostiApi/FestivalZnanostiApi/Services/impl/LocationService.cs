using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.DTOs.Extensions;
using FestivalZnanostiApi.Repositories;

namespace FestivalZnanostiApi.Services.impl
{
    public class LocationService : ILocationService
    {

        private readonly ILocationRepository _locationRepository;

        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<LocationDto> CreateChildLocation(string locationName, int parentId)
        {
            var parent = await _locationRepository.FindById(parentId);
            if (parent == null)
            {
                throw new Exception($"Parent location with id = {parentId} does not exist!");
            }

            if (parent.ParentLocationId != null)
            {
                throw new Exception($"Location with id = {parentId} is not parent location!");
            }

            var newLocationId = await _locationRepository.SaveChildLocation(locationName, parentId);

            var newLocation = await _locationRepository.FindById(newLocationId);

            return newLocation!.AsLocationDto();
        }

        public async Task<LocationDto> CreateParentLocation(string locationName)
        {
            var newLocationId = await _locationRepository.SaveParentLocation(locationName);

            var newLocation = await _locationRepository.FindById(newLocationId);

            return newLocation!.AsLocationDto();
        }
        public async Task<IEnumerable<LocationDto>> GetLocations()
        {
            var locations = await _locationRepository.GetLocations();

            List<LocationDto> locationDtoList = locations.Select(l => l.AsLocationDto()).ToList();

            return locationDtoList;
        }


        public async Task<IEnumerable<LocationDto>> GetChildLocations(int parentLocationId)
        {
            var parent = await _locationRepository.FindById(parentLocationId);
            if (parent == null)
            {
                throw new Exception($"Parent location with id = {parentLocationId} does not exist!");
            }

            var locations = await _locationRepository.GetChildLocations(parentLocationId);

            List<LocationDto> locationDtoList = locations.Select(l => l.AsLocationDto()).ToList();

            return locationDtoList;

        }

        public async Task<IEnumerable<LocationDto>> GetParentLocations()
        {
            var locations = await _locationRepository.GetParentLocations();

            List<LocationDto> locationDtoList = locations.Select(l => l.AsLocationDto()).ToList();

            return locationDtoList;
        }

        public async Task MergeLocations(List<int> locationIds, int mergeIntoLocationId)
        {
            if (locationIds.Contains(mergeIntoLocationId))
            {

                var unmergableLocationIds = (await _locationRepository.GetLocationsWithTrackedTimeslots()).Select(location => location.Id).Append(1);

                bool unmergableLocationIdFound = unmergableLocationIds.Intersect(locationIds).Any();

                if (unmergableLocationIdFound)
                {
                    throw new Exception("Locations you are trying to merge can not be merged!");
                }

                var mergeIntoLocation = await _locationRepository.FindById(mergeIntoLocationId);

                if (mergeIntoLocation == null)
                {
                    throw new Exception("Location you are trying to merge other locations in does not exist!");
                }

                await _locationRepository.MergeLocations(locationIds, mergeIntoLocationId);


            }
            else
            {
                throw new Exception("No intersection between locationIds and mergeIntoLocationId!");
            }
        }

        public async Task<LocationDto> GetLocation(int id)
        {
            var location = await _locationRepository.FindById(id);

            if (location == null)
            {
                throw new Exception($"Location with id = {id} does not exist!");
            }

            return location.AsLocationDto();

        }
    }
}
