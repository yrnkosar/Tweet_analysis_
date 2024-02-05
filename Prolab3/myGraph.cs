
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prolab3
{
    public class myGraph
    {
        public Dictionary<User, List<User>> userGraph;
        public myGraph()
        {
            userGraph = new Dictionary<User, List<User>>();
        }
        public void AddEdge(User fromUser, User toUser)
        {
            if (!userGraph.ContainsKey(fromUser))
            {
                userGraph[fromUser] = new List<User>();
            }

            if (!userGraph.ContainsKey(toUser))
            {
                userGraph[toUser] = new List<User>();
            }

            userGraph[fromUser].Add(toUser);
        }
        public List<User> GetFollowingUsers(User user)
        {
            if (userGraph.ContainsKey(user))
            {
                return userGraph[user];
            }

            return new List<User>();
        }
    }
}
