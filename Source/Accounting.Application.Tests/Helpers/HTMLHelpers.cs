using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Html.Dom;
using AngleSharp.Io;

#nullable disable
namespace Accounting.Application.Tests.Helpers
{
    /// <summary>
    /// Retrieve an HTML document as an AngleSharp IHTMLDocument.
    /// 
    /// Combines with <see cref="HttpClientExtensions.SendAsync(HttpClient, IHtmlFormElement, IHtmlElement, System.Collections.Generic.IEnumerable{System.Collections.Generic.KeyValuePair{string, string}})"/> to help post a form while handling the CSRF check.
    /// </summary>
    /// <remarks>
    /// MIT License.
    /// Copied from <see href="https://github.com/dotnet/AspNetCore.Docs/tree/main/aspnetcore/test/integration-tests/samples/3.x/IntegrationTestsSample/tests/RazorPagesProject.Tests/Helpers"/>
    /// </remarks>
    /// <seealso href="https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-5.0"/> 
    public static class HtmlHelpers
    {
        public static async Task<IHtmlDocument> GetDocumentAsync(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var document = await BrowsingContext.New()
                .OpenAsync(ResponseFactory, CancellationToken.None);
            return (IHtmlDocument)document;

            void ResponseFactory(VirtualResponse htmlResponse)
            {
                htmlResponse
                    .Address(response.RequestMessage.RequestUri)
                    .Status(response.StatusCode);

                MapHeaders(response.Headers);
                MapHeaders(response.Content.Headers);

                htmlResponse.Content(content);

                void MapHeaders(HttpHeaders headers)
                {
                    foreach (var header in headers)
                    {
                        foreach (var value in header.Value)
                        {
                            htmlResponse.Header(header.Key, value);
                        }
                    }
                }
            }
        }
    }
}
#nullable enable
