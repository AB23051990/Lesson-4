
namespace Models
{
    internal class CpuMetric : global::CpuMetric
    {
        public TimeSpan Time { get; set; }
        public int Value { get; set; }
    }
}