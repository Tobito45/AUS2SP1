using KdTree.Structuries;
using System.Xml.Linq;

namespace KdTree
{
    internal class Program
    {
        static void Main(string[] args)
        {
            KdTree<City> kdTree = new KdTree<City>(2);

            //CreateKDTreeFromLecture(kdTree);
            //CreateKDTreeFromLectureNoSorted(kdTree);

            CreateKdTreeFromGenerator(kdTree, 50);

            //CreateKDTreeFromLectureWithSameElements(kdTree);

            //CreateKDTreeFromLectureWithSameElementsForDelete(kdTree);

            //CreateKDTreeWithSameNodes(kdTree);

            foreach (City element in kdTree.GetLevelOrder(level =>
            {
                Console.WriteLine();
                Console.WriteLine($"Level:{level}-------");
            }))
            {
                Console.Write($"{element.ToString()} | ");
            }

            Console.WriteLine();
            Console.WriteLine();
            foreach (Node<City> element in kdTree.GetLevelOrderNodes(level =>
            {
                Console.WriteLine($"Level:{level}");
            }))
            {
                Console.WriteLine($"{element.Data.ToString()} - ({element.Parent?.Data.Name}) => " +
                    $"({element.LeftSon?.Data.ToString()}),({element.RightSon?.Data.ToString()})  | ");
            }
        }

        private static void CreateKdTreeFromGenerator(KdTree<City> kdTree, int count)
        {
            OperationGeneratorCities generator = new OperationGeneratorCities(kdTree, (1, 500), (1, 500), null);
            generator.GenerateInsert(count).GenerateFind(count, (list) =>
            {
                Console.WriteLine("--------------");
                foreach (City element in list)
                {
                    Console.WriteLine(element.ToString());
                }
                Console.WriteLine("--------------");
            }).GenerateDelete(count, (list, before, after, data) =>
            {
                Console.WriteLine("===============");
                Console.WriteLine($"Count before: {before}");
                Console.WriteLine($"Count after: {after}");
                Console.WriteLine($"Data: {data.ToString()}");

                foreach (City element in list)
                {
                    Console.WriteLine(element.ToString());
                }
                Console.WriteLine("===============");
            });
        }

        private static void CreateKDTreeFromLecture(KdTree<City> kdTree)
        {
            kdTree.Insert(new City("Nitra", (23, 35)));
            kdTree.Insert(new City("Sered", (20, 33)));
            kdTree.Insert(new City("Topolcanky", (25, 36)));
            kdTree.Insert(new City("Bosany", (24, 40)));
            kdTree.Insert(new City("Tlmace", (28, 34)));
            kdTree.Insert(new City("Moravce", (26, 35)));
            kdTree.Insert(new City("Levice", (30, 33)));
            kdTree.Insert(new City("Bojnice", (29, 46)));
            kdTree.Insert(new City("Novaky", (27, 43)));

            kdTree.Insert(new City("Galanda", (16, 31)));
            kdTree.Insert(new City("Senica", (14, 39)));
            kdTree.Insert(new City("Bratislava", (13, 32)));
            kdTree.Insert(new City("Hodin", (12, 41)));
            kdTree.Insert(new City("Trnava", (17, 42)));

        }

        private static void CreateKDTreeFromLectureNoSorted(KdTree<City> kdTree)
        {
            List<City> cities = new List<City>();

            cities.Add(new City("Bojnice", (29, 46)));
            cities.Add(new City("Bosany", (24, 40)));
            cities.Add(new City("Bratislava", (13, 32)));
            cities.Add(new City("Galanda", (16, 31)));
            cities.Add(new City("Hodin", (12, 41)));
            cities.Add(new City("Levice", (30, 33)));
            cities.Add(new City("Moravce", (26, 35)));
            cities.Add(new City("Nitra", (23, 35)));
            cities.Add(new City("Novaky", (27, 43)));
            cities.Add(new City("Senica", (14, 39)));
            cities.Add(new City("Sered", (20, 33)));
            cities.Add(new City("Trnava", (17, 42)));
            cities.Add(new City("Topolcanky", (25, 36)));
            cities.Add(new City("Tlmace", (28, 34)));
            kdTree.Insert(cities, (list) => Console.WriteLine(string.Join(",", list.Select(n => n.Name))));


        }

        private static void CreateKDTreeFromLectureWithSameElements(KdTree<City> kdTree)
        {
            kdTree.Insert(new City("Nitra", (23, 35)));
            kdTree.Insert(new City("Tlmance - urad", (24, 36)));
            kdTree.Insert(new City("Tlmance", (24, 34)));
            kdTree.Insert(new City("Tlmace - nem", (24, 35)));
            kdTree.Insert(new City("Tlmace - parkovicko", (24, 40)));
            kdTree.Insert(new City("Levice", (30, 33)));
            kdTree.Insert(new City("Bojnice", (29, 46)));
            kdTree.Insert(new City("Novaky", (27, 43)));

            kdTree.Insert(new City("Senica", (22, 39)));
            kdTree.Insert(new City("Senica - skola", (22, 31)));
            kdTree.Insert(new City("Senica - stanice", (22, 42)));
            kdTree.Insert(new City("Senica - urad", (22, 32)));
            kdTree.Insert(new City("Hodin", (12, 41)));
        }

        private static void CreateKDTreeFromLectureWithSameElementsForDelete(KdTree<City> kdTree)
        {
            kdTree.Insert(new City("Senica", (22, 39)));
            kdTree.Insert(new City("Tlmance - urad", (24, 36)));
            kdTree.Insert(new City("Tlmance", (24, 34)));
            kdTree.Insert(new City("Tlmace - nem", (24, 35)));
            kdTree.Insert(new City("Tlmace - nem2", (24, 35)));
            kdTree.Insert(new City("Tlmace - parkovicko", (24, 40)));
            kdTree.Insert(new City("Levice", (30, 33)));
            kdTree.Insert(new City("Bojnice", (29, 46)));
            kdTree.Insert(new City("Novaky", (27, 43)));
        }

        private static void CreateKDTreeWithSameNodes(KdTree<City> kdTree)
        {
            kdTree.Insert(new City("Nitra", (23, 35)));
            kdTree.Insert(new City("Nitra1", (23, 35)));
            kdTree.Insert(new City("Nitra2", (23, 35)));
            kdTree.Insert(new City("Nitra3", (23, 35)));
        }
    }
}