namespace HwaidakAPI.Helpers
{
    public class PeriodicRequestService : IHostedService, IDisposable
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private Timer _timer;

        public PeriodicRequestService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(4));
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                // Replace with your target URL
                var requestUrl = "https://api.hwaidakhotels.com/health";
                var response = await httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    // Process the response here
                }
                else
                {
                    // Handle the error response here
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
