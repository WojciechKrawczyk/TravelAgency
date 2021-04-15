using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using TravelAgencies.DataAccess;

//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  <Wojciech> <Krawczyk>

namespace TravelAgencies.Agencies
{
    //Interfaces implementation
    public interface ITrip
    {
        int Days { get; }
        List<TripAdvice> Advice { get; }
        List<ListNode> Rooms { get; }

        int Price { get; }
        double Rating { get; }
        void Print();
    }

    public interface IPhoto
    {
        void Print();
    }

    public interface IReview
    {
        void Print();
    }

    //PolandTrip implementation
    public class PolandTrip: ITrip
    {
        private int days;
        private List<TripAdvice> advice = new List<TripAdvice>();
        private List<ListNode> rooms = new List<ListNode>();
        private int price = 0;
        private double ratingSum = 0.0;
        private BookingDecodec bookingDecodec = new BookingDecodec();
        private TripAdvisorDecodec tripAdvisorDecodec = new TripAdvisorDecodec();

        public PolandTrip(BookingIterator booking, TripAdvisorIterator advisor, Random random)
        {
            days = random.Next(1, 5);
            EncryptedNumber num;
            while (advice.Count < 3 * days)
            {
                while (true)
                {
                    if (advisor.HasNext)
                    {
                        TripAdvice a = advisor.GetNext;
                        if (a.Country == "Poland")
                        {
                            advice.Add(a);
                            num = new EncryptedNumber(a.Price);
                            this.price += tripAdvisorDecodec.GetRealNumber(num);
                            num = new EncryptedNumber(a.Rating);
                            this.ratingSum += tripAdvisorDecodec.GetRealNumber(num);
                            break;
                        }
                    }
                    else
                        advisor.Reset();
                }
            }

            while (rooms.Count < days)
            {
                while (true)
                {
                    if (booking.HasNext)
                    {
                        ListNode r = booking.GetNext;
                        rooms.Add(r);
                        num = new EncryptedNumber(r.Price);
                        this.price += bookingDecodec.GetRealNumber(num);
                        num = new EncryptedNumber(r.Rating);
                        this.ratingSum += bookingDecodec.GetRealNumber(num);
                        break;
                    }
                    else
                        booking.Reset();
                }
            }
        }

        public void Print()
        {
            System.Console.WriteLine($"Rating: {Rating.ToString("N4")}");
            System.Console.WriteLine($"Price: {Price}");
            System.Console.WriteLine("");

            int adviceCounter = 0;
            int roomsCounter = 0;
            for (int i = 1; i <= Days; i++)
            {
                System.Console.WriteLine($"Day {i} in Poland!");
                System.Console.WriteLine($"Accomodation: {rooms[roomsCounter].Name}");
                roomsCounter++;
                System.Console.WriteLine("Attractions:");
                for (int j = 1; j <= 3; j++)
                {
                    System.Console.WriteLine($"     {advice[adviceCounter].Name}");
                    adviceCounter++;
                }
                System.Console.WriteLine("");
            }
        }

        public int Days { get { return days; } }
        public List<TripAdvice> Advice { get{ return advice; } }
        public List<ListNode> Rooms { get { return rooms; } }
        public int Price { get { return price; } }
        public double Rating { get { return ratingSum / (days * 4); } }
    }

    public class PolandPhoto: IPhoto
    {
        private PhotMetadata photo = null;
        private ShutterStockDecodec shutterStockDecodec = new ShutterStockDecodec();

        public PolandPhoto(ShutterStockIterator shutterStock)
        {
            while (this.photo == null)
            {
                if (shutterStock.HasNext)
                {
                    PhotMetadata p = shutterStock.GetNext;
                    if (p.Longitude >= 14.4 && p.Longitude <= 23.5 && p.Latitude >= 49.8 && p.Latitude <= 54.2)
                    {
                        this.photo = p;
                    }
                }
                else
                    shutterStock.Reset();
            }
        }

        public void Print()
        {
            string n = photo.Name;
            n = n.Replace('s', 'ś');
            n = n.Replace('c', 'ć');
            EncryptedNumber w = new EncryptedNumber(photo.WidthPx);
            EncryptedNumber h = new EncryptedNumber(photo.HeightPx);
            int ww = shutterStockDecodec.GetRealNumber(w);
            int hh = shutterStockDecodec.GetRealNumber(h);
            System.Console.WriteLine($"{n} ({ww}x{hh})");
        }
    }
    
    public class PolandReview: IReview
    {
        string user = null;
        string coment = null;

        public PolandReview(OysterIterator oyster)
        {
            while (this.user == null)
            {
                if (oyster.HasNext)
                {
                    BSTNode node = oyster.GetNext;
                    this.user = node.UserName;
                    this.coment = node.Review;
                }
                else
                    oyster.Reset();
            }
        }

        public void Print()
        {
            string u = user;
            u = u.Replace('e', 'ę');
            u = u.Replace('a', 'ą');
            string c = coment;
            c = c.Replace('e', 'ę');
            c = c.Replace('a', 'ą');

            System.Console.WriteLine($"{u}: {c}");
        }
    }

    //ItalyTravel implementation
    public class ItalyTrip: ITrip
    {
        private int days;
        private List<TripAdvice> advice = new List<TripAdvice>();
        private List<ListNode> rooms = new List<ListNode>();
        private int price = 0;
        private double ratingSum = 0.0;
        private BookingDecodec bookingDecodec = new BookingDecodec();
        private TripAdvisorDecodec tripAdvisorDecodec = new TripAdvisorDecodec();

        public ItalyTrip(BookingIterator booking, TripAdvisorIterator advisor, Random random)
        {
            days = random.Next(1, 5);
            EncryptedNumber num;
            while (advice.Count < 3 * days)
            {
                while (true)
                {
                    if (advisor.HasNext)
                    {
                        TripAdvice a = advisor.GetNext;
                        if (a.Country == "Italy")
                        {
                            advice.Add(a);
                            num = new EncryptedNumber(a.Price);
                            this.price += tripAdvisorDecodec.GetRealNumber(num);
                            num = new EncryptedNumber(a.Rating);
                            this.ratingSum += tripAdvisorDecodec.GetRealNumber(num);
                            break;
                        }
                    }
                    else
                        advisor.Reset();
                }
            }

            while (rooms.Count < days)
            {
                while (true)
                {
                    if (booking.HasNext)
                    {
                        ListNode r = booking.GetNext;
                        rooms.Add(r);
                        num = new EncryptedNumber(r.Price);
                        this.price += bookingDecodec.GetRealNumber(num);
                        num = new EncryptedNumber(r.Rating);
                        this.ratingSum += bookingDecodec.GetRealNumber(num);
                        break;
                    }
                    else
                        booking.Reset();
                }
            }
        }

        public void Print()
        {
            System.Console.WriteLine($"Rating: {Rating.ToString("N4")}");
            System.Console.WriteLine($"Price: {Price}");
            System.Console.WriteLine("");

            int adviceCounter = 0;
            int roomsCounter = 0;
            for (int i = 1; i <= Days; i++)
            {
                System.Console.WriteLine($"Day {i} in Italy!");
                System.Console.WriteLine($"Accomodation: {rooms[roomsCounter].Name}");
                roomsCounter++;
                System.Console.WriteLine("Attractions:");
                for (int j = 1; j <= 3; j++)
                {
                    System.Console.WriteLine($"     {advice[adviceCounter].Name}");
                    adviceCounter++;
                }
                System.Console.WriteLine("");
            }
        }

        public int Days { get { return days; } }
        public List<TripAdvice> Advice { get { return advice; } }
        public List<ListNode> Rooms { get { return rooms; } }
        public int Price { get { return price; } }
        public double Rating { get { return ratingSum / (days * 4); } }
    }

    public class ItalyPhoto: IPhoto
    {
        private PhotMetadata photo = null;
        private ShutterStockDecodec shutterStockDecodec = new ShutterStockDecodec();

        public ItalyPhoto(ShutterStockIterator shutterStock)
        {
            while (this.photo == null)
            {
                if (shutterStock.HasNext)
                {
                    PhotMetadata p = shutterStock.GetNext;
                    if (p != null) 
                    if (p.Longitude >= 8.8 && p.Longitude <= 15.2 && p.Latitude >= 37.7 && p.Latitude <= 44.0)
                    {
                        this.photo = p;
                    }
                }
                else
                    shutterStock.Reset();
            }
        }

        public void Print()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Dello_");
            sb.Append(photo.Name);
            EncryptedNumber w = new EncryptedNumber(photo.WidthPx);
            EncryptedNumber h = new EncryptedNumber(photo.HeightPx);
            int ww = shutterStockDecodec.GetRealNumber(w);
            int hh = shutterStockDecodec.GetRealNumber(h);
            System.Console.WriteLine($"{sb.ToString()} ({ww}x{hh})");
        }
    }

    public class ItalyReview: IReview
    {
        string user = null;
        string coment = null;

        public ItalyReview(OysterIterator oyster)
        {
            while (this.user == null)
            {
                if (oyster.HasNext)
                {
                    BSTNode node = oyster.GetNext;
                    this.user = node.UserName;
                    this.coment = node.Review;
                }
                else
                    oyster.Reset();
            }
        }

        public void Print()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Della_");
            sb.Append(user);

            System.Console.WriteLine($"{sb.ToString()}: {coment}");
        }
    }

    //FranceTravel implementation
    public class FranceTrip : ITrip
    {
        private int days;
        private List<TripAdvice> advice = new List<TripAdvice>();
        private List<ListNode> rooms = new List<ListNode>();
        private int price = 0;
        private double ratingSum = 0.0;
        private BookingDecodec bookingDecodec = new BookingDecodec();
        private TripAdvisorDecodec tripAdvisorDecodec = new TripAdvisorDecodec();

        public FranceTrip(BookingIterator booking, TripAdvisorIterator advisor, Random random)
        {
            days = random.Next(1, 5);
            EncryptedNumber num;
            while (advice.Count < 3 * days)
            {
                while (true)
                {
                    if (advisor.HasNext)
                    {
                        TripAdvice a = advisor.GetNext;
                        if (a.Country == "France")
                        {
                            advice.Add(a);
                            num = new EncryptedNumber(a.Price);
                            this.price += tripAdvisorDecodec.GetRealNumber(num);
                            num = new EncryptedNumber(a.Rating);
                            this.ratingSum += tripAdvisorDecodec.GetRealNumber(num);
                            break;
                        }
                    }
                    else
                        advisor.Reset();
                }
            }

            while (rooms.Count < days)
            {
                while (true)
                {
                    if (booking.HasNext)
                    {
                        ListNode r = booking.GetNext;
                        rooms.Add(r);
                        num = new EncryptedNumber(r.Price);
                        this.price += bookingDecodec.GetRealNumber(num);
                        num = new EncryptedNumber(r.Rating);
                        this.ratingSum += bookingDecodec.GetRealNumber(num);
                        break;
                    }
                    else
                        booking.Reset();
                }
            }
        }

        public void Print()
        {
            System.Console.WriteLine($"Rating: {Rating.ToString("N4")}");
            System.Console.WriteLine($"Price: {Price}");
            System.Console.WriteLine("");

            int adviceCounter = 0;
            int roomsCounter = 0;
            for (int i = 1; i <= Days; i++)
            {
                System.Console.WriteLine($"Day {i} in France!");
                System.Console.WriteLine($"Accomodation: {rooms[roomsCounter].Name}");
                roomsCounter++;
                System.Console.WriteLine("Attractions:");
                for (int j = 1; j <= 3; j++)
                {
                    System.Console.WriteLine($"     {advice[adviceCounter].Name}");
                    adviceCounter++;
                }
                System.Console.WriteLine("");
            }
        }

        public int Days { get { return days; } }
        public List<TripAdvice> Advice { get { return advice; } }
        public List<ListNode> Rooms { get { return rooms; } }
        public int Price { get { return price; } }
        public double Rating { get { return ratingSum / (days * 4); } }
    }

    public class FrancePhoto : IPhoto
    {
        private PhotMetadata photo = null;
        private ShutterStockDecodec shutterStockDecodec = new ShutterStockDecodec();

        public FrancePhoto(ShutterStockIterator shutterStock)
        {
            while (this.photo == null)
            {
                if (shutterStock.HasNext)
                {
                    PhotMetadata p = shutterStock.GetNext;
                    if (p.Longitude >= 0.0 && p.Longitude <= 5.4 && p.Latitude >= 43.6 && p.Latitude <= 50.0)
                    {
                        this.photo = p;
                    }
                }
                else
                    shutterStock.Reset();
            }
        }

        public void Print()
        {
            EncryptedNumber w = new EncryptedNumber(photo.WidthPx);
            EncryptedNumber h = new EncryptedNumber(photo.HeightPx);
            int ww = shutterStockDecodec.GetRealNumber(w);
            int hh = shutterStockDecodec.GetRealNumber(h);
            System.Console.WriteLine($"{photo.Name} ({ww}x{hh})");
        }
    }

    public class FranceReview : IReview
    {
        string user = null;
        string coment = null;

        public FranceReview(OysterIterator oyster)
        {
            while (this.user == null)
            {
                if (oyster.HasNext)
                {
                    BSTNode node = oyster.GetNext;
                    this.user = node.UserName;
                    this.coment = node.Review;
                }
                else
                    oyster.Reset();
            }
        }

        public void Print()
        {
            string s = coment;
            string[] tab = s.Split(new char[] { ' ' });
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < tab.Length; i++)
            {
                if (tab[i].Length < 4)
                    sb.Append("la ");
                else
                    sb.Append(tab[i] + " ");
            }
            if (sb[0] == ' ')
                sb.Remove(0, 1);
            if (sb[sb.Length - 1] == ' ')
                sb.Remove(sb.Length - 1, 1);

            System.Console.WriteLine($"{user}: {sb.ToString()}");
        }
    }

    //Abstract Factory implementation
    public interface ITravelAgency
	{
        ITrip CreateTrip();
        IPhoto CreatePhoto();
        IReview CreateReview();
    }

    public class PolandTravel: ITravelAgency
    {
        private BookingIterator booking;
        private OysterIterator oyster;
        private ShutterStockIterator shutter;
        private TripAdvisorIterator advisor;
        private Random random;

        public PolandTravel(BookingDatabase bookingDatabase, OysterDatabase oysterDatabase, ShutterStockDatabase shutterDatabase, TripAdvisorDatabase advisorDatabase, Random random)
        {
            this.booking = new BookingIterator(bookingDatabase);
            this.oyster = new OysterIterator(oysterDatabase);
            this.shutter = new ShutterStockIterator(shutterDatabase);
            this.advisor = new TripAdvisorIterator(advisorDatabase);
            this.random = random;
        }

        public ITrip CreateTrip()
        {
            return new PolandTrip(this.booking, this.advisor, this.random);
        }

        public IPhoto CreatePhoto()
        {
            return new PolandPhoto(this.shutter);
        }

        public IReview CreateReview()
        {
            return new PolandReview(this.oyster);
        }
    }

    public class ItalyTravel: ITravelAgency
    {
        private BookingIterator booking;
        private OysterIterator oyster;
        private ShutterStockIterator shutter;
        private TripAdvisorIterator advisor;
        private Random random;

        public ItalyTravel(BookingDatabase bookingDatabase, OysterDatabase oysterDatabase, ShutterStockDatabase shutterDatabase, TripAdvisorDatabase advisorDatabase, Random random)
        {
            this.booking = new BookingIterator(bookingDatabase);
            this.oyster = new OysterIterator(oysterDatabase);
            this.shutter = new ShutterStockIterator(shutterDatabase);
            this.advisor = new TripAdvisorIterator(advisorDatabase);
            this.random = random;
        }

        public ITrip CreateTrip()
        {
            return new ItalyTrip(this.booking, this.advisor, this.random);
        }

        public IPhoto CreatePhoto()
        {
            return new ItalyPhoto(this.shutter);
        }

        public IReview CreateReview()
        {
            return new ItalyReview(this.oyster);
        }
    }

    public class FranceTravel : ITravelAgency
    {
        private BookingIterator booking;
        private OysterIterator oyster;
        private ShutterStockIterator shutter;
        private TripAdvisorIterator advisor;
        private Random random;

        public FranceTravel(BookingDatabase bookingDatabase, OysterDatabase oysterDatabase, ShutterStockDatabase shutterDatabase, TripAdvisorDatabase advisorDatabase, Random random)
        {
            this.booking = new BookingIterator(bookingDatabase);
            this.oyster = new OysterIterator(oysterDatabase);
            this.shutter = new ShutterStockIterator(shutterDatabase);
            this.advisor = new TripAdvisorIterator(advisorDatabase);
            this.random = random;
        }

        public ITrip CreateTrip()
        {
            return new FranceTrip(this.booking, this.advisor, this.random);
        }

        public IPhoto CreatePhoto()
        {
            return new FrancePhoto(this.shutter);
        }

        public IReview CreateReview()
        {
            return new FranceReview(this.oyster);
        }
    }
}