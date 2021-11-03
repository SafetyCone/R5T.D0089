using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;

using R5T.D0088;
using R5T.T0064;


namespace R5T.D0089
{
    /// <summary>
    /// Startup for a web host.
    /// </summary>
    [ServiceDefinitionMarker]
    public interface IWebHostStartup : IHostStartup
    {
        Task ConfigureApplication(IApplicationBuilder applicationBuilder);
    }
}
