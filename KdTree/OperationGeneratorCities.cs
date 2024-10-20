using KdTree.Generator;
using KdTree.Structuries;

namespace KdTree
{
    internal class OperationGeneratorCities : OperationGenerator<KdTree<City>, City>
    {
        private readonly (int minX, int maxX) xCoordinates;
        private readonly (int minY, int maxY) yCoordinates;
        public OperationGeneratorCities(KdTree<City> structure, (int minX, int maxX) xCoordinates,
            (int minY, int maxY) yCoordinates, int? seed) : base(structure, seed) 
        {
            this.xCoordinates = xCoordinates;
            this.yCoordinates = yCoordinates;
        }
        public override City GenerateRadomData()
        {
            return new City(GenerateRandomWord(8), ((int)random.NextInt64(xCoordinates.minX, xCoordinates.maxX),
                                (int)random.NextInt64(yCoordinates.minY, yCoordinates.maxY)));
        }

        public string GenerateRandomWord(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            char[] word = new char[length];
            for (int i = 0; i < length; i++)
            {
                word[i] = chars[random.Next(chars.Length)];
            }
            return new string(word);
        }
    }
}
