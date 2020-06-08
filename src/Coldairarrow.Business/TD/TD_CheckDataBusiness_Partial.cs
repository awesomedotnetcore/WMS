using Coldairarrow.Entity.TD;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.TD
{
    public partial class TD_CheckDataBusiness
    {
        public async Task ClearDataAsync(string checkId)
        {
            await DeleteAsync(p => p.CheckId == checkId);
        }

        public async Task PushDataAsync(List<TD_CheckData> data)
        {
            await base.InsertAsync(data);
        }

        public async Task<PageResult<TDCheckDataDTO>> QueryDataListAsync(PageInput<TDCheckDataConditionDTO> input)
        {
            var q = GetIQueryable();
            q=q.Include(i => i.Storage)                
                .Include(i => i.Material)
                    .ThenInclude(i=>i.Measure)                
                .Include(i => i.Location)
                    .ThenInclude(i=>i.PB_Laneway)
                .Include(i => i.Location)
                    .ThenInclude(i => i.PB_Rack)                
                .Include(i => i.Location)
                    .ThenInclude(i => i.PB_Rack)
                .Include(i => i.Location)
                    .ThenInclude(i => i.PB_StorArea)
                .Include(i => i.Tray)
                .Include(i => i.Zone);

            var where = LinqHelper.True<TD_CheckData>();
            var search = input.Search;

            where = where.And(p => p.CheckId == search.CheckId);

            return await q.Where(where).Select(
                u => new TDCheckDataDTO()
                {
                    CheckId = u.CheckId,
                    CheckNum = u.CheckNum,
                    DisNum = u.DisNum,
                    Id = u.Id,
                    BarCode = u.Material.BarCode,
                    localId = u.localId,
                    MeasureId = u.Material.MeasureId,
                    StorId = u.StorId,
                    TrayId = u.TrayId,
                    ZoneId = u.ZoneId,
                    LocationCode = u.Location.Code,
                    LocationName = u.Location.Name,
                    LanewayName = u.Location.PB_Laneway.Name,
                    TrayName = u.Tray.Name,
                    ZoneName = u.Zone.Name,
                    LocalNum = u.LocalNum,
                    MaterialId = u.MaterialId,
                    MaterialName = u.Material.Name,
                    MaterialCode = u.Material.Code,
                    MeasureName = u.Material.Measure.Name,
                    BatchNo = u.BatchNo,
                    RackName = u.Location.PB_Rack.Name,
                    Remarks = u.Location.Remarks,
                    AreaName = u.Location.PB_StorArea.Name,
                })
                .GetPageResultAsync(input);
        }

        public async Task<List<TDCheckDataDTO>> QueryDataListAsync(string checkId)
        {
            var q = GetIQueryable();
            q = q.Include(i => i.Storage)
                .Include(i => i.Material)
                    .ThenInclude(i => i.Measure)
                .Include(i => i.Location)
                    .ThenInclude(i => i.PB_Laneway)
                .Include(i => i.Location)
                    .ThenInclude(i => i.PB_Rack)
                .Include(i => i.Location)
                    .ThenInclude(i => i.PB_Rack)
                .Include(i => i.Location)
                    .ThenInclude(i => i.PB_StorArea)
                .Include(i => i.Tray)
                .Include(i => i.Zone);

            var where = LinqHelper.True<TD_CheckData>();            
            where = where.And(p => p.CheckId == checkId);

            return await q.Where(where).Select(
                u => new TDCheckDataDTO()
                {
                    CheckId = u.CheckId,
                    CheckNum = u.CheckNum,
                    DisNum = u.DisNum,
                    Id = u.Id,
                    BarCode = u.Material.BarCode,
                    localId = u.localId,
                    MeasureId = u.Material.MeasureId,
                    StorId = u.StorId,
                    TrayId = u.TrayId,
                    ZoneId = u.ZoneId,
                    LocationCode = u.Location.Code,
                    LocationName = u.Location.Name,
                    LanewayName = u.Location.PB_Laneway.Name,
                    TrayName = u.Tray.Name,
                    ZoneName = u.Zone.Name,
                    LocalNum = u.LocalNum,
                    MaterialId = u.MaterialId,
                    MaterialName = u.Material.Name,
                    MaterialCode = u.Material.Code,
                    MeasureName = u.Material.Measure.Name,
                    BatchNo = u.BatchNo,
                    RackName = u.Location.PB_Rack.Name,
                    Remarks = u.Location.Remarks,
                    AreaName = u.Location.PB_StorArea.Name,
                })
                .ToListAsync();
        }

        public async Task<List<TDCheckDataDTO>> AllCheckDataListAsync(string checkId)
        {
            var q = GetIQueryable();
            q = q.Include(i => i.Storage)
                .Include(i => i.Material)
                    .ThenInclude(i => i.Measure)
                .Include(i => i.Location)
                    .ThenInclude(i => i.PB_Laneway)
                .Include(i => i.Location)
                    .ThenInclude(i => i.PB_Rack)
                .Include(i => i.Location)
                    .ThenInclude(i => i.PB_Rack)
                .Include(i => i.Location)
                    .ThenInclude(i => i.PB_StorArea)
                .Include(i => i.Tray)
                .Include(i => i.Zone);

            var where = LinqHelper.True<TD_CheckData>();
            where = where.And(p => p.CheckId == checkId);

            return await q.Where(where).Select(
                u => new TDCheckDataDTO()
                {
                    CheckId = u.CheckId,
                    CheckNum = u.CheckNum,
                    DisNum = u.DisNum,
                    Id = u.Id,
                    BarCode = u.Material.BarCode,
                    localId = u.localId,
                    MeasureId = u.Material.MeasureId,
                    StorId = u.StorId,
                    TrayId = u.TrayId,
                    ZoneId = u.ZoneId,
                    LocationCode = u.Location.Code,
                    LocationName = u.Location.Name,
                    LanewayName = u.Location.PB_Laneway.Name,
                    TrayName = u.Tray.Name,
                    ZoneName = u.Zone.Name,
                    LocalNum = u.LocalNum,
                    MaterialId = u.MaterialId,
                    MaterialName = u.Material.Name,
                    MaterialCode = u.Material.Code,
                    MeasureName = u.Material.Measure.Name,
                    BatchNo = u.BatchNo,
                    RackName = u.Location.PB_Rack.Name,
                    Remarks = u.Location.Remarks,
                    AreaName = u.Location.PB_StorArea.Name,
                }).ToListAsync();
        }

        public async Task ModifyCheckNumAsync(string userId, TDCheckNumModifyDTO data)
        {
            var entity = await GetEntityAsync(data.Id);
            entity.CheckNum = data.CheckNum;
            entity.CheckUserId = userId;
            if(entity.CheckNum!=null && entity.CheckNum>0)
            {
                entity.DisNum = entity.CheckNum - entity.LocalNum;
            }
            await UpdateAsync(entity);
        }

        public async Task<bool> AllCompletedAsync(string CheckId)
        {
            var q = GetIQueryable();
            var result = await q.Where(p => p.CheckId == CheckId && !p.CheckNum.HasValue).CountAsync();

            return result == 0;
        }
    }
}