using System.Collections.Generic;
using System.Linq;
using Common.Info;
using Newtonsoft.Json;

namespace Common.Info {
    public class PaginatedList<T>
        where T : class, new() {

        [JsonProperty("elements")]
        public IList<T> Elements { get; set; }

        [JsonProperty("pageInfo")]
        public PageInfo PageInfo { get; set; }

        public PaginatedList(IQueryable<T> source,
            int pageNumber, int pageSize) {
            this.CreateItemsAsync(source, pageNumber, pageSize);
        }

        public PaginatedList() { }

        //get the total items of query
        public void CreateItemsAsync(
            IQueryable<T> source, int pageNumber, int pageSize) {

            var count = source.Count();
            var items = source.Skip(
                (pageNumber - 1) * pageSize)
                .Take(pageSize).ToList();

            this.Elements = items ?? new List<T>();

            this.PageInfo = new PageInfo(count, pageNumber, pageSize);
        }
    }
}
