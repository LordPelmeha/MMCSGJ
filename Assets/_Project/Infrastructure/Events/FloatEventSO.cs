#nullable enable
using UnityEngine;
using System;
using System.Collections.Generic;
namespace HalfEmpty.Infrastructure.Events {
/// <summary>
/// Typed-global event that carries a float value (e.g. health changed).
/// </summary>
[CreateAssetMenu(menuName = "Events/Float Event", fileName = "NewFloatEvent")]
public class FloatEventSO : ScriptableObject
{
    private readonly List<Action<float>> _listeners = new();
    /// <summary>Register a listener.</summary>
    public void Register(Action<float> listener) => _listeners.Add(listener);
    /// <summary>Unregister a listener.</summary>
    public void Unregister(Action<float> listener)
    {
        if (_listeners.Contains(listener)) _listeners.Remove(listener);
    }
}
}