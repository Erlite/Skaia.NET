
// ------------------------------------------
// Copyright (c) 2018 All Rights Reserved
// Author: Younes Meziane
// Purpose: Thread-safe queue implementation.
// ------------------------------------------

using System.Collections.Generic;

namespace Skaia.Surrogates
{
    public class SafeQueue<T>
    {
        private readonly Queue<T> queue = new Queue<T>();

        /// <summary>
        /// Enqueue an item to the queue.
        /// </summary>
        /// <param name="item"></param>
        public void Enqueue(T item)
        {
            lock (queue)
            {
                queue.Enqueue(item);
            }
        }

        /// <summary>
        /// Try to dequeue an item from the queue.
        /// </summary>
        /// <param name="item"> The item retrieved if succesful. </param>
        /// <returns> True on success. </returns>
        public bool TryDequeue(out T item)
        {
            lock (queue)
            {
                if (queue.Count > 0)
                {
                    item = queue.Dequeue();
                    return true;
                }

                item = default(T);
                return false;
            }
        }
    }
}