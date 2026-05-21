#nullable enable

using System;
using UnityEngine;

namespace HalfEmpty.Application.Enemies
{
    /// <summary>
    /// Detection component for enemies. Uses a distance check and optional angle check
    /// to determine whether the player is within sensing range.
    /// </summary>
    public class DetectionLogic
    {
        private readonly Transform _self;
        private readonly float _range;
        private readonly float _angle;
        private readonly LayerMask _targetMask;
        /// <summary>Raised when the player enters detection range.</summary>
        public event Action<Transform>? OnPlayerDetected;
        /// <summary>Initialises the detection component.</summary>
        /// <param name="self">The enemy's transform.</param>
        /// <param name="range">Detection distance in world units.</param>
        /// <param name="angle">Half-angle of the detection cone in degrees. Use 180 for full circle.</param>
        /// <param name="targetMask">Which layers count as the player.</param>
        public DetectionLogic(Transform self, float range, float angle, LayerMask targetMask)
        {
            _self = self;
            _range = range;
            _angle = angle;
            _targetMask = targetMask;
        }
    }
}
