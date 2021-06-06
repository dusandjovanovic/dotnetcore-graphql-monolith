namespace GraphQL.API.Messages
{
    public class CityAddedMessage
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string Message { get; set; }
    }
}