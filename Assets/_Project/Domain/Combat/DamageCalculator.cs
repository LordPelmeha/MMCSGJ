#nullable enable

using System;
using UnityEngine;

namespace HalfEmpty.Domain.Combat
{
    /// <summary>
    /// Static damage calculator.
    /// </summary>
    public static class DamageCalculator
    {
        /// <summary>
        /// Calculates final damage = baseDamage * multiplier.
        /// </summary>
        public static float Calculate(float baseDamage, float multiplier)
        {
            return baseDamage * multiplier;
        }
    }
}
