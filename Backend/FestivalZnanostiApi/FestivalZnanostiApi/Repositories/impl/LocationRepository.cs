using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Enums;
using FestivalZnanostiApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FestivalZnanostiApi.Repositories.impl
{
    public class LocationRepository : ILocationRepository
    {

        private readonly FestivalZnanostiContext _context;

        public LocationRepository(FestivalZnanostiContext context)
        {
            _context = context;



        }
        public async Task<Location?> FindById(int locationId)
        {
            return await _context.Location.FindAsync(locationId);
        }



        public async Task<IEnumerable<Location>> GetLocations()
        {
            return await _context.Location.ToListAsync();
        }

        public async Task<IEnumerable<Location>> GetLocationsWithTrackedTimeslots()
        {
            return await _context.Location.Where(l => l.TimeSlotsTracked).ToListAsync();
        }


        public async Task<IEnumerable<Location>> GetChildLocations(int parentId)
        {
            return await _context.Location.Where(l => l.ParentLocationId == parentId).ToListAsync();
        }

        public async Task<IEnumerable<Location>> GetParentLocations()
        {
            return await _context.Location.Where(l => l.ParentLocationId == null).ToListAsync();   // IPAK NE ZA SAD:  id=1 have location Tehnicki muzej „Nikola Tesla“, Savska cesta 18
        }

        public async Task<int> SaveChildLocation(string locationName, int parentId)
        {

            Location childLocation = new Location
            {
                Name = locationName,
                TimeSlotsTracked = false,
                EventDuration = 60,
                ParallelEventCount = 1000,
                ParentLocationId = parentId
            };

            _context.Add(childLocation);
            await _context.SaveChangesAsync();

            return childLocation.Id;
        }

        public async Task<int> SaveParentLocation(string locationName)
        {
            Location parentLocation = new Location
            {
                Name = locationName,
                TimeSlotsTracked = false,
                EventDuration = 60,
                ParallelEventCount = 1000,
                ParentLocationId = null
            };

            _context.Add(parentLocation);
            await _context.SaveChangesAsync();

            return parentLocation.Id;
        }



        public async Task MergeLocations(List<int> locationIds, int mergeIntoLocationId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    locationIds = locationIds.Where(id => id != mergeIntoLocationId).ToList();

                    // Update LocationIds in Events
                    foreach (var locationId in locationIds)
                    {
                        var eventsToUpdate = _context.Event.Where(e => e.LocationId == locationId);
                        foreach (var @event in eventsToUpdate)
                        {
                            @event.LocationId = mergeIntoLocationId;
                        }
                    }

                    // Update ParentLocationIds in Locations
                    foreach (var locationId in locationIds)
                    {
                        var childLocationsToUpdate = _context.Location.Where(l => l.ParentLocationId == locationId);
                        foreach (var childLocation in childLocationsToUpdate)
                        {
                            childLocation.ParentLocationId = mergeIntoLocationId;
                        }
                    }

                    // Remove merged locations
                    var locationsToRemove = _context.Location.Where(l => locationIds.Contains(l.Id));
                    _context.Location.RemoveRange(locationsToRemove);

                    // Save changes
                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    return;
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    throw new Exception("Problem while merging locations! \n" + exception.Message);
                }


            }
        }

    }
}
