using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prolab3
{
    public class InterestTable
    {
        private readonly LinkedList<User>[] table;
        private readonly int size;
        public InterestTable(int tableSize)
        {
            size = tableSize;
            table = new LinkedList<User>[size];
            for (int i = 0; i < size; i++)
            {
                table[i] = new LinkedList<User>();
            }
        }
        private ulong CalculateKey(string interest)
        {
            ulong key = 14695981039346656037UL;
            foreach (char c in interest)
            {
                key ^= (ulong)c;
                key *= 1099511628211UL;
            }
            return key;
        }
        private int HashFunction(ulong key)
        {
            return (int)(key % (ulong)size);
        }

        public void AddUserInterests(User user)
        {
            foreach (var interest in user.Interests)
            {
                ulong interestKey = CalculateKey(interest);
                int index = HashFunction(interestKey);
                if (table[index] == null)
                {
                    table[index] = new LinkedList<User>();
                }
                if (!table[index].Contains(user))
                {
                    table[index].AddLast(user);
                }
            }
        }

        public List<User> FindUsersWithInterest(string interest)
        {
            ulong interestKey = CalculateKey(interest);
            int index = HashFunction(interestKey);

            return table[index].Where(user => user.Interests.Contains(interest)).ToList();
        }
        public void AddUsers(List<User> users)
        {
            foreach (var user in users)
            {
                AddUserInterests(user);
            }
        }

        public void GenerateAnalysisReport(string fileName)
        {
            try
            {
                using (StreamWriter file = new StreamWriter(fileName))
                {
                    file.WriteLine("İlgi Alanları Analiz Raporu");
                    file.WriteLine($"Rapor Tarihi: {DateTime.Now}");
                    file.WriteLine();
                    file.WriteLine("Kullanıcılar ve İlgi Alanları:");
                    foreach (var list in table)
                    {
                        if (list != null)
                        {
                            foreach (var user in list)
                            {
                                file.WriteLine($"{user.Username} - İlgi Alanları: {string.Join(", ", user.Interests)}");
                            }
                        }
                    }
                }

                Console.WriteLine($"Analiz raporu başarıyla oluşturuldu ve '{fileName}' adıyla kaydedildi.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Rapor oluşturulurken bir hata oluştu: {ex.Message}");
            }
        }
    }
}
