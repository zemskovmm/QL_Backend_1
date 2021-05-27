using System.Collections.Generic;

namespace QuartierLatin.Backend.Models
{
    public class CostGroup
    {
        public static (int from, int to) GetCostGroup(int group) => group switch
        {
            1 => (0, 10000),
            2 => (11000, 20000),
            3 => (21000, 30000),
            4 => (31000, 40000),
            5 => (41000, 50000),
            6 => (51000, 60000),
            _ => (0, 100000)
        };

        public static IReadOnlyList<int> CostGroups { get; } = new[] {1, 2, 3, 4, 5, 6};
    }
}