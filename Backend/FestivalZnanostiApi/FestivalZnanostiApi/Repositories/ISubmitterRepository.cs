using FestivalZnanostiApi.DTOs;

namespace FestivalZnanostiApi.Repositories
{
    public interface ISubmitterRepository
    {
        public Task<int> SaveSubmitter(SubmitterDto submitter);
    }
}
