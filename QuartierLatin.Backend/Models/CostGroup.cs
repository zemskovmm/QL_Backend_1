using System.Collections.Generic;

namespace QuartierLatin.Backend.Models
{
    public class CostGroup
    {
        public static (int from, int? to) GetCostGroup(int group) => group switch
        {
            1 => (0, 10000),
            2 => (10000, 20000),
            3 => (20000, 30000),
            4 => (30000, 40000),
            5 => (40000, 50000),
            6 => (50000, null),
            _ => (0, 100000)
        };

        public static IReadOnlyList<int> CostGroups { get; } = new[] {1, 2, 3, 4, 5, 6};
    }
}