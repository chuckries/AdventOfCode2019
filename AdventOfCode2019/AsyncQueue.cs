﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019
{
    class AsyncQueue<T>
    {
        public AsyncQueue()
        {
            _items = new Queue<T>();
        }

        public AsyncQueue(IEnumerable<T> items)
        {
            _items = new Queue<T>(items);
        }

        public void Enqueue(T item)
        {
            lock(_lock)
            {
                if (_outstandingRequests.Count > 0)
                {
                    var request = _outstandingRequests.Dequeue();
                    request.SetResult(item);
                }
                else
                {
                    _items.Enqueue(item);
                }
            }
        }

        public Task<T> Dequeue()
        {
            lock(_lock)
            {
                if (_items.Count > 0)
                    return Task.FromResult(_items.Dequeue());
                else
                {
                    TaskCompletionSource<T> request = new TaskCompletionSource<T>();
                    _outstandingRequests.Enqueue(request);
                    return request.Task;
                }
            }
        }

        Queue<T> _items;
        Queue<TaskCompletionSource<T>> _outstandingRequests = new Queue<TaskCompletionSource<T>>();
        object _lock = new object();
    }
}
