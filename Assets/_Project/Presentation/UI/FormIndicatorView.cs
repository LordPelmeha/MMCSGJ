 #nullable enable
 using HalfEmpty.Domain.Enums;
 using UnityEngine;
 using UnityEngine.UI;
 using TMPro;
 namespace HalfEmpty.Presentation.UI
 {
 /// <summary>
 /// Displays the active form icon in the HUD corner.
 /// </summary>
 public class FormIndicatorView : MonoBehaviour
 {
     [Header("Form Icons")]
     [SerializeField] private Sprite? _headIcon;
     [SerializeField] private Sprite? _bodyIcon;
     [Header("UI")]
     [SerializeField] private Image? _iconImage;
     [SerializeField] private TextMeshProUGUI? _formLabel;
     /// <summary>Set the display to the given form.</summary>
     public void SetForm(FormType form)
     {
         if (_iconImage != null)
         {
             _iconImage.sprite = form == FormType.Head ? _headIcon : _bodyIcon;
         }
         if (_formLabel != null)
         {
             _formLabel.text = form == FormType.Head ? "HEAD" : "BODY";
         }
     }
 }
 }
