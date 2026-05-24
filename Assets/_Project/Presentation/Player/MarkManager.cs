 #nullable enable
 using UnityEngine;
 using System;
 namespace HalfEmpty.Presentation.Player
 {
 /// <summary>
 /// Tracks active marks, exposes current / maximum mark count, and applies MarkableView components.
 /// </summary>
 public class MarkManager : MonoBehaviour
 {
     public int CurrentMarks { get; private set; }
     public int MaxMarks { get; private set; }
     /// <summary>Fires whenever current mark count changes.</summary>
     public event Action<int, int>? OnMarkCountChanged;
     /// <summary>
     /// Initialise with a max mark limit (from FormConfigSO).
     /// </summary>
     public void Init(int maxMarks)
     {
         MaxMarks = maxMarks;
         CurrentMarks = 0;
     }
     /// <summary>Register a new mark. Fires OnMarkCountChanged.</summary>
     public void RegisterMark()
     {
         CurrentMarks = Mathf.Min(CurrentMarks + 1, MaxMarks);
         OnMarkCountChanged?.Invoke(CurrentMarks, MaxMarks);
     }
     /// <summary>Unregister / expire a mark. Fires OnMarkCountChanged.</summary>
     public void UnregisterMark()
     {
         CurrentMarks = Mathf.Max(0, CurrentMarks - 1);
         OnMarkCountChanged?.Invoke(CurrentMarks, MaxMarks);
     }
     /// <summary>True if all mark slots are in use.</summary>
     public bool IsFull => CurrentMarks >= MaxMarks;
 }
 }
