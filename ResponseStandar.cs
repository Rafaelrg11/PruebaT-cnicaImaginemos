namespace PruebaTécnicaImaginemos.ApiView;

public class ResponseStandar<T>
{
    public T data { get; set; }
    public long total { get; set; }
}
