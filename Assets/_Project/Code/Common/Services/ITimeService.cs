using System;

namespace Code.Common.Services
{
    public interface ITimeService
    {
        public float DeltaTime { get; }

        public DateTime UtcNow { get; }

        public void StopTime();

        public void StartTime();
    }
}