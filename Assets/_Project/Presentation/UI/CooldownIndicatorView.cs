#nullable enable
using UnityEngine;
using UnityEngine.UI;
namespace HalfEmpty.Presentation.UI {
/// <summary>
/// Displays a single ability cooldown as a circular or bar fill.
/// </summary>
public class CooldownIndicatorView : MonoBehaviour
{
    [SerializeField] private Image? _fillImage;
    [SerializeField] private string _abilityName = "";
    /// <summary>
    /// Set the normalised fill amount (0 = ready, 1 = on cooldown).
    /// </summary>
    public void SetFill(float normalised)
    {
        if (_fillImage != null)
            _fillImage.fillAmount = Mathf.Clamp01(normalised);
    }
}
}