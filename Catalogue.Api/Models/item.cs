namespace Catalogue.Api.Models
{
    public record Item
    {
        public Guid Id { get; init; } // only allow creation but cant edit wit init

        public string? Name { get; init; }

        public decimal Price { get; init; }

        public DateTimeOffset CreatedDate
        {
            get; init;
        }
    }
}