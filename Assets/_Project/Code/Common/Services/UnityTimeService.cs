using System;
using UnityEngine;

namespace Code.Common.Services
{
    public class UnityTimeService : ITimeService
    {
        private bool _paused;

        public float DeltaTime => !_paused ? Time.deltaTime : 0;

        public DateTime UtcNow => DateTime.UtcNow;

        public void StopTime() => _paused = true;

        public void StartTime() => _paused = false;
    }
}