using Module_05.Requests;
using Module_05.Responses;

namespace Module_05.Services;

public interface IBiddingService
{
    Task<BidResponse> CreateBidAsync(CreateBidRequest request);
    Task<BidResponse?> GetBidAsync(Guid id);
    Task<List<BidResponse>> GetAllBidsAsync();
}