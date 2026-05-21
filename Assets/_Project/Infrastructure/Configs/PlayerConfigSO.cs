#nullable enable
using UnityEngine;
namespace HalfEmpty.Infrastructure.Configs
{
/// <summary>
/// ScriptableObject configuration for the player's global parameters and both forms.
/// </summary>
[CreateAssetMenu(menuName = "Configs/Player Config", fileName = "NewPlayerConfig")]
public class PlayerConfigSO : ScriptableObject
{
    [Header("Form Switch")]
    [Tooltip("Cooldown between form switches in seconds.")]
    public float formSwitchCooldown = 0.5f;
    [Header("Parry")]
    [Tooltip("How long the parry hitbox stays active.")]
    public float parryWindow = 0.3f;
    [Tooltip("Cooldown between parries in seconds.")]
    public float parryCooldown = 0.5f;
    [Header("Forms")]
    [Tooltip("Configuration for Head form.")]
    public FormConfigSO? headFormConfig;
    [Tooltip("Configuration for Body form.")]
    public FormConfigSO? bodyFormConfig;
}
}