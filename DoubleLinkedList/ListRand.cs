using System;
using System.Collections;
using System.IO;

namespace DoubleLinkedList
{
    public class ListRand : IEnumerable
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        public ListRand()
        {
            Head = null;
            Tail = null;
            Count = 0;
        }

        public void Serialize(BinaryWriter s)
        {
            s.Write(Count);
            
            int currentNodeIndex = 0;

            ListNode currentNode = Head;
            while (currentNode != null)
            {
                var randNode = Head;

                // Calculate each node random index
                var randomNodeIndex = 0;
                while (randNode != currentNode.Rand)
                {
                    randomNodeIndex++;
                    randNode = randNode.Next;
                }
                // Write fields to binary file
                s.Write(randomNodeIndex);
                s.Write(currentNode.Data);
                s.Write(currentNodeIndex);

                currentNodeIndex++;
                currentNode = currentNode.Next;
            }
        }
        
        public void Deserealize(BinaryReader s)
        {
            int count = s.ReadInt32();
            int[] randomNodeIndex = new int[count];
            
            // Read data form binary file and node
            while (s.PeekChar() > -1)
            {
                int rand = s.ReadInt32();
                string data = s.ReadString();
                int cur = s.ReadInt32();

                randomNodeIndex[cur] = rand;

                Add(data);
            }

            // Recover random links of each node
            ListNode currentNode = Head;
            int index = 0;
            int middle = (count / 2) + 1;
            while (currentNode != null)
            {
                ListNode randomNode;
                if (randomNodeIndex[index] <= middle)
                {
                    randomNode = Head;
                    for (int i = 0; i < randomNodeIndex[index]; i++)
                    {
                        randomNode = randomNode.Next;
                    }
                }
                else
                {
                    randomNode = Tail;
                    for (int i = count - 1; i > randomNodeIndex[index]; i--)
                    {
                        randomNode = randomNode.Prev;
                    }
                }

                index++;
                currentNode.Rand = randomNode;
                currentNode = currentNode.Next;
            }
        }
        
        public void Add(string data)
        {
            ListNode newNode = new ListNode();

            if (Head == null)
            {
                Head = newNode;
            }
            else
            {
                Tail.Next = newNode;
                newNode.Prev = Tail;
            }

            Tail = newNode;
            Tail.Data = data;
            Count++;
        }

        /// <summary>
        /// Set random link to each node in list
        /// </summary>
        public void SetRandNodes()
        {
            Random rand = new Random();
            
            ListNode current = Head;
            while (current != null)
            {
                var randNode = Head;
                int itemIndex = rand.Next(0, Count);
                for (int i = 0; i < itemIndex; i++)
                {
                    randNode = randNode.Next;
                }

                current.Rand = randNode;
                current = current.Next;
            }
        }

        public IEnumerator GetEnumerator()
        {
            ListNode current = Head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
    }
}