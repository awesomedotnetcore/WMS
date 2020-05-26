using Coldairarrow.Business.Base;
using Coldairarrow.Business.TD;
using Coldairarrow.Entity.TD;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
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
        IOperator _Op { get; }

        public TD_CheckController(ITD_CheckBusiness tD_CheckBus
            , IBase_UserStorBusiness base_UserStorBus
            , ITD_CheckAreaBusiness tD_CheckAreaBus
            , ITD_CheckMaterialBusiness tD_CheckMaterialBus
            , ITD_CheckDataBusiness tD_CheckDataBus
            , IOperator op)
        {
            _tD_CheckBus = tD_CheckBus;
            _base_UserStorBus = base_UserStorBus;
            _tD_CheckAreaBus = tD_CheckAreaBus;
            _tD_CheckMaterialBus = tD_CheckMaterialBus;
            _tD_CheckDataBus = tD_CheckDataBus;
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
                foreach(var id in model.Ids)
                {
                    TD_CheckArea area = new TD_CheckArea();
                    area.CherkId = data.Id;
                    area.StoarAreaId = id;

                    areaList.Add(area);
                }
                await _tD_CheckAreaBus.PushAsync(areaList);
            }
        }
    }
}