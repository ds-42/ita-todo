namespace Common.Application.Dto;

public class Pagination : BaseFilter
{
    public int? Offset { get; set; }
    public int? Limit { get; set; }
}
