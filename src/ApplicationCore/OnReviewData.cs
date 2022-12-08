using System.Collections.Generic;
using ApplicationCore.Entities.Values;

namespace ApplicationCore
{
    public class OnReviewData
    {
        public Dictionary<int, List<ClientPackageInfo>> ReviewDictionary { get; set; } = new();
    }
}