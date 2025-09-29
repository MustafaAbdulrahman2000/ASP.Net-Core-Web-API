namespace Module_02.Responses.V2;

public sealed class PriceResponse
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = null!;
}