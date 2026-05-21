#nullable enable
using UnityEngine;
using UnityEngine.UI;
namespace HalfEmpty.Presentation.UI {
/// <summary>
/// Shows the current mark usage (e.g.\u00a0"3/5\u00a0Marks").
/// </summary>
public class MarkCounterView : MonoBehaviour
{
    [SerializeField] private Text? _countText;
    [SerializeField] private Text? _maxText;
    /// <summary>Set mark count and max.</summary>
    public void SetCount(int current, int max)
    {
        if (_countText != null) _countText.text = current.ToString();
        if (_maxText != null) _maxText.text = max.ToString();
    }
}
}