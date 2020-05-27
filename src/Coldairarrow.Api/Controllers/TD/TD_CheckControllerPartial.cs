using Coldairarrow.Business.Base;
using Coldairarrow.Business.PB;
using Coldairarrow.Business.TD;
using Coldairarrow.Entity.PB;
using Coldairarrow.Entity.TD;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using org.apache.zookeeper.data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.TD
{
    public partial class TD_CheckController 
    {
        #region DI
        ITD_CheckBusiness _tD_CheckBus { get; }

        IBase_UserStorBusiness _base_UserStorBus { get; }

        ITD_CheckAreaBusiness _tD_CheckAreaBus { get; }

        ITD_CheckMaterialBusiness _tD_CheckMaterialBus { get; }

        ITD_CheckDataBusiness _tD_CheckDataBus { get; }

        IPB_AreaMaterialBusiness _pB_AreaMaterialBus { get; }
        IOperator _Op { get; }

        public TD_CheckController(ITD_CheckBusiness tD_CheckBus
            , IBase_UserStorBusiness base_UserStorBus
            , ITD_CheckAreaBusiness tD_CheckAreaBus
            , ITD_CheckMaterialBusiness tD_CheckMaterialBus
            , ITD_CheckDataBusiness tD_CheckDataBus
            , IPB_AreaMaterialBusiness pB_AreaMaterialBus
            , IOperator op)
        {
            _tD_CheckBus = tD_CheckBus;
            _base_UserStorBus = base_UserStorBus;
            _tD_CheckAreaBus = tD_CheckAreaBus;
            _tD_CheckMaterialBus = tD_CheckMaterialBus;
            _tD_CheckDataBus = tD_CheckDataBus;
            _pB_AreaMaterialBus = pB_AreaMaterialBus;
            _Op = op;
        }

        #endregion

        [HttpPost]
        public async Task PushData(TDCheckConditionDTO model)
        {
            var data = model.Data;
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);
                data.StorId = (await _base_UserStorBus.GetStorage(_Op.UserId)).Where(p => p.IsDefault == true).FirstOrDefault().Id;
                data.EquId = "1";
                data.IsComplete = false;
                data.Status = 0;

                await _tD_CheckBus.AddDataAsync(data);
            }
            else
            {
                await _tD_CheckBus.UpdateDataAsync(data);
                await _tD_CheckDataBus.ClearDataAsync(data.Id);
                await _tD_CheckMaterialBus.ClearDataAsync(data.Id);
                await _tD_CheckAreaBus.ClearDataAsync(data.Id);
            }

            if(data.Type=="Area")
            {
                var areaList = new List<TD_CheckArea>();
                var materList = new List<PB_AreaMaterial>();
                foreach (var id in model.Ids)
                {
                    TD_CheckArea area = new TD_CheckArea();
                    area.CherkId = data.Id;
                    area.StoarAreaId = id;

                    areaList.Add(area);

                    materList.AddRange(await _pB_AreaMaterialBus.GetDataListAsync(id));
                }
                await _tD_CheckAreaBus.PushAsync(areaList);

                var materialList = (from u in materList select new TD_CheckMaterial() { CheckId = data.Id, MaterialId = u.MaterialId }).Distinct().ToList();
                await _tD_CheckMaterialBus.PushAsync(materialList);
            }
        }
    }
}