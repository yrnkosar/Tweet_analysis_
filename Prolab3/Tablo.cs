using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prolab3
{
    public class Tablo
    {

        Node[] dizi;
        public int size;
        public Tablo(int size)
        {
            this.size = size;
            dizi = new Node[size];

            for (int i = 0; i < size; i++)
            {
                dizi[i] = null;
            }
        }

        int indexUret(ulong key)
        {
            return (int)(key % (ulong)size);
        }
        ulong MultiplyAddAndDivide(ulong value)
        {
            ulong mad = value * 3 + 5;
            mad /= 2;
            return mad;
        }

        public void ekle(User user)
        {
            ulong key = MultiplyAddAndDivide(user.Id);
            int index = indexUret(key);
            if (index >= size || index < 0)
            {
                Console.WriteLine("Geçersiz indeks!");
                return;
            }
            Node eleman = new Node(key, user);

            if (dizi[index] == null)
            {
                dizi[index] = eleman;
                Console.WriteLine($"{user.Username},{user.Id} eklendi");
            }
            else
            {
                Node temp = dizi[index];

                while (temp.next != null)
                {
                    temp = temp.next;
                }

                temp.next = eleman;
                Console.WriteLine($"{user.Username},{user.Id} eklendi");
            }
        }

    }
}
