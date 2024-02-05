using prolab3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolab3
{
    public class Graphy
    {
        public List<User> Users { get; set; }
        public List<Edge> Edges { get; set; }

        public Graphy(List<User> users)
        {
            Users = users;
            Edges = new List<Edge>();

            foreach (var user in Users)
            {
                foreach (var followerUsername in user.Followers)
                {
                    var follower = Users.FirstOrDefault(u => u.Username.Equals(followerUsername, StringComparison.OrdinalIgnoreCase));
                    if (follower != null)
                    {
                        Edges.Add(new Edge(user, follower));
                    }
                }
            }
        }

        public List<User> FindCommonFollowers(string username1, string username2)
        {
            User user1 = Users.FirstOrDefault(u => u.Username.Equals(username1, StringComparison.OrdinalIgnoreCase));
            User user2 = Users.FirstOrDefault(u => u.Username.Equals(username2, StringComparison.OrdinalIgnoreCase));

            if (user1 != null && user2 != null)
            {
                List<string> commonFollowersUsernames = user1.FindCommonFollowersWith(user2);
                List<User> commonFollowers = Users.Where(u => commonFollowersUsernames.Contains(u.Username)).ToList();

                return commonFollowers;
            }

            return new List<User>();
        }
        public void PrintGraphWithCommonFollowers(string username1, string username2)
        {
            Console.WriteLine("Graph Structure:");

            foreach (var user in Users)
            {
                foreach (var followerUsername in user.Followers)
                {
                    var follower = Users.FirstOrDefault(u => u.Username.Equals(followerUsername, StringComparison.OrdinalIgnoreCase));
                    if (follower != null && IsCommonFollower(user, follower, username1, username2))
                    {
                        Console.WriteLine($"(Edge: {user.Username} -> {follower.Username})");
                    }
                }

                Console.WriteLine();
            }
        }
        private bool IsCommonFollower(User user, User follower, string username1, string username2)
        {
            List<string> commonFollowersUsernames1 = Users
                .FirstOrDefault(u => u.Username.Equals(username1, StringComparison.OrdinalIgnoreCase))
                ?.FindCommonFollowersWith(user) ?? new List<string>();

            List<string> commonFollowersUsernames2 = Users
                .FirstOrDefault(u => u.Username.Equals(username2, StringComparison.OrdinalIgnoreCase))
                ?.FindCommonFollowersWith(user) ?? new List<string>();

            return commonFollowersUsernames1.Contains(follower.Username) && commonFollowersUsernames2.Contains(follower.Username);
        }
        public class Edge
        {
            public User Source { get; set; }
            public User Target { get; set; }

            public Edge(User source, User target)
            {
                Source = source;
                Target = target;
            }
        }
    }
}
