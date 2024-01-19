using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hive.NET.Core.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hive.NET.Core.Components;

internal class Bee
{
    private readonly ILogger<Bee> _logger;
    internal List<BeeError> RegisteredErrors = new();
    public Guid Id { get;}
    public bool IsWorking { get; private set; }

    public Bee()
    {
        var options = ServiceLocator.GetService<IOptions<HiveSettings>>();
        _logger = BoostrapExtensions.BuildLogger<Bee>(options.Value.LogLevel);
        
        Id = Guid.NewGuid();
        IsWorking = false;
    }

    public async Task<bool> DoWork(BeeWorkItem unitOfWork, 
        BeeFinishedWorkCallback beeCallback)
    {
        try
        {
            IsWorking = true;

            unitOfWork.task.Start();
            await unitOfWork.task;

            IsWorking = false;

            beeCallback(this);
            //todo refactor to use internal logging to enable log level filtering
            _logger.LogDebug("{0:G}: bzzzt, nothing happened", DateTime.UtcNow);
            unitOfWork.onSuccess?.Invoke();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"Bee {Id} failed working on task {unitOfWork.Id} with exception: ({ex})");
            unitOfWork.onFailure?.Invoke(ex);
            RegisteredErrors.Add(new BeeError()
            {
                Id = Guid.NewGuid(),
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                WorkItemDescription = unitOfWork.Description,
                WorkItemId = unitOfWork.Id,
                OccuredAt = DateTime.UtcNow
            });
            
            return false;
        }
    }

    public delegate Task BeeFinishedWorkCallback(Bee bee);
}
