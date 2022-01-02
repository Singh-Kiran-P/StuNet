using Server.Api.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Server.Api.Config
{
    [ExcludeFromCodeCoverage]
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole {
                    Name = RolesEnum.prof,
                    NormalizedName = RolesEnum.prof_NORM,
                    Id = "0eb56564-4c92-4259-ab6f-6a9912c5c0c3"
                },
                new IdentityRole {
                    Name = RolesEnum.student,
                    NormalizedName = RolesEnum.student_NORM,
                    Id = "36c604a2-1f4e-4552-8741-74140540679b"
                }
            );
        }
    }
}
