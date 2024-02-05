using Newtonsoft.Json;
using ZemberekDotNet.Core.Turkish;
using ZemberekDotNet.Morphology;
using ZemberekDotNet.Tokenization;
using System.Collections.Generic;
using System;
using ZemberekDotNet.Core.Embeddings;
using QuickGraph;
using static System.Net.Mime.MediaTypeNames;
using Prolab3;
namespace prolab3
{
    class Program
    {

        static void Main()
        {

            var filePath = @"C:\Users\yaren\Desktop\plb3\prolab3\twitter_data.json\twitter_data.json";
            var jsonText = System.IO.File.ReadAllText(filePath);
            List<User> users = JsonConvert.DeserializeObject<List<User>>(jsonText);
            Graphy graphy = new Graphy(users);

            int userCount = users.Count;
            Console.WriteLine($"Toplam kullanıcı sayısı: {userCount}");
            User userToGet = new User(" ");
            Graf graf = new Graf();
            InterestTable interestTable = new InterestTable(1000);
            interestTable.AddUsers(users);

            Tablo htablo = new Tablo(1000);
            TurkishMorphology morphology = TurkishMorphology.CreateWithDefaults();
            myGraph graph = new myGraph();
            CustomLinkedList userList = new CustomLinkedList();

            foreach (var user in users)
            {

                Console.WriteLine($"Username: {user.Username}");
                var allTweets = string.Join(" ", user.Tweets);
                var words = allTweets.Split(new[] { ' ', ',', '\n', '\r', '.', '!', '?' });
                var rootFrequencies = CountRootFrequencies(words, morphology);
                var topRoots = GetTopRoots(rootFrequencies, 10);

                Dictionary<string, User> userDict2 = users.ToDictionary(u => u.Username, u => u);
                foreach (var followerUsername in user.Followers)
                {
                    if (user.Followers != null && user.FollowersUsers != null)
                    {
                        if (userDict2.TryGetValue(followerUsername, out var followerUser))
                        {
                            user.FollowersUsers.Add(followerUser);
                        }
                        else
                        {
                            throw new Exception($"Follower user '{followerUsername}' not found.");
                        }
                    }
                }
                Dictionary<string, User> userDict = users.ToDictionary(u => u.Username, u => u);

                foreach (var followingUsername in user.Following)
                {
                    if (userDict.TryGetValue(followingUsername, out var followingUser))
                    {
                        user.FollowingUsers.Add(followingUser);
                    }
                    else
                    {
                        Console.WriteLine($"Following user '{followingUsername}' not found.");
                    }
                }
                foreach (var followingUser in user.FollowingUsers)
                {
                    Console.WriteLine($"User '{user.Username}' is following user '{followingUser.Username}'");
                }
                foreach (var root in topRoots)
                {
                    if (user.Interests == null)
                    {
                        user.Interests = new List<string>();
                    }
                    user.Interests.Add(root.Key);
                }
              
                User userWithMaxFollowers = users.OrderByDescending(u => u.Followers_count).FirstOrDefault();
                if (userWithMaxFollowers != null)
                {
                    userToGet = userWithMaxFollowers;
                    List<User> usersWithSameInterest = graf.GetUsersWithSameInterest(userToGet);
                    Console.WriteLine($"User with max followers: {userToGet.Username}");
                }
                htablo.ekle(user);
                UpdateUserInterests(user);
                Console.WriteLine("\nTop 10 Words:");
                foreach (var root in topRoots)
                {
                    Console.WriteLine($"- {root.Key}: {root.Value} times");
                }
                Console.WriteLine("\n------------------------\n");
                interestTable.AddUserInterests(user);
            }
            Console.WriteLine("Lütfen aramak istediğiniz ilgi alanını girin:");
            string interestKeyword = Console.ReadLine();
            var usersWithInterest = interestTable.FindUsersWithInterest(interestKeyword);
            var graphyc = new Graphy(usersWithInterest);
            foreach (var edge in graphy.Edges)
            {
                Console.WriteLine($"Edge: {edge.Source.Username} -> {edge.Target.Username}");
            }
            foreach (var user in usersWithInterest)
            {
                Console.WriteLine($"Username: {user.Username}");
            }
            
            Console.Write("Enter the username of the first user: ");
            string username1 = Console.ReadLine();

            Console.Write("Enter the username of the second user: ");
            string username2 = Console.ReadLine();

            List<User> commonFollowers = graphy.FindCommonFollowers(username1, username2);

            if (commonFollowers.Count > 0)
            {
                Console.WriteLine($"Common Followers of {username1} and {username2}:");
                foreach (var follower in commonFollowers)
                {
                    Console.WriteLine(follower.Username);
                }
            }
            else
            {
                Console.WriteLine("No common followers.");
            }
            graphy.PrintGraphWithCommonFollowers(username1, username2);


            string dosyaAdi = "AnalizRaporu.txt";

            interestTable.GenerateAnalysisReport(dosyaAdi);
            Console.ReadLine();
        }
        static Dictionary<string, int> CountRootFrequencies(string[] words, TurkishMorphology morphology)
        {
            var frequencies = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            var commonWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
              { "etmek","eylemek","olmak","aa", "acaba", "ait", "altı", "a", "altmış", "ama", "amma", "anca", "ancağ", "ancak", "artık", "asla","iç","art", "aslında", "az", "b", "bana", "bari", "başkası","beri", "bazen", "bazı", "bazıları", "bazısı", "be", "belki", "ben", "bende", "benden", "beni", "benim", "beş", "bide", "bile", "bin", "bir", "birazı", "birçoğ", "birçoğu", "birçok", "birçokları", "biri", "birisi", "birkaç", "birkaçı", "birkez", "birşey", "birşeyi", "biz", "bizden", "bize", "bizi", "bizim", "böyle", "böylece",
                "bu", "buna", "bunda", "bundan", "bunu", "bunun", "burada", "bütün", "c", "ç", "çoğu", "çoğuna", "çoğunu", "çok", "çünkü", "d", "da", "daha", "dahi", "dandini", "de", "defa", "değ", "değil", "değin", "dek", "demek", "diğer", "diğeri", "diğerleri", "diye", "dk", "dha", "doğrusu", "doksan", "dokuz", "dolayı", "dört", "e", "eğer", "eh", "elbette", "elli", "en", "etkili", "f", "fakat", "fakad", "falan", "falanca", "felan", "filan", "filanca", "g", "ğ", "gene", "gereğ",
                "gibi", "göre", "görece", "h","ilk","son", "hakeza", "hakkında", "hâlâ", "halbuki", "hangi", "hangisi", "hani", "hasebiyle", "hatime", "hatta", "hele", "hem", "henüz", "hep", "hepsi", "hepsine", "hepsini", "her", "her biri", "herkes", "herkese", "herkesi", "hiç", "hiç kimse", "hiçbiri", "hiçbirine", "hiçbirini", "hoş", "i", "ı", "ın", "için", "içinde", "içre", "iki", "ila", "ile", "imdi", "indinde", "intağ", "intak", "ise", "işte", "ister", "j", "k", "kaç", "kaçı", "kadar", "kah", "karşın", "katrilyon",
                "kelli", "kendi", "kendine", "kendini", "keşke", "keşki", "kez", "keza", "kezaliğ", "kezalik", "ki", "kim", "kimden", "kime", "kimi", "kimin", "kimisi", "kimse", "kırk", "l", "67", "lakin", "m", "madem", "mademki", "mamafih", "meğer", "meğerki", "meğerse", "mi", "mı", "milyar", "milyon", "mu", "mü", "n", "nasıl", "nde", "ne", "ne kadar", "ne zaman", "neden", "nedense", "nedir", "nerde", "nere", "nerede", "nereden", "nereli", "neresi", "nereye", "nesi", "neye", "neyi", "neyse", "niçin", "ni", "nı", "nin", "nın",
                "nitekim", "niye", "o", "ö", "öbürkü", "öbürü", "on", "ön", "ona", "önce", "onda", "ondan", "onlar", "onlara", "onlardan", "onlari", "onların", "onu", "onun", "orada", "ötekisi", "ötürü", "otuz", "öyle", "oysa", "oysaki", "p", "pad", "pat", "peki", "r", "rağmen", "s", "ş", "sakın", "sana", "sanki", "şayet", "sekiz", "seksen", "sen", "senden", "seni", "senin", "şey", "şeyden", "şeye", "şeyi", "şeyler", "şimdi", "siz", "sizden", "size", "sizi", "sizin", "son", "sonra", "68", "şöyle", "şu",
                "şuna", "şunda", "şundan", "şunu", "şunun", "t", "ta", "tabi", "tamam", "tl", "trilyon", "tüm", "tümü", "u", "ü", "üç", "üsd", "üst", "uyarınca", "üzere", "v", "var", "ve", "velev", "velhasıl", "velhasılıkelam", "vesselam", "veya", "veyahud", "veyahut", "y", "ya", "ya da", "yani", "yazığ", "yazık", "yedi", "yekdiğeri", "yerine", "yetmiş", "yine", "yirmi", "yoksa", "yukarda", "yukardan", "yukarıda", "yukarıdan", "yüz", "z", "zaten", "zinhar", "zira",
                "gerek", "gibi", "göre", "görece", "h", "hakeza", "hakkında", "hâlâ", "halbuki", "hangi", "hangisi", "hani", "hasebiyle", "hatime", "hatta", "hele", "hem", "henüz", "hep", "hepsi", "hepsine", "hepsini", "her", "her biri", "herkes", "herkese", "herkesi", "hiç", "hiç kimse", "hiçbiri", "hiçbirine", "hiçbirini", "hoş", "i", "ı", "ın", "için", "içinde", "içre", "iki", "ila", "ile", "imdi", "indinde", "intağ", "intak", "ise", "işte", "ister", "j", "k", "kaç", "kaçı", "kadar", "kah", "karşın", "katrilyon",
                "kelli", "kendi", "kendine", "kendini", "keşke", "keşki", "kez", "keza", "kezaliğ", "kezalik", "ki", "kim", "kimden", "kime", "kimi", "kimin", "kimisi", "kimse", "kırk", "l", "67", "lakin", "m", "madem", "mademki", "mamafih", "meğer", "meğerki", "meğerse", "mi", "mı", "milyar", "milyon", "mu", "mü", "n", "nasıl", "nde", "ne", "ne kadar", "ne zaman", "neden", "nedense", "nedir", "nerde", "nere", "nerede", "nereden", "nereli", "neresi", "nereye", "nesi", "neye", "neyi", "neyse", "niçin", "ni", "nı", "nin", "nın",
                "nitekim", "niye", "o", "ö", "öbürkü", "öbürü", "on", "ön", "ona", "önce", "onda", "ondan", "onlar", "onlara", "onlardan", "onlari", "onların", "onu", "onun", "orada", "ötekisi", "ötürü", "otuz", "öyle", "oysa", "oysaki", "p", "pad", "pat", "peki", "r", "rağmen", "s", "ş", "sakın", "sana", "sanki", "şayet", "sekiz", "seksen", "sen", "senden", "seni", "senin", "şey", "şeyden", "şeye", "şeyi", "şeyler", "şimdi", "siz", "sizden", "size", "sizi", "sizin", "son", "sonra", "68", "şöyle", "şu",
                "şuna", "şunda", "şundan", "şunu", "şunun", "t", "ta", "tabi", "tamam", "tl", "trilyon", "tüm", "tümü", "u", "ü", "üç", "üsd", "üst", "uyarınca", "üzere", "v", "var", "ve", "velev", "velhasıl", "velhasılıkelam", "vesselam", "veya", "veyahud", "veyahut", "y", "ya", "ya da", "yani", "yazığ", "yazık", "yedi", "yekdiğeri", "yerine", "yetmiş", "yine", "yirmi", "yoksa", "yukarda", "yukardan", "yukarıda", "yukarıdan", "yüz", "z", "zaten", "zinhar", "zira"
            };
            var suffixesToRemove = new HashSet<string> { "mek", "mak" };
            foreach (var word in frequencies.Keys.ToList())
            {
                if (suffixesToRemove.Any(suffix => word.EndsWith(suffix)))
                {
                    frequencies.Remove(word);
                }
            }
            foreach (var word in words)
            {
                var results = morphology.Analyze(word);
                foreach (var analysis in results)
                {
                    var root = analysis.GetDictionaryItem().lemma;
                    if (!commonWords.Contains(root))
                    {
                        if (frequencies.ContainsKey(root))
                        {
                            frequencies[root]++;
                        }
                        else
                        {
                            frequencies[root] = 1;
                        }
                    }
                }
            }
            frequencies = frequencies.Where(kv => !suffixesToRemove.Any(suffix => kv.Key.EndsWith(suffix)))
                             .ToDictionary(kv => kv.Key, kv => kv.Value);

            return frequencies;
        }

        static void UpdateUserInterests(User user)
        {
            user.UpdateInterests(user);
        }

        static Dictionary<string, int> GetTopRoots(Dictionary<string, int> rootFrequencies, int count)
        {
            return rootFrequencies.OrderByDescending(x => x.Value).Take(count).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
