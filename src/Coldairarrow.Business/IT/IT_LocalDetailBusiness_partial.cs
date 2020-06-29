using Coldairarrow.Entity.IT;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.IT
{
    public partial class IT_LocalDetailBusiness : BaseBusiness<IT_LocalDetail>, IIT_LocalDetailBusiness, ITransientDependency
    {
        public async Task AddDataAsync(List<IT_LocalDetail> list)
        {
            await InsertAsync(list);
        }

        public async Task UpdateDataAsync(List<IT_LocalDetail> list)
        {
            await UpdateAsync(list);
        }
        public async Task DeleteDataAsync(List<IT_LocalDetail> list)
        {
            await DeleteAsync(list);
        }

        public async Task<PageResult<IT_LocalDetail>> GetDataListAsync(IT_LocalDetailPageInput input)
        {
            var q = GetIQueryable()
                .Include(i => i.Location)
                .Include(i => i.Tray)
                .Include(i => i.TrayZone)
                .Include(i => i.Material)
                .Include(i => i.Measure);
            var where = LinqHelper.True<IT_LocalDetail>();
            where = where.And(w => w.StorId == input.StorId);
            var search = input.Search;
            if (!search.LocalName.IsNullOrEmpty())
                where = where.And(w => w.Location.Code.Contains(search.LocalName) || w.Location.Name.Contains(search.LocalName));
            if (!search.TrayName.IsNullOrEmpty())
                where = where.And(w => w.Tray.Code.Contains(search.TrayName) || w.Tray.Name.Contains(search.TrayName));
            if (!search.MaterialName.IsNullOrEmpty())
                where = where.And(w => w.Material.Code.Contains(search.MaterialName) || w.Material.Name.Contains(search.MaterialName));
            if (!search.Code.IsNullOrEmpty())
                where = where.And(w => w.BatchNo.Contains(search.Code) || w.BarCode.Contains(search.Code));

            if (search.InTimeStart.HasValue)
                where = where.And(w => w.InTime >= search.InTimeStart.Value);
            if (search.InTimeEnd.HasValue)
                where = where.And(w => w.InTime <= search.InTimeEnd.Value);


            return await q.Where(where).GetPageResultAsync(input);
        }


        public async Task UpdataDatasByBussiness(List<BusinessInfo> list, string userId)
        {
            if (list.Count > 0)
            {
                var q = GetIQueryable();
                var StorIdList = list.Select(i => i.StorId).Distinct().ToList();
                var LocalIdList = list.Select(i => i.LocalId).Distinct().ToList();
                var TrayIdList = list.Select(i => i.TrayId).Distinct().ToList();
                var ZoneIdList = list.Select(i => i.ZoneId).Distinct().ToList();
                var MaterialIdList = list.Select(i => i.MaterialId).Distinct().ToList();
                var BatchNoList = list.Select(i => i.BatchNo).Distinct().ToList();
                var BarCodeList = list.Select(i => i.BarCode).Distinct().ToList();
                //var hasMatch = 0;

                q = q.Where(w => StorIdList.Contains(w.StorId) && LocalIdList.Contains(w.LocalId) && TrayIdList.Contains(w.TrayId)
                && ZoneIdList.Contains(w.ZoneId) && MaterialIdList.Contains(w.MaterialId) && BatchNoList.Contains(w.BatchNo)
                && BarCodeList.Contains(w.BarCode));

                var datalist = await q.OrderBy(o => o.InTime).ToListAsync();
                var addDatas = new List<IT_LocalDetail>();
                var updataDatas = new List<IT_LocalDetail>();
                var deletDatas = new List<string>();

                foreach (var b in list)
                {
                    if (b.ActionType == (int)ActionTypeEnum.出库)
                    {
                        foreach (var i in datalist)
                        {
                            if (b.StorId == i.StorId && b.LocalId == i.LocalId && b.TrayId == i.TrayId && b.ZoneId == i.ZoneId
                             && b.MaterialId == i.MaterialId && b.BatchNo == i.BatchNo && b.BarCode == i.BarCode && i.Num > 0)
                            {
                                if (i.Num > b.Num)
                                {
                                    i.Num -= b.Num;
                                    updataDatas.Add(i);
                                    break;
                                }
                                else if (i.Num == b.Num)
                                {
                                    i.Num = 0;
                                    deletDatas.Add(i.Id);
                                    break;
                                }
                                else
                                {
                                    i.Num = 0;
                                    deletDatas.Add(i.Id);
                                    b.Num -= i.Num.Value;
                                }
                            }
                        }
                    }
                    else
                    {
                        addDatas.Add(new IT_LocalDetail()
                        {
                            Id = IdHelper.GetId(),
                            StorId = b.StorId,
                            LocalId = b.LocalId,
                            TrayId = b.TrayId,
                            ZoneId = b.ZoneId,
                            MaterialId = b.MaterialId,
                            BarCode = b.BarCode,
                            BatchNo = b.BatchNo,
                            Num = b.Num,
                            MeasureId = b.MeasureId,
                            InTime = DateTime.Now,
                            CreateTime = DateTime.Now,
                            CreatorId = userId
                        });
                    }
                }

                if (updataDatas.Count > 0)
                {
                    await UpdateAsync(updataDatas);
                }
                if (addDatas.Count > 0)
                {
                    await InsertAsync(addDatas);
                }
                if (deletDatas.Count > 0)
                {
                    await DeleteDataAsync(deletDatas);
                }
            }
        }
    }
}