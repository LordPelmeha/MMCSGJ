#nullable enable
namespace HalfEmpty.Domain.Combat
{
/// <summary>
/// Possible result of a successful parry.
/// </summary>
public enum ParryResult
{
    ReflectedProjectile = 0,
    InstantKillMelee = 1,
    ReflectMelee = 2
}
}