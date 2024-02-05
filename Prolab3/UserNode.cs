using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prolab3
{
    public class UserNode
    {
        public User User { get; }
        public List<UserNode> SimilarInterestUsers { get; }
        public UserNode Next { get; set; }
        public UserNode(User user)
        {
            User = user;
            SimilarInterestUsers = new List<UserNode>();
            Next = null;
        }

        public void AddSimilarInterestUser(UserNode userNode)
        {
            SimilarInterestUsers.Add(userNode);
        }
    }

    public class CustomLinkedList
    {
        private UserNode head;

        public UserNode GetHead()
        {
            return head;
        }

    }
}
