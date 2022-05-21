using System;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class ClassifiedAd : Entity
    {
        public ClassifiedAdId Id { get; }

        public ClassifiedAd(ClassifiedAdId id, UserId ownerId)
        {
            Id = id;
            OwnerId = ownerId;
            State = ClassifiedAdState.Inactive;
            EnsureValidState();
            Raise(new Events.ClassifiedAdCreated
            {
                Id = id,
                OwnerId = ownerId
            });
        } 

        public void SetTitle(ClassifiedAdTitle title)
        {
            Title = title;
            EnsureValidState();
            Raise(new Events.ClassifiedAdTitleChanged{
                Id = Id,
                Title = title
            });
        }
        public void UpdateText(ClassifiedAdText text)
        {
            Text = text;
            EnsureValidState();
            Raise(new Events.ClassifiedAdTextUpdated{
                Id = Id,
                AdText = text
            });
        }
        public void UpdatePrice(Price price)
        {
            Price = price;
            EnsureValidState();
            Raise(new Events.ClassifiedAdPriceUpdated
            {
                Id = Id,
                CurrencyCode = price.Currency.CurrencyCode,
                Price = price.Amount
            });
        }

        public void RequestToPublish()
        {
            State = ClassifiedAdState.PendingReview;
            EnsureValidState();
            Raise(new Events.ClassifiedAdSentForReview{
                Id = Id
            });
        }

        protected void EnsureValidState() 
        {
            var valid = 
                Id != null &&
                OwnerId != null &&
                (State switch
                {
                    ClassifiedAdState.PendingReview =>
                        Title != null &&
                        Text != null &&
                        Price?.Amount > 0,
                    ClassifiedAdState.Active =>
                        Title != null &&
                        Text != null &&
                        Price?.Amount > 0 &&
                        ApprovedBy != null,
                    _ => true
                });
            
            if (!valid)
                throw new InvalidEntityStateException(
                    this, $"Post-checks failed in state {State}"
                );
        }

        public UserId OwnerId { get; }
        public ClassifiedAdTitle? Title { get; private set; }
        public ClassifiedAdText? Text { get; private set; }
        public Price? Price { get; private set; }
        public UserId? ApprovedBy { get; private set; }
        public ClassifiedAdState State { get; private set; }

        public enum ClassifiedAdState
        {
            PendingReview,
            Active,
            Inactive,
            MarkedAsSold
        }
    }
}