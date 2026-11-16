using BenchmarkDotNet.Attributes;

namespace ChestDecor.Benchmarks
{
    /// <summary>ChestDecor has no game-free hot path: its pure layer is the PaidMaterial refund record, built once per attachment removal. The mod's
    /// real per-frame cost (hover, camera patch) needs the running game and is measured with ModLog.Every timings in a TestLab run (PROJECT_RULES 30).</summary>
    [MemoryDiagnoser]
    public class ChestDecorBenchmarks
    {
        string _prefab;
        int _amount;

        [GlobalSetup]
        public void Setup()
        {
            _prefab = "Wood";
            _amount = 5;
        }

        [Benchmark]
        public int PaidMaterialBuild()
        {
            var m = new PaidMaterial(_prefab, _amount);
            return m.Amount + m.Prefab.Length;
        }
    }
}
