using System.Collections.Generic; // Add necessary using directives

public class PaginatedResponse<T>
{
    public int TotalItems { get; set; }
    public List<T> Items { get; set; }
}