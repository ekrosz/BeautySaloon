﻿namespace WebApplication.Wrappers;

public interface IHttpClientWrapper
{
    Task<bool> SendAsync(Func<string, Task> call);

    Task<T?> SendAsync<T>(Func<string, Task<T>> call);
}
