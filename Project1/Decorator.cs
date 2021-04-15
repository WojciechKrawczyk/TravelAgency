using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  <Wojciech> <Krawczyk>

namespace TravelAgencies.DataAccess
{
    interface EncryptedData
    {
        string GetData();
    }

    class EncryptedNumber : EncryptedData
    {
        string data;

        public EncryptedNumber(string number)
        {
            this.data = number;
        }

        public string GetData()
        {
            return this.data;
        }
    }

    abstract class EncryptedDataDecorator: EncryptedData
    {
        protected EncryptedData data;

        public EncryptedDataDecorator(EncryptedData data)
        {
            this.data = data;
        }

        public virtual string GetData()
        {
            return data.GetData();
        }
    }

    class FrameCodecDecorator: EncryptedDataDecorator
    {
        private int n;

        public FrameCodecDecorator(EncryptedData data, int n):base(data)
        {
            this.n = n;
        }

        public override string GetData()
        {
            if (n < 0 || n > 9)
                throw new ArgumentException();
            StringBuilder main = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            StringBuilder rsb = new StringBuilder();
            for (int i = 1; i <= this.n; i++)
            {
                sb.Append(i.ToString());
                rsb.Insert(0, i.ToString());
            }
            main.Append(sb).Append(this.data.GetData()).Append(rsb);
            return main.ToString();
        }
    }

    class FrameDecodecDecorator: EncryptedDataDecorator
    {
        private int n;

        public FrameDecodecDecorator(EncryptedData data, int n) : base(data)
        {
            this.n = n;
        }

        public override string GetData()
        {
            if (n < 0 || n > 9)
                throw new ArgumentException();

            char[] tab = base.GetData().ToCharArray();
            if (2 * this.n >= tab.Length)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            for (int i = n; i < tab.Length-n; i++)
            {
                sb.Append(tab[i]);
            }
            return sb.ToString();
        }
    }

    class ReverseCodecDecorator: EncryptedDataDecorator
    {
        public ReverseCodecDecorator(EncryptedData data) : base(data) { }

        public override string GetData()
        {
            char[] tab = base.GetData().ToCharArray();
            Array.Reverse(tab);
            return new string(tab);
        }
    }

    class PushCodecDecorator: EncryptedDataDecorator
    {
        private int n;

        public PushCodecDecorator(EncryptedData data, int n) : base(data)
        {
            this.n = n;
        }

        public override string GetData()
        {
            string s = base.GetData();
            if (this.n == 0 || s.Length < 1)   
                return s;

            char[] tab = s.ToCharArray();
            char outc;
            if (n > 0)
            {
                for (int i = 1; i <= n; i++) 
                {
                    char inc = tab[0];
                    for (int j = 1; j < s.Length; j++)
                    {
                        outc = tab[j];
                        tab[j] = inc;
                        inc = outc;
                    }
                    tab[0] = inc;
                }
                return new string(tab);
            }
            if (n < 0)
            {
                for (int i = 1; i <= Math.Abs(n); i++)
                {
                    char inc = tab[tab.Length - 1];
                    for (int j = tab.Length - 2; j >= 0; j--)
                    {
                        outc = tab[j];
                        tab[j] = inc;
                        inc = outc;
                    }
                    tab[tab.Length - 1] = inc;
                }
            }
            return new string(tab);
        }
    }

    class CezarCodecDecorator: EncryptedDataDecorator
    {
        private int n;

        public CezarCodecDecorator(EncryptedData data, int n) : base(data)
        {
            this.n = n;
        }

        public override string GetData()
        {
            int j = this.n % 10;
            char[] tab = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            char[] ret = base.GetData().ToCharArray();
            for (int i = 0; i < ret.Length; i++)
            {
                int k = (ret[i] - 48 + j) % 10;
                if (k < 0)
                    k = k + 10;
                ret[i] = tab[k];
            }
            return new string(ret);
        }
    }

    class SwapCodecDecorator: EncryptedDataDecorator
    {
        public SwapCodecDecorator(EncryptedData data) : base(data) { }

        public override string GetData()
        {
            char[] tab = base.GetData().ToCharArray();
            int l = tab.Length % 2;
            for (int i = 0; i <= tab.Length - 2 - l; i += 2)
            {
                char p = tab[i];
                tab[i] = tab[i + 1];
                tab[i + 1] = p;
            }
            return new string(tab);
        }
    }

    //Special DecodecMachine used by TravelAgencies
    class BookingDecodec
    {
        public int GetRealNumber(EncryptedData data)
        {
            SwapCodecDecorator swap = new SwapCodecDecorator(data);
            CezarCodecDecorator cezar = new CezarCodecDecorator(swap, 1);
            ReverseCodecDecorator reverse = new ReverseCodecDecorator(cezar);
            FrameDecodecDecorator fd = new FrameDecodecDecorator(reverse, 2);

            return int.Parse(fd.GetData());
        }
    }

    class TripAdvisorDecodec
    {
        public int GetRealNumber(EncryptedData data)
        {
            PushCodecDecorator push = new PushCodecDecorator(data, -3);
            SwapCodecDecorator swap = new SwapCodecDecorator(push);
            FrameDecodecDecorator fd = new FrameDecodecDecorator(swap, 2);
            push = new PushCodecDecorator(fd, -3);

            return int.Parse(push.GetData());
        }
    }

    class ShutterStockDecodec
    {
        public int GetRealNumber(EncryptedData data)
        {
            ReverseCodecDecorator reverse = new ReverseCodecDecorator(data);
            PushCodecDecorator push = new PushCodecDecorator(reverse, 3);
            FrameDecodecDecorator fd = new FrameDecodecDecorator(push, 1);
            CezarCodecDecorator cezar = new CezarCodecDecorator(fd, -4);

            return int.Parse(cezar.GetData());
        }
    }


}