using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tyche.TycheDAL.Constants;
using Tyche.TycheDAL.Models;

namespace Tyche.TycheDAL.Configuration
{
    internal class BlockedIPConfiguration : IEntityTypeConfiguration<BlockedIP>
    {
        public void Configure(EntityTypeBuilder<BlockedIP> builder)
        {
            builder.ToTable(Tables.BlockedIPs);

            builder.HasKey(blockedIp => blockedIp.Id);
        }
    }
}