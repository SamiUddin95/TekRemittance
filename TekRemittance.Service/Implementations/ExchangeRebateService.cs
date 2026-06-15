using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Repository.Models.dto;
using TekRemittance.Service.Interfaces;

namespace TekRemittance.Service.Implementations
{
    public class ExchangeRebateService : IExchangeRebateService
    {
        private readonly IExchangeRebateRepository _repository;
        public ExchangeRebateService(IExchangeRebateRepository repository)
        {
            _repository = repository;
        }
        public async Task<ExchangeRebateResultDto> GetExchangeRebateAsync(ExchangeRebateRequestDTO request)
        {
            return await _repository.GetExchangeRebateAsync(request);
        }
    }
}
