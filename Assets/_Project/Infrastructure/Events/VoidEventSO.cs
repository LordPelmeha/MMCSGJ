#nullable enable
using UnityEngine;
using System;
using System.Collections.Generic;
namespace HalfEmpty.Infrastructure.Events {
/// <summary>
/// Typed-global event that carries no payload. Raise it via script or Inspector.
/// </summary>
[CreateAssetMenu(menuName = "Events/Void Event", fileName = "NewVoidEvent")]
public class VoidEventSO : ScriptableObject
{
    private readonly List<Action> _listeners = new();
    /// <summary>Register a listener.</summary>
    public void Register(Action listener) => _listeners.Add(listener);
    /// <summary>Unregister a listener.</summary>
    public void Unregister(Action listener)
    {
        if (_listeners.Contains(listener)) _listeners.Remove(listener);
    }
    /// <summary>Raise the event to all registered listeners.</summary>
    public void Raise()
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i]?.Invoke();
        }
    }
}
}