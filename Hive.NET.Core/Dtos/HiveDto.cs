﻿using System;
using System.Collections.Generic;

namespace Hive.NET.Core.Api;

internal class HiveDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<BeeDto> Bees { get; set; }
    public bool Archived { get; set; }
}