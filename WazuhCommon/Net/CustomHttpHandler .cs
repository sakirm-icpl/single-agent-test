namespace WazuhCommon.Net
{
    internal class CustomHttpHandler : DelegatingHandler
    {
        private readonly SemaphoreSlim throttler;

        public CustomHttpHandler(HttpMessageHandler innerHandler, int maxConcurrentRequests) : base(innerHandler)
        {
            if (innerHandler == null)
            {
                throw new ArgumentNullException(nameof(innerHandler));
            }

            throttler = new SemaphoreSlim(maxConcurrentRequests);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await throttler.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                throttler.Release();
            }
        }
    }
}
