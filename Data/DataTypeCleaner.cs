namespace Dataedo_RazorPages.Data
{
    internal class DataTypeCleaner
    {
        public IList<string> DataTypes = new List<string>();

        public DataTypeCleaner(string dataTypesFile)
        {
            var streamReader = new StreamReader(dataTypesFile);

            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                var dataTypes = line.Split(';');
                for (int i = 0; i < dataTypes.Length; ++i)
                {
                    if (dataTypes[i] != String.Empty)
                    {
                        DataTypes.Add(dataTypes[i]);
                    }
                }
            }
        }

        public string ExtraCleaning(string dataType)
        {
            if (DataTypes.Where(dt => dt == dataType).FirstOrDefault() == null)
            {
                var dataTypes = DataTypes.OrderByDescending(dt => dt.Length).ToArray();

                for (int i = 0; i < dataTypes.Length; ++i)
                {
                    if (dataType.ToLower().Contains(dataTypes[i]))
                    {
                        return dataTypes[i];
                    }
                }
            }
            return dataType;
        }
    }
}
