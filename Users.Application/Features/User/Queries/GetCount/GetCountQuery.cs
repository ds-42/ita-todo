﻿using Common.Application.Dto;
using MediatR;

namespace Users.Application.Features.User.Queries.GetCount;

public class GetCountQuery : BaseFilter, IRequest<int>
{
}
