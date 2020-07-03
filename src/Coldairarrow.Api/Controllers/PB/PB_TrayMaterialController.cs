using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    [Route("/PB/[controller]/[action]")]
    public class PB_TrayMaterialController : BaseApiController
    {
        #region DI

        public PB_TrayMaterialController(IPB_TrayMaterialBusiness pB_TrayMaterialBus)
        {
            _pB_TrayMaterialBus = pB_TrayMaterialBus;
        }

        IPB_TrayMaterialBusiness _pB_TrayMaterialBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_TrayMaterial>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _pB_TrayMaterialBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<List<PB_TrayMaterial>> GetDataListByTypeId(string typeId)
        {
            return await _pB_TrayMaterialBus.GetDataListAsync(typeId);
        }

        [HttpPost]
        public async Task<PB_TrayMaterial> GetTheData(IdInputDTO input)
        {
            return await _pB_TrayMaterialBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_TrayMaterial data)
        {
            //if (data.Id.IsNullOrEmpty())
            //{
            //    InitEntity(data);

            //    await _pB_TrayMaterialBus.AddDataAsync(data);
            //}
            //else
            //{
            //    await _pB_TrayMaterialBus.UpdateDataAsync(data);
            //}
            await _pB_TrayMaterialBus.AddDataAsync(data);
        }
        
        [HttpPost]
        public async Task SaveDatas(PBTrayMateriaConditionDTO data)
        {           
            await _pB_TrayMaterialBus.AddDataAsync(data);

        }
        //public async Task SaveDatas(string typeId, List<string> targetKeys)
        //{
        //    var list = await _pB_TrayMaterialBus.GetDataListAsync(typeId);
        //    var addList = new List<PB_TrayMaterial>();

        //    foreach (var i in targetKeys)
        //    {
        //        foreach(var s in list)
        //        {
        //            if(i == s.MaterialId)
        //            {
        //                list.Remove(s);
        //            }
        //        }
        //        addList.Add(new PB_TrayMaterial() { 
        //            TrayTypeId = typeId,
        //            MaterialId = i
        //        });
        //    }
        //    var deleteList = list.Select(s => s.MaterialId).ToList();

        //    await _pB_TrayMaterialBus.AddDataAsync(addList);
        //    //await _pB_TrayMaterialBus.DeleteDataAsync(typeId, deleteList);
        //}

        [HttpPost]
        public async Task DeleteData(string trayTypeId, List<string> materialIds)
        {
            await _pB_TrayMaterialBus.DeleteDataAsync(trayTypeId, materialIds);
        }

        #endregion
    }
}