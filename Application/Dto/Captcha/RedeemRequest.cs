namespace Application
{
    public record RedeemRequest(string token, List<long> solutions);
}
