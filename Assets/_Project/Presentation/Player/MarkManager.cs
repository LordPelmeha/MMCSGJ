#nullable enable
using UnityEngine;
namespace HalfEmpty.Presentation.Player
{
/// <summary>
/// Tracks active marks, exposes current / maximum mark count, and applies MarkableView components.
/// </summary>
public class MarkManager : MonoBehaviour
{
    public int CurrentMarks { get; private set; }
    public int MaxMarks { get; private set; }
}
}
