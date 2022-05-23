using System;
using Marketplace.Framework;
using static Marketplace.Domain.Events;

namespace Marketplace.Domain
{
    public class ClassifiedAd : AggregateRoot<ClassifiedAdId>
    {
        public UserId OwnerId { get; private set; }
        public ClassifiedAdTitle Title { get; private set; }
        public ClassifiedAdText Text { get; private set; }
        public Price Price { get; private set; }
        public UserId ApprovedBy { get; private set; }
        public ClassifiedAdState State { get; private set; }

        public List<Picture> Pictures { get; private set; }

        public ClassifiedAd(ClassifiedAdId id, UserId ownerId)
        {
            Pictures = new List<Picture>();
            Apply(new Events.ClassifiedAdCreated
            {
                Id = id,
                OwnerId = ownerId
            });
        } 

        public void SetTitle(ClassifiedAdTitle title)
        {
            Apply(new Events.ClassifiedAdTitleChanged{
                Id = Id,
                Title = title
            });
        }
        public void UpdateText(ClassifiedAdText text)
        {
            Apply(new Events.ClassifiedAdTextUpdated{
                Id = Id,
                AdText = text
            });
        }
        public void UpdatePrice(Price price)
        {
            Apply(new Events.ClassifiedAdPriceUpdated
            {
                Id = Id,
                CurrencyCode = price.Currency.CurrencyCode,
                Price = price.Amount
            });
        }

        public void RequestToPublish()
        {
            Apply(new Events.ClassifiedAdSentForReview{
                Id = Id
            });
        }

        public void AddPicture(Uri pictureUri, PictureSize size) =>
            Apply(new Events.PictureAddedToAClassifiedAd
            {
                PictureId = new Guid(),
                ClassifiedAdId = Id,
                Url = pictureUri.ToString(),
                Height = size.Height,
                Width = size.Width
            });

        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.ClassifiedAdCreated e:
                    Id = new ClassifiedAdId(e.Id);
                    OwnerId = new UserId(e.OwnerId);
                    State = ClassifiedAdState.Inactive;
                    break;
                case Events.ClassifiedAdTitleChanged e:
                    Title = new ClassifiedAdTitle(e.Title);
                    break;
                case Events.ClassifiedAdTextUpdated e:
                    Text = new ClassifiedAdText(e.AdText);
                    break;
                case Events.ClassifiedAdPriceUpdated e:
                    Price = new Price(e.Price, e.CurrencyCode);
                    break;
                case Events.ClassifiedAdSentForReview e:
                    State = ClassifiedAdState.PendingReview;
                    break;
                case Events.PictureAddedToAClassifiedAd e:
                    var newPicture = new Picture
                    {
                        Size = new PictureSize(e.Width, e.Height),
                        Location = new Uri(e.Url),
                        Order = Pictures.Max(x => x.Order) + 1
                    };
                    Pictures.Add(newPicture);
                    break;
            }
        }

        protected override void EnsureValidState() 
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

        public enum ClassifiedAdState
        {
            PendingReview,
            Active,
            Inactive,
            MarkedAsSold
        }
    }
}