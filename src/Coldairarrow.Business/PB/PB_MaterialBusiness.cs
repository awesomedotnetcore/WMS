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
    public partial class PB_MaterialBusiness : BaseBusiness<PB_Material>, IPB_MaterialBusiness, ITransientDependency
    {
        public PB_MaterialBusiness(IDbAccessor db)
            : base(db)
        {
        }

        #region 外部接口

        public async Task<PageResult<PB_Material>> GetDataListAsync(PageInput<ConditionDTO> input)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<PB_Material>();
            var search = input.Search;

            //筛选
            if (!search.Condition.IsNullOrEmpty() && !search.Keyword.IsNullOrEmpty())
            {
                var newWhere = DynamicExpressionParser.ParseLambda<PB_Material, bool>(
                    ParsingConfig.Default, false, $@"{search.Condition}.Contains(@0)", search.Keyword);
                where = where.And(newWhere);
            }

            return await q.Where(where).GetPageResultAsync(input);
        }

        public async Task<List<PB_Material>> GetDataListAsync()
        {
            var q = GetIQueryable();

            return await q.ToListAsync();
        }

        

        [DataAddLog(UserLogType.物料管理, "Name", "物料")]
        [DataRepeatValidate(new string[] { "Code", "Name" }, new string[] { "编号", "名称" })]
        public async Task AddDataAsync(PB_Material data)
        {
            await InsertAsync(data);
        }

        [DataEditLog(UserLogType.物料管理, "Name", "物料")]
        [DataRepeatValidate(new string[] { "Code", "Name" }, new string[] { "编号", "名称" })]
        public async Task UpdateDataAsync(PB_Material data)
        {
            await UpdateAsync(data);
        }

        [DataDeleteLog(UserLogType.物料管理, "Name", "物料")]
        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        public async Task AddDataExlAsync(List<PB_Material> list)
        {
            await InsertAsync(list);   

        }

        public IQueryable<T> GetQueryable<T>() where T : class, new()
        {
            return Db.GetIQueryable<T>();
        }
        #endregion

        #region 私有成员

        #endregion
    }
}