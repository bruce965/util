// Copyright (c) 2021 Fabio Iotti
// The copyright holders license this file to you under the MIT license,
// available at https://github.com/bruce965/util/raw/master/LICENSE

using UnityEngine;

namespace Utility
{
    public static class ValueDamp
    {
        /// <summary>
        /// Time-independent damping with exponential-decay.
        /// </summary>
        /// <param name="current">Current value.</param>
        /// <param name="target">Target value.</param>
        /// <param name="smoothing">Proportion of <paramref name="current"/> remaining after one second.</param>
        /// <param name="deltaTime">Delta time.</param>
        /// <returns>Dampened value.</returns>
        public static float Damp(float current, float target, float smoothing, float deltaTime)
        {
            // https://web.archive.org/web/20210630140440/https://www.rorydriscoll.com/2016/03/07/frame-rate-independent-damping-using-lerp/
            return Mathf.Lerp(current, target, 1 - Mathf.Pow(smoothing, deltaTime));
        }
    }
}
