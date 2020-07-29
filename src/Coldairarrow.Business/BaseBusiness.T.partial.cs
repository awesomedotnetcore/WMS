using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Coldairarrow.Business
{
    public abstract partial class BaseBusiness<T>
    {
        public async Task<T> GetByCode(string code)
        {
            return await this.Db.GetIQueryable<T>().Where("Code == @0", code).FirstOrDefaultAsync();
        }
    }
}
