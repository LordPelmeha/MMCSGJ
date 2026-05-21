#nullable enable
using UnityEngine;
namespace HalfEmpty.Presentation.Enemies {
/// <summary>
/// MonoBehaviour component that marks a GameObject as observable through Magma Trail.
/// </summary>
public class MarkableView : MonoBehaviour
{
    private bool _isMarked;
    /// <summary>True when the object has an active mark.</summary>
    public bool IsMarked => _isMarked;
    /// <summary>
    /// Called by the marking system to register a mark on this object.
    /// </summary>
    public void ApplyMark()
    {
        _isMarked = true;
    }
}
}