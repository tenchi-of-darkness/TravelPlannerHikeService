﻿using Microsoft.AspNetCore.Mvc;

namespace Hike.API.Models.Requests.Trail;

public record GetTrailsRequest([FromQuery] string? SearchValue, [FromQuery] int Page=1, [FromQuery] int PageSize=10);