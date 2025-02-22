using Blocked_CountryCore.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blocked_CountryServices.Services
{
    public class BackGroundServices : IHostedService,IDisposable
    {
        private Timer? _timer = null;
        private readonly ICachingServices _cachingServices;

        public BackGroundServices(ICachingServices cachingServices)
        {
            _cachingServices = cachingServices;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
          

            _timer = new Timer(RemoveTemporalBlockedCountries, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(2));

            return Task.CompletedTask;
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public  void RemoveTemporalBlockedCountries(object? state) 
        {
           _cachingServices.RemoveTemporalBlockedCountries();

        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}
