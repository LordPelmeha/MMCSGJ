 #nullable enable
 using UnityEngine;
 using UnityEngine.UI;
 using TMPro;
 namespace HalfEmpty.Presentation.UI {
 /// <summary>
 /// Shows the current mark usage (e.g. "3/5 Marks").
 /// </summary>
 public class MarkCounterView : MonoBehaviour
 {
     [SerializeField] private TextMeshProUGUI? _countText;
     [SerializeField] private TextMeshProUGUI? _maxText;
     /// <summary>Set mark count and max.</summary>
     public void SetCount(int current, int max)
     {
         if (_countText != null) _countText.text = current.ToString();
         if (_maxText != null) _maxText.text = max.ToString();
     }
 }
 }