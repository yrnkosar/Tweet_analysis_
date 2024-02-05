using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prolab3
{
    public class Node
    {
        public ulong Key;
        public User User;
        public Node next;

        public Node(ulong key, User user)
        {
            Key = key;
            User = user;
            next = null;
        }

    }
}
