using System;
using System.Collections.Generic;
using Hive.NET.Core.Api;
using Hive.NET.Core.Components;

namespace Hive.NET.Core.Manager;

public interface IHiveManager
{
    public void AddHive(Guid id, Components.Hive? hive);
    public Components.Hive? GetHive(Guid id);
    public Components.Hive? GetHive(string name);
    
    //Internals
    internal List<HiveDto> GetHives();
    internal HiveDetailsDto GetHiveDetails(Guid id);
    internal List<BeeErrorDto> GetHiveRegisteredErrors(Guid id);
}