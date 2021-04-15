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
	public class ListNode
	{
		public ListNode Next { get; set; }
		public string Name { get; set; }
		public string Rating { get; set; }//Encrypted
		public string Price { get; set; }//Encrypted
	}

	public class BookingDatabase : IContainer<ListNode>
	{
		public ListNode[] Rooms { get; set; }

        public IIterator<ListNode> GetIterator()
        {
            return new BookingIterator(this);
        }

        public ListNode this[int index]
        {
            get
            {
                if (index >= 0 && index < this.Count && Rooms != null) 
                {
                    int cur = -1;
                    int k = 0;
                    ListNode ret = null;
                    //start copy
                    ListNode[] position = new ListNode[Rooms.Length];
                    for (int i = 0; i < position.Length; i++)
                    {
                        if (Rooms[i] != null)
                        {
                            ListNode p = new ListNode();
                            p.Name = Rooms[i].Name;
                            p.Price = Rooms[i].Price;
                            p.Rating = Rooms[i].Rating;
                            p.Next = null;
                            position[i] = p;
                            ListNode n = Rooms[i].Next;
                            while (n != null)
                            {
                                ListNode pt = new ListNode();
                                pt.Name = n.Name;
                                pt.Price = n.Price;
                                pt.Rating = n.Rating;
                                pt.Next = null;
                                p.Next = pt;
                                p = pt;
                                n = n.Next;
                            }
                        }
                    }
                    //end copy
                    while(cur != index)
                    {
                        if (position[k] != null)
                        {
                            cur++;
                            ret = position[k];
                            position[k] = position[k].Next;
                        }
                        k++;
                        if (k == Rooms.Length)
                            k = 0;
                    }
                    ret.Next = null;
                    return ret;
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
                ListNode p = new ListNode();
                for (int i = 0; i < Rooms.Length; i++)
                {
                    p = Rooms[i];
                    while (p != null)
                    {
                        count++;
                        p = p.Next;
                    }
                }
                return count;
            }
        }
    }

    public class BookingIterator: IIterator<ListNode>
    {
        BookingDatabase database = null;
        int index = 0;

        public BookingIterator(BookingDatabase database)
        {
            this.database = database;
        }

        public void Reset()
        {
            this.index = 0;
        }

        public ListNode GetNext
        {
            get
            {
                if(HasNext)
                {
                    ListNode tmp = database[index];
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
