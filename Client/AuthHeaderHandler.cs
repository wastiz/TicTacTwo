using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly IAccessTokenProvider _tokenProvider;

    public AuthHeaderHandler(IAccessTokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (!request.RequestUri.AbsolutePath.Contains("/auth/") &&
            !request.RequestUri.AbsolutePath.Contains("/login"))
        {
            var tokenResult = await _tokenProvider.RequestAccessToken();
            
            if (tokenResult.TryGetToken(out var token))
            {
                request.Headers.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token.Value);
            }
        }
        
        return await base.SendAsync(request, cancellationToken);
    }
}