#nullable enable
using System;
using HalfEmpty.Domain.Enums;
using UnityEngine;
using HalfEmpty.Presentation;
using HalfEmpty.Application.Enemies.States;
using HalfEmpty.Application.FSM;
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
        /// <summary>
        /// Call every update. Checks whether the player is within range and cone.
        /// </summary>
        /// <param name="playerTransform">The player's transform (found externally).</param>
        public void UpdateDetection(Transform playerTransform)
        {
            if (playerTransform == null) return;
            float distance = Vector2.Distance(_self.position, playerTransform.position);
            if (distance > _range) return;
            // Direction to player
            Vector2 dirToPlayer = (playerTransform.position - _self.position).normalized;
            // Cone check — use enemy's forward direction (right in 2D)
            float dot = Vector2.Dot(dirToPlayer, (Vector2)_self.right);
            float halfAngleCos = Mathf.Cos(_angle * 0.5f * Mathf.Deg2Rad);
            if (dot >= halfAngleCos)
            {
                OnPlayerDetected?.Invoke(playerTransform);
            }
        }
    }
}
