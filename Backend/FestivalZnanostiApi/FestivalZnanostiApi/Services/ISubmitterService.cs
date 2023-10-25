using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Models;

namespace FestivalZnanostiApi.Services
{
    public interface ISubmitterService
    {
        public Task<int> CreateSubmitter(SubmitterDto submitter);
    }
}
