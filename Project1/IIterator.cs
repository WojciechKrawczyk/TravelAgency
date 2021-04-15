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
    public interface IContainer<T>
    {
        IIterator<T> GetIterator();
        T this[int index] { get; }
        int Count { get; }
    }

    public interface IIterator<T>
    {
        void Reset();
        T GetNext{ get; }
        bool HasNext { get; }
    }
}