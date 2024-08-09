using BookStore.Model;
using Nest;

namespace BookStore.Services
{
    public class SearchService
    {
        private readonly IElasticClient _elasticClient;

        public SearchService(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string query)
        {
            var response = await _elasticClient.SearchAsync<Book>(s => s
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Title)
                        .Query(query)
                    )
                )
            );

            return response.Documents;
        }
    }
}
