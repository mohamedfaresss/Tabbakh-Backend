using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Domain;

namespace DataAcess.Configuration
{
    public class FavoriteRecipeConfiguration : IEntityTypeConfiguration<FavoriteRecipe>
    {
        public void Configure(EntityTypeBuilder<FavoriteRecipe> builder)
        {
            builder.HasKey(fr => new { fr.UserId, fr.RecipeId });

            builder.HasOne(fr => fr.User)
                   .WithMany(u => u.FavoriteRecipes)
                   .HasForeignKey(fr => fr.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(fr => fr.Recipe)
                   .WithMany(r => r.FavoritedBy)
                   .HasForeignKey(fr => fr.RecipeId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
