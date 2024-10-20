namespace KdTree
{
    internal class City : Structuries.IKdTreeComparable<City>
    {
        public string Name { get; private set; }
        public (int x, int y) Cordinates { get; private set; }

        public City(string name, (int x, int y) cordinates)
        {
            Name = name;
            Cordinates = cordinates;
        }

        public int Compare(City other, int level)
        {
            int compareParameterThis = 0;
            int compareParameterOther = 0;
            switch (level)
            {
                case 1:
                    compareParameterThis = Cordinates.x;
                    compareParameterOther = other.Cordinates.x;
                    break;
                case 2:
                    compareParameterThis = Cordinates.y;
                    compareParameterOther = other.Cordinates.y;
                    break;
            }
            if (compareParameterOther > compareParameterThis)
                return 1;
            if (compareParameterOther < compareParameterThis)
                return -1;

            return 0;
        }


        public bool FullComapre(City other) => Cordinates.x == other.Cordinates.x && Cordinates.y == other.Cordinates.y;

        public string ToString() => $"{Name} -> ({Cordinates.x},{Cordinates.y})";
    }
}
