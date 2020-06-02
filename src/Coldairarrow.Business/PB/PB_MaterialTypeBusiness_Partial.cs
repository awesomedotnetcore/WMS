using Coldairarrow.Entity.PB;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Quartz.Util;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial class PB_MaterialTypeBusiness 
    {
        public async Task<List<MaterialTypeTreeDTO>> GetTreeDataListAsync(string parentId = null)
        {
            var where = LinqHelper.True<PB_MaterialType>();
            if (!parentId.IsNullOrEmpty())
                where = where.And(x => x.ParentId == parentId);


            var list = await GetIQueryable().Where(where).ToListAsync();
            var treeList = list
                .Select(x => new MaterialTypeTreeDTO
                {
                    Id = x.Id,
                    ParentId = x.ParentId,
                    Text = x.Name,
                    Value = x.Id,
                    Code = x.Code,
                    Remark = x.Remarks,
                }).ToList();

            return TreeHelper.BuildTree(treeList);
        }

        public async Task<List<string>> GetChildrenIdsAsync(string typeId)
        {
            var allNode = await GetIQueryable().Select(x => new MaterialTypeTreeDTO
            {
                Id = x.Id,
                ParentId = x.ParentId,
                Text = x.Name,
                Value = x.Id,
                Code = x.Code,
                Remark = x.Remarks,
            }).ToListAsync();

            var children = TreeHelper
                .GetChildren(allNode, allNode.Where(x => x.Id == typeId).FirstOrDefault(), true)
                .Select(x => x.Id)
                .ToList();

            return children;
        }

        private async Task SetPathAsync(PB_MaterialType data)
        {
            if (data.ParentId.IsNullOrWhiteSpace())
            {
                data.Path = string.Format("{0}", data.Id);
            }
            else
            {
                var parent = await GetIQueryable().Where(p => p.Id == data.ParentId).FirstOrDefaultAsync();
                data.Path = string.Format("{0}/{1}", parent.Path, data.Id);

                if(parent.IsLeaf)
                {
                    parent.IsLeaf = false;
                    await UpdateDataAsync(parent);
                }
            }
        }

        private async Task ModifyRefDataAsync(PB_MaterialType data)
        {
            var old = await GetEntityAsync(data.Id);
            if(!string.IsNullOrWhiteSpace(old.ParentId) 
                && old.ParentId!=data.ParentId)
            {
                var childList= await GetIQueryable().Where(p => p.ParentId == data.Id).ToListAsync();
                foreach(var child in childList)
                {
                    child.Path = child.Path.Replace(old.Path, data.Path);
                }

                await UpdateAsync(childList);

                var pcCount = await GetIQueryable().Where(p => p.ParentId == old.ParentId).CountAsync();
                if(pcCount<=1)
                {
                    var oldParent = await GetEntityAsync(old.ParentId);
                    oldParent.IsLeaf = true;

                    await UpdateAsync(oldParent);
                }
            }
        }
    }
}