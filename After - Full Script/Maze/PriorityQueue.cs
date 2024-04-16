using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PriorityQueue<T>
{
    private class Item
    {
        public T value;
        public float priority;

        public Item(T value, float priority) { this.value = value; this.priority = priority; }
    }

    private List<Item> items = new List<Item>();

    public void Enqueue(T value, float priority)
    {
        int indexToPut = 0;
        Item itemToPut = new Item(value, priority);

        items.Add(itemToPut);

        for(int i = 0; i < items.Count-1; i++)
        {
            if (items[i].priority <= priority)
            {
                indexToPut++;

            }
            else break;
        }

        for(int i = items.Count-1; i > indexToPut; i--)
        {
            items[i] = items[i - 1];
        }

        items[indexToPut] = itemToPut;
    }

    public T GetFirstAndDequeue()
    {
        T item = items[0].value;

        items.Remove(items[0]);
        return item;
    }

    public int Count
    {
        get { 
            return items.Count;
        }
    }
}
