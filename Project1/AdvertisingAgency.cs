using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgencies.DataAccess;
using TravelAgencies.Agencies;

//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  <Wojciech> <Krawczyk>

namespace TravelAgencies
{
    //Interfaces
    interface IAdvertisingAgency
    {
        IOffer CreatePermanentOffer(ITravelAgency agency);
        IOffer CreateTemporaryOffer(ITravelAgency agency);
    }

    interface IOffer
    {
        void Print();
    }

    //Offers
    class GraphicPermanentOffer: IOffer
    {
        private List<IPhoto> photos = new List<IPhoto>();
        private ITrip trip;

        public GraphicPermanentOffer(ITravelAgency agency, int photosLimit)
        {
            this.trip = agency.CreateTrip();
            for (int i = 1; i <= photosLimit; i++)
            {
                photos.Add(agency.CreatePhoto());
            }
        }

        public void Print()
        {
            trip.Print();
            for (int i = 0; i < photos.Count; i++)
                photos[i].Print();
        }
    }

    class GraphicTemporaryOffer: IOffer
    {
        private List<IPhoto> photos = new List<IPhoto>();
        private ITrip trip;
        private int daysLimit;
        private int views = 0;

        public GraphicTemporaryOffer(ITravelAgency agency, int photosLimit, int daysLimit)
        {
            this.daysLimit = daysLimit;
            this.trip = agency.CreateTrip();
            for (int i = 1; i <= photosLimit; i++)
            {
                photos.Add(agency.CreatePhoto());
            }
        }

        public void Print()
        {
            this.views++;
            if (this.views <= this.daysLimit)
            {
                trip.Print();
                for (int i = 0; i < photos.Count; i++)
                    photos[i].Print();
            }
            else
                System.Console.WriteLine("This offer is expired");
        }
    }

    class TextPermanentOffer : IOffer
    {
        private List<IReview> reviews = new List<IReview>();
        private ITrip trip;

        public TextPermanentOffer(ITravelAgency agency, int reviewsLimit)
        {
            this.trip = agency.CreateTrip();
            for (int i = 1; i <= reviewsLimit; i++)
            {
                reviews.Add(agency.CreateReview());
            }
        }

        public void Print()
        {
            trip.Print();
            for (int i = 0; i < reviews.Count; i++)
                reviews[i].Print();
        }
    }

    class TextTemporaryOffer : IOffer
    {
        private List<IReview> reviews = new List<IReview>();
        private ITrip trip;
        private int daysLimit;
        private int views = 0;

        public TextTemporaryOffer(ITravelAgency agency, int reviewsLimit, int daysLimit)
        {
            this.daysLimit = daysLimit;
            this.trip = agency.CreateTrip();
            for (int i = 1; i <= reviewsLimit; i++)
            {
                reviews.Add(agency.CreateReview());
            }
        }

        public void Print()
        {
            this.views++;
            if (this.views <= this.daysLimit)
            {
                trip.Print();
                for (int i = 0; i < reviews.Count; i++)
                    reviews[i].Print();
            }
            else
                System.Console.WriteLine("This offer is expired");
        }
    }

    //Agencies
    class GraphicOffersAgency : IAdvertisingAgency
    {
        private int daysLimit;
        private int photosLimit;

        public GraphicOffersAgency(int daysLimit, int photosLimit)
        {
            this.daysLimit = daysLimit;
            this.photosLimit = photosLimit;
        }

        public IOffer CreatePermanentOffer(ITravelAgency agency)
        {
            return new GraphicPermanentOffer(agency, this.photosLimit);
        }

        public IOffer CreateTemporaryOffer(ITravelAgency agency)
        {
            return new GraphicTemporaryOffer(agency, this.photosLimit, this.daysLimit);
        }
    }

    class TextOffersAgency : IAdvertisingAgency
    {
        private int daysLimit;
        private int reviewsLimit;

        public TextOffersAgency(int daysLimit, int reviewsLimit)
        {
            this.daysLimit = daysLimit;
            this.reviewsLimit = reviewsLimit;
        }

        public IOffer CreatePermanentOffer(ITravelAgency agency)
        {
            return new TextPermanentOffer(agency, this.reviewsLimit);
        }

        public IOffer CreateTemporaryOffer(ITravelAgency agency)
        {
            return new TextTemporaryOffer(agency, this.reviewsLimit, this.daysLimit);
        }
    }
}
