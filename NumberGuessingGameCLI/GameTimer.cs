using System.Diagnostics;

namespace NumberGuessingGameCLI
{
    class GameTimer
    {
        private Stopwatch stopwatch = new Stopwatch();
        public void Start() => stopwatch.Start();
        public void Stop() => stopwatch.Stop();
        public string ElapsedTime() => stopwatch.Elapsed.Seconds + " seconds";

    }
}
