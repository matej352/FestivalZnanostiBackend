using FestivalZnanostiApi.DTOs;
using FestivalZnanostiApi.Models;
using FestivalZnanostiApi.Repositories;

namespace FestivalZnanostiApi.Services.impl
{
    public class SubmitterService : ISubmitterService
    {

        private readonly ISubmitterRepository _repository;



        public SubmitterService(ISubmitterRepository repository)
        {
            _repository = repository;
        }


        public async Task<int> CreateSubmitter(SubmitterDto submitter)
        {
            return await _repository.SaveSubmitter(submitter);
        }
    }
}
