using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Movies.Client
{
    /// <inheritdoc />
    public class TimeoutDelegatingHandler : DelegatingHandler
    {
        private readonly TimeSpan _timeOut;

        public TimeoutDelegatingHandler(TimeSpan timeout)
        {
            _timeOut = timeout;
        }

        public TimeoutDelegatingHandler(HttpMessageHandler innerHandler, TimeSpan timeout)
            : base(innerHandler)
        {
            _timeOut = timeout;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using var linkedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            linkedCancellationTokenSource.CancelAfter(_timeOut);
            try
            {
                return await base.SendAsync(request, linkedCancellationTokenSource.Token);
            }
            catch (OperationCanceledException canceledException)
            {
                if (Equals(!cancellationToken.IsCancellationRequested))
                {
                    throw new TimeoutException("The request timed out", canceledException);
                }
                throw;
            }
        }
    }
}