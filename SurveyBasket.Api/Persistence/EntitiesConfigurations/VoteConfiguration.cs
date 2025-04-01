namespace SurveyBasket.Api.Persistence.EntitiesConfigurations;

public class VoteConfiguration : IEntityTypeConfiguration<Vote>
{
    public void Configure(EntityTypeBuilder<Vote> builder)
    {
        builder.HasIndex(x => new { x.Id, x.UserId }).IsUnique();

    }
}
