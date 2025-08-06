namespace AppServices.Abstract
{
    public interface IEncrypthonService
    {
        string AesDecrypthon(string text);
        string AesEncrypthon(string text);
    }
}
