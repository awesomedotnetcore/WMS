using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial class PB_BarCodeTypeBusiness : BaseBusiness<PB_BarCodeType>, IPB_BarCodeTypeBusiness, ITransientDependency
    {
        readonly IServiceProvider _serviceProvider;
        public PB_BarCodeTypeBusiness(IDbAccessor db,IServiceProvider serviceProvider)
            : base(db)
        {
            _serviceProvider = serviceProvider;
        }

        #region 外部接口

        public async Task<PageResult<PB_BarCodeType>> GetDataListAsync(PageInput<PB_BarCodeTypeQM> input)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<PB_BarCodeType>();
            var search = input.Search;

            ////筛选
            //if (!search.Condition.IsNullOrEmpty() && !search.Keyword.IsNullOrEmpty())
            //{
            //    var newWhere = DynamicExpressionParser.ParseLambda<PB_BarCodeType, bool>(
            //        ParsingConfig.Default, false, $@"{search.Condition}.Contains(@0)", search.Keyword);
            //    where = where.And(newWhere);
            //}
            if (!search.Keyword.IsNullOrEmpty())
                where = where.And(w => w.Name.Contains(search.Keyword) || w.Code.Contains(search.Keyword));

            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<PB_BarCodeType> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        [DataAddLog(UserLogType.条码类型, "Name", "条码类型")]
        [DataRepeatValidate(new string[] { "Code" }, new string[] { "条码类型" }, allData: false, matchOr: false)]
        public async Task AddDataAsync(PB_BarCodeType data)
        {
            await InsertAsync(data);
        }

        [DataEditLog(UserLogType.条码类型, "Name", "条码类型")]
        [DataRepeatValidate(new string[] { "Code" }, new string[] { "条码类型" }, allData: false, matchOr: false)]
        public async Task UpdateDataAsync(PB_BarCodeType data)
        {
            await UpdateAsync(data);
        }

        [DataDeleteLog(UserLogType.条码类型, "Name", "条码类型")]
        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        #endregion

        #region 私有成员

        #endregion
    }
}