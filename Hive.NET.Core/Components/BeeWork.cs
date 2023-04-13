namespace Hive.NET.Core.Components;

public record BeeWork(Task task, Action onSuccess = default, Action<Exception> onFailure = default);
