using System;
using Newtonsoft.Json;

namespace Common.Info {
    public class PageInfo {

        [JsonProperty("totalSize")]
        public int TotalSize { get; private set; }

        [JsonProperty("pageNumber")]
        public int PageNumber { get; private set; }

        [JsonProperty("totalPages")]
        public int TotalPages { get; private set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; private set; }

        public PageInfo(int totalSize, int pageNumber, int pageSize) {
            TotalSize = totalSize;
            PageSize = pageSize;
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(TotalSize / (double)pageSize);
        }

        //[JsonProperty("hasPreviousPage")]
        public bool HasPreviousPage {
            get {
                return (PageNumber > 1);
            }
        }

        //[JsonProperty("hasPreviousPage")]
        public bool HasNextPage {
            get {
                return (PageNumber < PageSize);
            }
        }
    }
}
