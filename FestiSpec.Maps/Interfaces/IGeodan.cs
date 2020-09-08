namespace FestiSpec.Maps.Interfaces
{
    public interface IGeodan
    {
        void ShowAdresses(int zoom, double x, double y);
        string FindAdress(string query);
        dynamic FindRoute(string query);
    }
}
