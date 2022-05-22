using System;

namespace Marketplace.Contracts
{
    public static class ClassifiedAds
    {
        public static class V1
        {
            public record Create
            {
                public Guid Id { get; set; }
                public Guid OwnerId { get; set; }
            }
        }
    }
}