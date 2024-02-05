using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prolab3
{
    public class Graf
    {
        public Dictionary<string, List<User>> interestAdjacencyList;
        public Dictionary<User, List<User>> userAdjacencyList;
        public Graf()
        {
            userAdjacencyList = new Dictionary<User, List<User>>();
            interestAdjacencyList = new Dictionary<string, List<User>>();
        }
        public void AddUser(User user)
        {
            if (!userAdjacencyList.ContainsKey(user))
            {
                userAdjacencyList[user] = new List<User>();
            }
            foreach (string interest in user.Interests)
            {
                if (!interestAdjacencyList.ContainsKey(interest))
                {
                    interestAdjacencyList[interest] = new List<User>();
                }

                if (!interestAdjacencyList[interest].Contains(user))
                {
                    interestAdjacencyList[interest].Add(user);
                }
            }
        }

        public List<User> GetUsersWithSameInterest(User user)
        {
            List<User> usersWithSameInterest = new List<User>();
            foreach (string interest in user.Interests)
            {
                if (interestAdjacencyList.ContainsKey(interest))
                {
                    usersWithSameInterest.AddRange(interestAdjacencyList[interest]);
                }
            }
            return usersWithSameInterest.Distinct().ToList();
        }
        public List<User> BFS(User startUser)
        {
            List<User> visited = new List<User>();
            Queue<User> queue = new Queue<User>();

            visited.Add(startUser);
            queue.Enqueue(startUser);

            while (queue.Count != 0)
            {
                User currentUser = queue.Dequeue();

                foreach (User adjacentUser in userAdjacencyList[currentUser])
                {
                    if (!visited.Contains(adjacentUser))
                    {
                        visited.Add(adjacentUser);
                        queue.Enqueue(adjacentUser);
                        List<User> relatedUsers = GetUsersWithSameInterest(adjacentUser);
                        foreach (User relatedUser in relatedUsers)
                        {
                            if (!visited.Contains(relatedUser))
                            {
                                visited.Add(relatedUser);
                                queue.Enqueue(relatedUser);
                            }
                        }
                    }
                }
            }

            return visited;
        }
    }
}
