using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartssystemServer.Network
{
    public class ThreadSafeList<T> : IList<T>
    {
        private readonly object _lock;
        private List<T> _list;

        public ThreadSafeList()
        {
            _lock = new object();
            _list = new List<T>();
        }

        public T this[int index]
        {
            get
            {
                lock (_lock)
                {
                    return _list[index];
                }
            }
            set
            {
                lock (_lock)
                {
                    _list[index] = value;
                }
            }
        }

        public int Count()
        {
            lock (_lock)
            {
                return _list.Count;
            }
        }

        public bool IsReadOnly => false;

        int ICollection<T>.Count
        {
            get
            {
                lock (_lock)
                {
                    return _list.Count;
                }
            }

        }

        public void Add(T item)
        {
            lock (_lock)
            {
                _list.Add(item);
            }
        }


        public void Clear()
        {
            lock (_lock)
            {
                _list.Clear();
            }
        }

        public bool Contains(T item)
        {
            lock (_lock)
            {
                return _list.Contains(item);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock (_lock)
            {
                //verkrijg de enumerator van een kloon omdat de enumerator zelf niet ts is.
                return _list.ToList().GetEnumerator();
            }
        }

        public int IndexOf(T item)
        {
            lock (_lock)
            {
                return _list.IndexOf(item);
            }
        }

        public void Insert(int index, T item)
        {
            lock (_lock)
            {
                _list.Insert(index, item);
            }
        }

        public bool Remove(T item)
        {
            lock (_lock)
            {
                return _list.Remove(item);
            }
        }

        public void RemoveAt(int index)
        {
            lock (_lock)
            {
                _list.RemoveAt(index);
            }
        }

        public T Find(Predicate<T> predicate)
        {
            lock (_lock)
            {
                return _list.Find(predicate);
            }
        }

        public void RemoveAll(Predicate<T> predicate)
        {
            lock (_lock)
            {
                _list.RemoveAll(predicate);
            }
        }

        //NOTE! Maakt kopieën waardoor references verloren gaan!s 
        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (_lock)
            {
                //verkrijg de enumerator van een kloon omdat de enumerator zelf niet ts is.
                return _list.ToList().GetEnumerator();
            }
        }


    }
}
