using Microsoft.EntityFrameworkCore;
using Module_05.Data;
using Module_05.Requests;
using Module_05.Responses;
using Module_05.Models;
using Microsoft.AspNetCore.DataProtection;

namespace Module_05.Services;

public class BiddingService(AppDbContext context, IDataProtectionProvider protectionProvider): IBiddingService
{
    private readonly IDataProtector _dataProtector = protectionProvider.CreateProtector("Bidding.Protection");
    public async Task<BidResponse> CreateBidAsync(CreateBidRequest request)
    {
        var bid = new Bid
        {
            Id = Guid.NewGuid(),
            Amount = request.Amount,
            BidDate = DateTime.UtcNow,
            FirstName = _dataProtector.Protect(request.FirstName!),
            LastName = _dataProtector.Protect(request.LastName!),
            Email = _dataProtector.Protect(request.Email!),
            Address = _dataProtector.Protect(request.Address!),
            Telephone = _dataProtector.Protect(request.Telephone!)
        };

        context.Bids!.Add(bid);

        await context.SaveChangesAsync();

        return BidResponse.FromModel(bid, _dataProtector);
    }
    public async Task<BidResponse?> GetBidAsync(Guid id)
    {
        var bid = await context.Bids!.FirstOrDefaultAsync(b => b.Id == id);

        if (bid is null) 
            return null;

        return BidResponse.FromModel(bid, _dataProtector);
    }
    public async Task<List<BidResponse>> GetAllBidsAsync()
    {
        var bids = await context.Bids!
                                .OrderByDescending(b => b.BidDate)
                                .ToListAsync();

        return bids.Select(b => BidResponse.FromModel(b, _dataProtector)).ToList();
    }
}
