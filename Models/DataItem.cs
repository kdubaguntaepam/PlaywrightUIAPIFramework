namespace Models;
using System;
public class DataItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateDataItemRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
