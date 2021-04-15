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
	class Program
	{
		static void Main(string[] args) { new Program().Run(); }

		private const int WebsitePermanentOfferCount = 2;
		private const int WebsiteTemporaryOfferCount = 3;
		private Random rd = new Random(257);

		//----------
		//YOUR CODE - additional fileds/properties/methods
        class OfferWebsite
        {
            private List<IOffer> offers = new List<IOffer>();

            public void AddOffer(IOffer offer)
            {
                this.offers.Add(offer);
            }

            public void Present()
            {
                for (int i = 0; i < offers.Count; i++)
                {
                    offers[i].Print();
                    System.Console.WriteLine("");
                }
            }
        }
        //----------

        public void Run()
		{
			Console.OutputEncoding = System.Text.Encoding.UTF8;

			(
				BookingDatabase accomodationData, 
				TripAdvisorDatabase tripsData, 
				ShutterStockDatabase photosData, 
				OysterDatabase reviewData
			) = Init.Init.Run();

            //----------
            //YOUR CODE - set up everything
            ITravelAgency[] travelAgencies = {
                                                new PolandTravel(accomodationData, reviewData, photosData, tripsData, rd),
                                                new ItalyTravel(accomodationData, reviewData, photosData, tripsData, rd),
                                                new FranceTravel(accomodationData, reviewData, photosData, tripsData, rd)
                                             };

            IAdvertisingAgency[] advertisingAgencies = { new GraphicOffersAgency(1, 3), new TextOffersAgency(2, 2) };
            //----------

            while (true)
            {
				Console.Clear();

                //----------
                //YOUR CODE - run
                OfferWebsite offerWebsite = new OfferWebsite();

                for (int i = 1; i <= WebsitePermanentOfferCount; i++)
                {
                    offerWebsite.AddOffer(advertisingAgencies[rd.Next(0, 2)].CreatePermanentOffer(travelAgencies[rd.Next(0, 3)]));
                }

                for (int i = 1; i <= WebsiteTemporaryOfferCount; i++)
                {
                    offerWebsite.AddOffer(advertisingAgencies[rd.Next(0, 2)].CreateTemporaryOffer(travelAgencies[rd.Next(0, 3)]));
                }
                //----------

                //uncomment
                Console.WriteLine("\n\n=======================FIRST PRESENT======================");
                offerWebsite.Present();
                Console.WriteLine("\n\n=======================SECOND PRESENT======================");
                offerWebsite.Present();
                Console.WriteLine("\n\n=======================THIRD PRESENT======================");
                offerWebsite.Present();


                if (HandleInput()) break;
			}
		}
		bool HandleInput()
		{
			var key = Console.ReadKey(true);
			return key.Key == ConsoleKey.Escape || key.Key == ConsoleKey.Q;
		}
    }
}
