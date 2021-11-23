using System.Collections.Generic;

namespace QuartierLatin.Backend.Application.ApplicationCore.Models
{
    public class CostHousingGroup
    {
        public static (int from, int? to) GetCostHousingGroup(int group) => group switch
        {
            1 => (0, 100),
            2 => (100, 200),
            3 => (200, 300),
            4 => (300, 400),
            5 => (400, 500),
            6 => (500, null),
            _ => (0, 1000)
        };

        public static IReadOnlyList<int> CostHousingGroups { get; } = new[] {1, 2, 3, 4, 5, 6};
    }
}