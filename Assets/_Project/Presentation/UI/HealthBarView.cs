#nullable enable
using UnityEngine;
using UnityEngine.UI;
namespace HalfEmpty.Presentation.UI {
/// <summary>
/// A horizontal health bar backed by an Image fill.
/// </summary>
public class HealthBarView : MonoBehaviour
{
    [SerializeField] private Image? _fillImage;
    /// <summary>Set the health bar value as current / max HP.</summary>
    public void SetValue(float currentHP, float maxHP)
    {
        if (_fillImage == null || maxHP <= 0f) return;
        _fillImage.fillAmount = Mathf.Clamp01(currentHP / maxHP);
    }
}
}