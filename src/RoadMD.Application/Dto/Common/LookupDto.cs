namespace RoadMD.Application.Dto.Common
{
    [Serializable]
    public class LookupDto
    {
        public LookupDto(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public LookupDto(Guid key, string value)
        {
            Key = key.ToString();
            Value = value;
        }

        public LookupDto(int key, string value)
        {
            Key = key.ToString();
            Value = value;
        }

        public string Key { get; }
        public string Value { get; }
    }
}