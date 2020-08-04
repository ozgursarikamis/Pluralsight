namespace PersonDataReader.CSV.Tests
{
    public class FakeFileLoader : ICSVFileLoader
    {
        private readonly string _dataType;

        public FakeFileLoader(string dataType)
        {
            _dataType = dataType;
        }

        public string LoadFile()
        {
            return _dataType switch
            {
                "Good" => TestData.WithGoodRecords,
                "Mixed" => TestData.WithGoodAndBadRecords,
                "Bad" => TestData.WithOnlyBadRecords,
                "Empty" => string.Empty,
                _ => TestData.WithGoodRecords
            };
        }
    }
}