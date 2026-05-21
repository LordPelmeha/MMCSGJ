#nullable enable
namespace HalfEmpty.Domain.Enums
{
/// <summary>
/// States an enemy AI can be in.
/// </summary>
public enum EnemyState
{
    Idle = 0,
    Chase = 1,
    Attack = 2,
    Shoot = 3,
    Death = 4
}
}