using System;
using Marketplace.Domain;
using Xunit;

namespace Marketplace.Tests
{
    public class ClassifiedAd_Publish_Spec
    {
        private readonly ClassifiedAd _classifiedAd;
        private readonly Guid _id = Guid.NewGuid();
        public ClassifiedAd_Publish_Spec()
        {
            _classifiedAd = new ClassifiedAd(
                new ClassifiedAdId(_id),
                new UserId(Guid.NewGuid())
            );
        }

        [Fact]
        public void Can_create_an_ad_instance()
        {
            Assert.Equal(_classifiedAd.Id, _id);
        }

        [Fact]
        public void Can_publish_a_valid_ad()
        {
            _classifiedAd.SetTitle(
                ClassifiedAdTitle.FromString("Test ad")
            );
            _classifiedAd.UpdateText(
                ClassifiedAdText.FromString("Buy my stuffs")
            );
            _classifiedAd.UpdatePrice(
                Price.FromDecimal(100.10m, "EUR", new FakeCurrencyLookup())
            );
            _classifiedAd.RequestToPublish();

            Assert.Equal(ClassifiedAd.ClassifiedAdState.PendingReview, _classifiedAd.State);
        }

        [Fact]
        public void Cannot_publish_without_title()
        {
            _classifiedAd.UpdateText(
                ClassifiedAdText.FromString("Buy my stuffs")
            );
            _classifiedAd.UpdatePrice(
                Price.FromDecimal(100.10m, "EUR", new FakeCurrencyLookup())
            );
            Assert.Throws<InvalidEntityStateException>(() => _classifiedAd.RequestToPublish());
        }

        [Fact]
        public void Cannot_publish_without_text()
        {
            _classifiedAd.SetTitle(
                ClassifiedAdTitle.FromString("Test ad")
            );
            _classifiedAd.UpdatePrice(
                Price.FromDecimal(100.10m, "EUR", new FakeCurrencyLookup())
            );
            Assert.Throws<InvalidEntityStateException>(() => _classifiedAd.RequestToPublish());
        }

        [Fact]
        public void Cannot_publish_without_price()
        {
            _classifiedAd.SetTitle(
                ClassifiedAdTitle.FromString("Test ad")
            );
            _classifiedAd.UpdateText(
                ClassifiedAdText.FromString("Buy my stuffs")
            );
            Assert.Throws<InvalidEntityStateException>(() => _classifiedAd.RequestToPublish());
        }

        [Fact]
        public void Cannot_publish_with_zero_price()
        {
            _classifiedAd.SetTitle(
                ClassifiedAdTitle.FromString("Test ad")
            );
            _classifiedAd.UpdateText(
                ClassifiedAdText.FromString("Buy my stuffs")
            );
            _classifiedAd.UpdatePrice(
                Price.FromDecimal(0.00m, "EUR", new FakeCurrencyLookup())
            );
            Assert.Throws<InvalidEntityStateException>(() => _classifiedAd.RequestToPublish());
        }
    }
}