using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cubipool.Common.Exceptions;
using Cubipool.Entity;
using Cubipool.Repository.Context;
using Cubipool.Repository.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Cubipool.Repository.Implementations
{
    public class ConstantsRepository : IConstantsRepository

    {
        private readonly EFDbContext _context;

        public ConstantsRepository(EFDbContext context)
        {
            this._context = context;
        }
        public async Task<Constant> FindOneByIdAsync(int id)
        {
            return await _context
                .Constants
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}