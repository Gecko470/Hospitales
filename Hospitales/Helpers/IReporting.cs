namespace Hospitales.Helpers
{
    public interface IReporting
    {
        byte[] Excel<T>(List<T> list, string[] nombrePropiedades);
        byte[] Pdf<T>(string titulo, string[] nombrePropiedades, List<T> list);
        byte[] Word<T>(string titulo, string[] nombrePropiedades, List<T> list);
    }
}
