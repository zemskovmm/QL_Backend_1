using System.Collections.Generic;

namespace QuartierLatin.Backend.Application.ApplicationCore.Models
{
    public class CostHousingGroup
    {
        public static (int from, int? to) GetCostHousingGroup(int group) => group switch
        {
            1 => (300, 400),
            2 => (400, 500),
            3 => (500, 600),
            4 => (600, 700),
            5 => (700, 800),
            6 => (900, 1000),
            7 => (1000, null),
            _ => (0, 1000)
        };

        public static IReadOnlyList<int> CostHousingGroups { get; } = new[] {1, 2, 3, 4, 5, 6, 7};
    }
}