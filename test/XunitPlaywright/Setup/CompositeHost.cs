using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XunitPlaywright.Setup;

public class CompositeHost : IHost
{
    private readonly IHost testHost;
    private readonly IHost kestrelHost;
    public CompositeHost(IHost testHost, IHost kestrelHost)
    {
        this.testHost = testHost;
        this.kestrelHost = kestrelHost;
    }
    public IServiceProvider Services => this.testHost.Services;
    public void Dispose()
    {
        this.testHost.Dispose();
        this.kestrelHost.Dispose();
    }
    public async Task StartAsync(
      CancellationToken cancellationToken = default)
    {
        await this.testHost.StartAsync(cancellationToken);
        await this.kestrelHost.StartAsync(cancellationToken);
    }
    public async Task StopAsync(
      CancellationToken cancellationToken = default)
    {
        await this.testHost.StopAsync(cancellationToken);
        await this.kestrelHost.StopAsync(cancellationToken);
    }
}
