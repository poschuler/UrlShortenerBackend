namespace UrlShortener.Api.Shared.Encoder
{
    public interface IBase62Converter
    {
        string Encode(long number);
    }
}
