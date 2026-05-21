#nullable enable
using HalfEmpty.Domain.Enums;
using UnityEngine;
namespace HalfEmpty.Presentation.Player {
/// <summary>
/// Triggers sprite-based animations based on the current form state.
/// </summary>
public class PlayerAnimationView
{
    private readonly Animator? _animator;
    private readonly SpriteRenderer? _headRenderer;
    private readonly SpriteRenderer? _bodyRenderer;
    /// <summary>
    /// Create with references to both form renderers.
    /// </summary>
    public PlayerAnimationView(Animator? animator, SpriteRenderer? headRenderer, SpriteRenderer? bodyRenderer)
    {
        _animator = animator;
        _headRenderer = headRenderer;
        _bodyRenderer = bodyRenderer;
    }
}
}