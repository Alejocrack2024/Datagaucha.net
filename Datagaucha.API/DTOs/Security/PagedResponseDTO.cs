public class PagedResponseDTO<T>
{
    public int code { get; set; }
    public string message { get; set; } = "";
    public bool success { get; set; }

    public int currentPage { get; set; }
    public int pageSize { get; set; }
    public int totalCount { get; set; }
    public int totalPages { get; set; }

    public T? payload { get; set; }
}