namespace Domain
{
    public class RefreshTokenDpo
    {
        public string UserId { get; set; } = default!;
        public string Token { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = default!;
        public DateTime ExpiresAt { get; set; } = default!;
    }
}
