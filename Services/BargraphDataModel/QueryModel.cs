namespace Services.BargraphDataModel
{
    public class QueryModel: PropertyChangedHelper
    {
        public Content Content { get; set; } = new Content();
        public object Crc { get; set; } = 23;
    }
}