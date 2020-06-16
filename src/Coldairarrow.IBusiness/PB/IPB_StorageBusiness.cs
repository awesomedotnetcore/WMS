using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public interface IPB_StorageBusiness
    {
        Task<PageResult<PB_Storage>> GetDataListAsync(PB_StoragePageInput input);
        Task<PB_Storage> GetTheDataAsync(string id);
        Task AddDataAsync(PB_Storage data);
        Task UpdateDataAsync(PB_Storage data);
        Task ModifyDefaultAsync(PB_Storage data);
        Task DeleteDataAsync(List<string> ids);
        Task<List<PB_Storage>> QueryStorageDataAsync();

        Task<List<PB_Storage>> GetListAsync();

        Task ModifyIsTrayAsync(string id);
        Task ModifyIsZoneAsync(string id);
        Task ModifyDisableAsync(string id);

       // IQueryable<T> GetQueryable<T>() where T : class, new();
        //Task<List<PB_Storage>> QueryStorIDAsync(string code);
    }
    public class PB_StorageQM
    {
        //public string Name { get; set; }
        //public string Code { get; set; }
        public string Keyword { get; set; }
    }
    public class PB_StoragePageInput : PageInput<PB_StorageQM>
    {
        public string Id { get; set; }
    }

    [Map(typeof(PB_Storage))]
    public class PB_StorageDTO
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public String Id { get; set; }

        /// <summary>
        /// 仓库编号
        /// </summary>
        public String Code { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 默认
        /// </summary>
        public Boolean IsDefault { get; set; }
    }
}