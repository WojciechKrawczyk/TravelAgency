using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//  Potwierdzam samodzielność powyższej pracy oraz niekorzystanie przeze mnie z niedozwolonych źródeł
//  <Wojciech> <Krawczyk>

//Oyster is a website with reviews of various holiday destinations.
namespace TravelAgencies.DataAccess
{
	public class BSTNode
	{
		public string Review { get; set; }
		public string UserName { get; set; }
		public BSTNode Left { get; set; }
		public BSTNode Right { get; set; }
	}

	public class OysterDatabase : IContainer<BSTNode>
	{
		public BSTNode Reviews { get; set; }
        private int counterIndex = 0;
        private int searchIndex = -1;
        private BSTNode searchNode = null;

        public IIterator<BSTNode> GetIterator()
        {
            return new OysterIterator(this);
        }

        public BSTNode this[int index]
        {
            get
            {
                if (index >= 0 && index < this.Count && Reviews != null)
                {
                    this.searchIndex = -1;
                    InOrderSearch(Reviews, index);
                    return this.searchNode;
                }
                else
                    return null;
            }
        }

        public int Count
        {
            get
            {
                if (Reviews != null)
                {
                    this.counterIndex = 0;
                    InOrder(Reviews);
                    return this.counterIndex;
                }
                return 0;
            }
        }

        private void InOrder(BSTNode node)
        {
            if (node.Left != null)
                InOrder(node.Left);
            this.counterIndex++;
            if (node.Right != null)
                InOrder(node.Right);
        }

        private void InOrderSearch(BSTNode node, int i)
        {
            if (node.Left != null)
                InOrderSearch(node.Left, i);

            this.searchIndex++;

            if (this.searchIndex == i)
            {
                this.searchNode = node;
                return;
            }

            if (node.Right != null)
                InOrderSearch(node.Right, i);
        }
    }

    public class OysterIterator : IIterator<BSTNode>
    {
        OysterDatabase database = null;
        int index = 0;

        public OysterIterator(OysterDatabase database)
        {
            this.database = database;
        }

        public void Reset()
        {
            this.index = 0;
        }

        public BSTNode GetNext
        {
            get
            {
                if (HasNext)
                {
                    BSTNode tmp = database[index];
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
