using Application.Features.Auth.Constants;
using Application.Features.OperationClaims.Constants;
using Application.Features.UserOperationClaims.Constants;
using Application.Features.Users.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NArchitecture.Core.Security.Constants;
using Application.Features.Hotels.Constants;
using Application.Features.RoomTypes.Constants;
using Application.Features.Inventories.Constants;
using Application.Features.Bookings.Constants;





namespace Persistence.EntityConfigurations;

public class OperationClaimConfiguration : IEntityTypeConfiguration<OperationClaim>
{
    public void Configure(EntityTypeBuilder<OperationClaim> builder)
    {
        builder.ToTable("OperationClaims").HasKey(oc => oc.Id);

        builder.Property(oc => oc.Id).HasColumnName("Id").IsRequired();
        builder.Property(oc => oc.Name).HasColumnName("Name").IsRequired();
        builder.Property(oc => oc.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(oc => oc.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(oc => oc.DeletedDate).HasColumnName("DeletedDate");

        builder.HasQueryFilter(oc => !oc.DeletedDate.HasValue);

        builder.HasData(_seeds);

        builder.HasBaseType((string)null!);
    }

    public static int AdminId => 1;
    private IEnumerable<OperationClaim> _seeds
    {
        get
        {
            yield return new() { Id = AdminId, Name = GeneralOperationClaims.Admin };

            IEnumerable<OperationClaim> featureOperationClaims = getFeatureOperationClaims(AdminId);
            foreach (OperationClaim claim in featureOperationClaims)
                yield return claim;
        }
    }

#pragma warning disable S1854 // Unused assignments should be removed
    private IEnumerable<OperationClaim> getFeatureOperationClaims(int initialId)
    {
        int lastId = initialId;
        List<OperationClaim> featureOperationClaims = new();

        #region Auth
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = AuthOperationClaims.Admin },
                new() { Id = ++lastId, Name = AuthOperationClaims.Read },
                new() { Id = ++lastId, Name = AuthOperationClaims.Write },
                new() { Id = ++lastId, Name = AuthOperationClaims.RevokeToken },
            ]
        );
        #endregion

        #region OperationClaims
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = OperationClaimsOperationClaims.Admin },
                new() { Id = ++lastId, Name = OperationClaimsOperationClaims.Read },
                new() { Id = ++lastId, Name = OperationClaimsOperationClaims.Write },
                new() { Id = ++lastId, Name = OperationClaimsOperationClaims.Create },
                new() { Id = ++lastId, Name = OperationClaimsOperationClaims.Update },
                new() { Id = ++lastId, Name = OperationClaimsOperationClaims.Delete },
            ]
        );
        #endregion

        #region UserOperationClaims
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = UserOperationClaimsOperationClaims.Admin },
                new() { Id = ++lastId, Name = UserOperationClaimsOperationClaims.Read },
                new() { Id = ++lastId, Name = UserOperationClaimsOperationClaims.Write },
                new() { Id = ++lastId, Name = UserOperationClaimsOperationClaims.Create },
                new() { Id = ++lastId, Name = UserOperationClaimsOperationClaims.Update },
                new() { Id = ++lastId, Name = UserOperationClaimsOperationClaims.Delete },
            ]
        );
        #endregion

        #region Users
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = UsersOperationClaims.Admin },
                new() { Id = ++lastId, Name = UsersOperationClaims.Read },
                new() { Id = ++lastId, Name = UsersOperationClaims.Write },
                new() { Id = ++lastId, Name = UsersOperationClaims.Create },
                new() { Id = ++lastId, Name = UsersOperationClaims.Update },
                new() { Id = ++lastId, Name = UsersOperationClaims.Delete },
            ]
        );
        #endregion

        
        #region Hotels CRUD
        
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = "Hotels.Admin" },
                new() { Id = ++lastId, Name = "Hotels.Read" },
                new() { Id = ++lastId, Name = "Hotels.Write" },
                
                new() { Id = ++lastId, Name = "Hotels.Create" },
                new() { Id = ++lastId, Name = "Hotels.Update" },
                new() { Id = ++lastId, Name = "Hotels.Delete" },
            ]
        );
        #endregion
        
        
        #region RoomTypes CRUD
        
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = "RoomTypes.Admin" },
                new() { Id = ++lastId, Name = "RoomTypes.Read" },
                new() { Id = ++lastId, Name = "RoomTypes.Write" },
                
                new() { Id = ++lastId, Name = "RoomTypes.Create" },
                new() { Id = ++lastId, Name = "RoomTypes.Update" },
                new() { Id = ++lastId, Name = "RoomTypes.Delete" },
            ]
        );
        #endregion
        
        
        #region Inventories CRUD
        
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = "Inventories.Admin" },
                new() { Id = ++lastId, Name = "Inventories.Read" },
                new() { Id = ++lastId, Name = "Inventories.Write" },
                
                new() { Id = ++lastId, Name = "Inventories.Create" },
                new() { Id = ++lastId, Name = "Inventories.Update" },
                new() { Id = ++lastId, Name = "Inventories.Delete" },
            ]
        );
        #endregion
        
        
        #region Bookings CRUD
        
        featureOperationClaims.AddRange(
            [
                new() { Id = ++lastId, Name = "Bookings.Admin" },
                new() { Id = ++lastId, Name = "Bookings.Read" },
                new() { Id = ++lastId, Name = "Bookings.Write" },
                
                new() { Id = ++lastId, Name = "Bookings.Create" },
                new() { Id = ++lastId, Name = "Bookings.Update" },
                new() { Id = ++lastId, Name = "Bookings.Delete" },
            ]
        );
        #endregion
        
        return featureOperationClaims;
    }
#pragma warning restore S1854 // Unused assignments should be removed
}
