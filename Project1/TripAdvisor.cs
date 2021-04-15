using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  <Wojciech> <Krawczyk>

namespace TravelAgencies.DataAccess
{
    public class TripAdvice
    {
        public string Name { get; set; }
        public string Price { get; set; }//Encrypted
        public string Rating { get; set; }//Encrypted
        public string Country { get; set; }
    }

	public class TripAdvisorDatabase : IContainer<TripAdvice>
	{
		public Guid[] Ids;
		public Dictionary<Guid, string>[] Names { get; set; }
		public Dictionary<Guid, string> Prices { get; set; }//Encrypted
		public Dictionary<Guid, string> Ratings { get; set; }//Encrypted
		public Dictionary<Guid, string> Countries { get; set; }

        public IIterator<TripAdvice> GetIterator()
        {
            return new TripAdvisorIterator(this);
        }

        public TripAdvice this[int index]
        {
            get
            {
                if (index >= 0 && index < this.Count && Ids != null && Names != null && Prices != null && Ratings != null && Countries != null) 
                {
                    int cur = -1;
                    for (int i = 0; i < Ids.Length; i++)
                    {
                        bool t = false;
                        int k = 0;
                        for (int j = 0; j < Names.Length; j++)
                        {
                            if (Names[j].ContainsKey(Ids[i]))
                            {
                                k = j;
                                t = true;
                                break;
                            }
                        }
                        if (!t)
                            continue;
                        if (Prices.ContainsKey(Ids[i]) && Ratings.ContainsKey(Ids[i]) && Countries.ContainsKey(Ids[i]))
                        {
                            cur++;
                            if (cur == index)
                            {
                                TripAdvice tripAdvice = new TripAdvice();
                                string name;
                                string price;
                                string rating;
                                string country;
                                Names[k].TryGetValue(Ids[i], out name);
                                Prices.TryGetValue(Ids[i], out price);
                                Ratings.TryGetValue(Ids[i], out rating);
                                Countries.TryGetValue(Ids[i], out country);
                                tripAdvice.Name = name;
                                tripAdvice.Price = price;
                                tripAdvice.Rating = rating;
                                tripAdvice.Country = country;
                                return tripAdvice;
                            }
                        }
                    }
                    return null;
                }
                else
                    return null;
            }
        }

        public int Count
        {
            get
            {
                int count = 0;
                for (int i = 0; i < Ids.Length; i++)
                {
                    bool t = false;
                    for (int j = 0; j < Names.Length; j++)
                    {
                        if (Names[j].ContainsKey(Ids[i]))
                        {
                            t = true;
                            break;
                        }
                    }
                    if (!t)
                        continue;
                    if (Prices.ContainsKey(Ids[i]) && Ratings.ContainsKey(Ids[i]) && Countries.ContainsKey(Ids[i]))
                        count++;
                }
                return count;
            }
        }
    }

    public class TripAdvisorIterator: IIterator<TripAdvice>
    {
        TripAdvisorDatabase database = null;
        int index = 0;

        public TripAdvisorIterator(TripAdvisorDatabase database)
        {
            this.database = database;
        }

        public void Reset()
        {
            this.index = 0;
        }

        public TripAdvice GetNext
        {
            get
            {
                if (HasNext)
                {
                    TripAdvice tmp = database[index];
                    this.index++;
                    return tmp;
                }
                return null;
            }
        }

        public bool HasNext
        {
            get
            {
                return index < database.Count;
            }
        }
    }
}

