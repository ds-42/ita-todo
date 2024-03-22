using Users.Services.Dto;

namespace Users.Services.Query.GetList;

public class GetListQuery : BaseUsersFilter
{
    public int? offset { get; set; }
    public int? limit { get; set; }
}
