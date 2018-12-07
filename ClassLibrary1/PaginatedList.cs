using Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Service {
    public class PaginatedList<TOne, TTwo> where TOne : Entity, new()
                where TTwo : DTO, new() {

        private readonly IMapper _mapper;

        public PaginatedList(IMapper mapper) {
            _mapper = mapper
                ?? throw new ArgumentNullException("mapper");
        }

        //get the total items of query
        public async Task<PageInfo<TTwo>> CreateItemsAsync(
            IQueryable<TOne> source, int pageNumber, int pageSize) {

            var count = await source.CountAsync();
            var items = await source.Skip(
                (pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            var itemsToUserDTO = _mapper.Map<List<TTwo>>(items);
            return new PageInfo<TTwo>(itemsToUserDTO, count, pageNumber, pageSize);
        }
    }

    public class PageInfo<T> {

        public int TotalSize { get; private set; }

        public int PageNumber { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public List<T> Items { get; set; }


        public PageInfo(List<T> items, int totalSize, int pageNumber, int pageSize) {
            TotalSize = totalSize;
            PageSize = pageSize;
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(TotalSize / (double)pageSize);
            Items = items;
        }

        public bool HasPreviousPage {
            get {
                return (PageNumber > 1);
            }
        }

        public bool HasNextPage {
            get {
                return (PageNumber < PageSize);
            }
        }
    }
}
