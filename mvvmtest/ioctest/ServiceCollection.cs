using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Collections.Generic;

namespace ioctest
{
    internal class ServiceCollec111tion : IServiceCollection
    {
        ServiceDescriptor IList<ServiceDescriptor>.this[int index] { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        int ICollection<ServiceDescriptor>.Count => throw new System.NotImplementedException();

        bool ICollection<ServiceDescriptor>.IsReadOnly => throw new System.NotImplementedException();

        void ICollection<ServiceDescriptor>.Add(ServiceDescriptor item)
        {
            throw new System.NotImplementedException();
        }

        void ICollection<ServiceDescriptor>.Clear()
        {
            throw new System.NotImplementedException();
        }

        bool ICollection<ServiceDescriptor>.Contains(ServiceDescriptor item)
        {
            throw new System.NotImplementedException();
        }

        void ICollection<ServiceDescriptor>.CopyTo(ServiceDescriptor[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        IEnumerator<ServiceDescriptor> IEnumerable<ServiceDescriptor>.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        int IList<ServiceDescriptor>.IndexOf(ServiceDescriptor item)
        {
            throw new System.NotImplementedException();
        }

        void IList<ServiceDescriptor>.Insert(int index, ServiceDescriptor item)
        {
            throw new System.NotImplementedException();
        }

        bool ICollection<ServiceDescriptor>.Remove(ServiceDescriptor item)
        {
            throw new System.NotImplementedException();
        }

        void IList<ServiceDescriptor>.RemoveAt(int index)
        {
            throw new System.NotImplementedException();
        }
    }
}