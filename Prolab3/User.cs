using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prolab3
{
    public class User
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public int Followers_count { get; set; }
        public int Following_count { get; set; }
        public List<string> Tweets { get; set; }
        public List<string> Following { get; set; }
        public List<string> Followers { get; set; }
        public string Language { get; set; }
        public string Region { get; set; }
        public List<User> FollowingUsers { get; set; }
        public List<User> FollowersUsers { get; set; }
        public ulong Id { get; }
        public List<string> Interests { get; set; }

        public User(string username)
        {
            Username = username;
            Interests = new List<string>();
            FollowingUsers = new List<User>();
            FollowersUsers = new List<User>();
            Id = GenerateUserId();

        }
        public Dictionary<string, int> CountRootFrequencies()
        {
            var frequencies = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            foreach (var interest in Interests)
            {
                if (frequencies.ContainsKey(interest))
                {
                    frequencies[interest]++;
                }
                else
                {
                    frequencies[interest] = 1;
                }
            }

            return frequencies;
        }

        private ulong GenerateUserId()
        {
            ulong Id = 0;
            int maxLength = Math.Min(8, Username.Length);
            for (int i = 0; i < maxLength; i++)
            {
                int asciiValue = (int)Username.ToUpper()[i];
                Id = Id * 100 + (ulong)asciiValue;
            }

            if (Username.Length > 8)
            {
                for (int i = 8; i < Username.Length; i++)
                {
                    int asciiValue = (int)Username.ToUpper()[i];
                    Id += (ulong)asciiValue;
                }
            }

            return Id;
        }
        public void UpdateInterests(User user)
        {
            var rootFrequencies = user.CountRootFrequencies();
            var maxInterests = 10;
            var count = 0;

            foreach (var root in rootFrequencies.OrderByDescending(x => x.Value))
            {
                if (count >= maxInterests)
                {
                    break;
                }

                if (!user.Interests.Contains(root.Key))
                {
                    user.Interests.Add(root.Key);
                    count++;
                }

            }
        }

        public List<string> FindCommonFollowersWith(User otherUser)
        {
            List<string> commonFollowers = Followers.Intersect(otherUser.Followers).ToList();
            return commonFollowers;
        }

    }
}
