using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  <Wojciech> <Krawczyk>

namespace TravelAgencies.DataAccess
{
	public class PhotMetadata
	{
		public string Name { get; set; }
		public string Camera { get; set; }
		public double[] CameraSettings { get; set; }
		public DateTime Date { get; set; }
		public string WidthPx { get; set; }//Encrypted
		public string HeightPx { get; set; }//Encrypted
		public double Longitude { get; set; }
		public double Latitude { get; set; }
	}

	public class ShutterStockDatabase : IContainer<PhotMetadata>
	{
		public PhotMetadata[][][] Photos;

        public IIterator<PhotMetadata> GetIterator()
        {
            return new ShutterStockIterator(this);
        }

        public PhotMetadata this[int index]
        {
            get
            {
                if (index >= 0 && index < this.Count && Photos != null)
                {
                    int cur = -1;
                    for (int i = 0; i < Photos.Length; i++)
                    {
                        if (Photos[i] != null)
                        {
                            for (int j = 0; j < Photos[i].Length; j++)
                            {
                                if (Photos[i][j] != null)
                                {
                                    for (int k = 0; k < Photos[i][j].Length; k++)
                                    {
                                        if (Photos[i][j][k] != null)
                                        {
                                            cur++;
                                            if (cur == index)
                                                return Photos[i][j][k];
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return null;
            }
        }

        public int Count
        {
            get
            {
                int count = 0;
                if (Photos != null)
                {
                    for (int i = 0; i < Photos.Length; i++)
                    {
                        if (Photos[i] != null)
                        {
                            for (int j = 0; j < Photos[i].Length; j++)
                            {
                                if (Photos[i][j] != null)
                                {
                                    for (int k = 0; k < Photos[i][j].Length; k++)
                                    {
                                        if (Photos[i][j][k] != null)
                                            count++;
                                    }
                                }
                            }
                        }
                    }
                }
                return count;
            }
        }
    }

    public class ShutterStockIterator: IIterator<PhotMetadata>
    {
        ShutterStockDatabase database = null;
        int index = 0;

        public ShutterStockIterator(ShutterStockDatabase database)
        {
            this.database = database;
        }

        public void Reset()
        {
            this.index = 0;
        }

        public PhotMetadata GetNext
        {
            get
            {
                if (HasNext)
                {
                    PhotMetadata tmp = database[index];
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
