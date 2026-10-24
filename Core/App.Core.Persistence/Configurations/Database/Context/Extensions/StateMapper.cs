using App.Core.Domain.Configurations.Enums;

namespace App.Core.Persistence.Identity.Repositories;

internal static class StateMapper
{
    internal static Microsoft.EntityFrameworkCore.EntityState GetState(this EntityState state)
    {
        return state switch
        {
            EntityState.Added => Microsoft.EntityFrameworkCore.EntityState.Added,
            EntityState.Deleted => Microsoft.EntityFrameworkCore.EntityState.Deleted,
            EntityState.Modified => Microsoft.EntityFrameworkCore.EntityState.Modified,
            _ => Microsoft.EntityFrameworkCore.EntityState.Unchanged
        };
    }
}
