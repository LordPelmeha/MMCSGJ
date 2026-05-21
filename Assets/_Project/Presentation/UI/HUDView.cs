#nullable enable
using HalfEmpty.Domain.Enums;
using UnityEngine;
using HalfEmpty.Presentation.Player;
namespace HalfEmpty.Presentation.UI {
/// <summary>
/// HUD overlay: displays Head HP, Body HP, current form icon, cooldowns, and mark count.
/// </summary>
public class HUDView : MonoBehaviour
{
    [Header("Sub-Views")]
    [SerializeField] private HealthBarView? _headHPBar;
    [SerializeField] private HealthBarView? _bodyHPBar;
    [SerializeField] private HealthBarView? _activeHPBar;
    [SerializeField] private FormIndicatorView? _formIndicator;
    [SerializeField] private CooldownIndicatorView? _dashCooldown;
    [SerializeField] private CooldownIndicatorView? _parryCooldown;
    [SerializeField] private MarkCounterView? _markCounter;
    [SerializeField] private PlayerController? _player;
    private void Update()
    {
        if (_player == null) return;
        var ctrl = _player.GetComponent<HalfEmpty.Presentation.Player.PlayerController>();
        // If we had direct access to controller, we'd query HP here.
        // HP bars are powered by PlayerHealthView events in the real setup.
    }
}
}