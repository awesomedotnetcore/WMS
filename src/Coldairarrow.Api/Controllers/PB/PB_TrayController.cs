using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.IBusiness;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Quartz.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    [Route("/PB/[controller]/[action]")]
    public class PB_TrayController : BaseApiController
    {
        #region DI

        public PB_TrayController(IPB_TrayBusiness pB_TrayBus, IOperator op, IServiceProvider provider)
        {
            _pB_TrayBus = pB_TrayBus;
            _Op = op;
            _provider = provider;
        }

        IPB_TrayBusiness _pB_TrayBus { get; }

        IOperator _Op { get; }

        IServiceProvider _provider { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_Tray>> GetDataList(PageInput<PB_TrayQM> input)
        {
            return await _pB_TrayBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<PB_Tray> GetTheData(IdInputDTO input)
        {
            return await _pB_TrayBus.GetTheDataAsync(input.id);
        }


        [HttpPost]
        public async Task EnableTheData(IdInputDTO input)
        {
            await _pB_TrayBus.EnableTheData(input.id);
        }


        [HttpPost]
        public async Task DisableTheData(IdInputDTO input)
        {
            await _pB_TrayBus.DisableTheData(input.id);
        }

        [HttpPost]
        public async Task<List<PB_Tray>> GetQueryData(TraySelectQueryDTO search)
        {
            return await _pB_TrayBus.GetQueryData(search);
        }
        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_Tray data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);
                if (data.Code.IsNullOrWhiteSpace())
                {
                    var typeSvc = this._provider.GetRequiredService<IPB_TrayTypeBusiness>();
                    var type = await typeSvc.GetByTypeCode(data.TrayTypeId);
                    var codeSvc = _provider.GetRequiredService<IPB_BarCodeTypeBusiness>();
                    var dic = new Dictionary<string, string>();
                    dic.Add("TypeCode", type.Code);
                    data.Code = await codeSvc.Generate("PB_Tray", dic);
                }
                await _pB_TrayBus.AddDataAsync(data);
            }
            else
            {
                await _pB_TrayBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _pB_TrayBus.DeleteDataAsync(ids);
        }

        /// <summary>
        /// 托盘信息数据导入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [NoCheckJWT]
        public async Task<AjaxResult> Import(IFormFile file)// file
        {
            string ReturnValue = string.Empty;
            //定义一个bool类型的变量用来做验证
            bool flag = true;
            try
            {
                #region 检查文件
                string fileExt = Path.GetExtension(file.FileName).ToLower();
                //定义一个集合一会儿将数据存储进来,全部一次丢到数据库中保存
                var Data = new List<PB_Tray>();
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                ms.Seek(0, SeekOrigin.Begin);
                IWorkbook book;
                if (fileExt == ".xlsx")
                {
                    book = new XSSFWorkbook(ms);
                }
                else if (fileExt == ".xls")
                {
                    book = new HSSFWorkbook(ms);
                }
                else
                {
                    book = null;
                }
                ISheet sheet = book.GetSheetAt(0);

                int CountRow = sheet.LastRowNum + 1;//获取总行数

                if (CountRow - 1 == 0)
                {
                    return Error("Excel列表数据项为空!");

                }
                #endregion
                #region 循环验证
                for (int i = 1; i < CountRow; i++)
                {
                    //获取第i行的数据
                    var row = sheet.GetRow(i);
                    if (row != null)
                    {
                        //循环的验证单元格中的数据
                        for (int j = 0; j < 6; j++)
                        {
                            if ((j == 4 || j == 5) || (row.GetCell(j) == null || row.GetCell(j).ToString().Trim().Length == 0))
                            {
                                //return Error(ReturnValue += $"注意第{i + 1}行,第{j + 1}列数据为空。");
                            }
                            else
                            if (row.GetCell(j) == null || row.GetCell(j).ToString().Trim().Length == 0)
                            {
                                flag = false;
                                return Error(ReturnValue += $"第{i + 1}行,第{j + 1}列数据不能为空。");
                            }
                        }
                    }
                }
                #endregion
                if (flag)
                {
                    for (int i = 1; i < CountRow; i++)//
                    {
                        //实例化实体对象
                        PB_Tray commodity = new PB_Tray();
                        var row = sheet.GetRow(i);
                        if (row.GetCell(0) != null && row.GetCell(0).ToString().Trim().Length > 0)
                        {
                            commodity.Id = IdHelper.GetId();
                            commodity.CreatorId = _Op.UserId; //"Admin";//_Op.UserId;
                            commodity.Status = 1;//导入默认启用
                            commodity.StartTime = Convert.ToDateTime(DateTime.Now.ToString());  //默认当前日期时间

                            commodity.Code = row.GetCell(0).ToString();

                        }
                        if (row.GetCell(1) != null && row.GetCell(1).ToString().Trim().Length > 0)
                        {
                            commodity.Name = row.GetCell(1).ToString();
                        }
                        if (row.GetCell(2) != null && row.GetCell(2).ToString().Trim().Length > 0)
                        {
                            commodity.LocalId = row.GetCell(2).ToString();
                        }
                        if (row.GetCell(3) != null && row.GetCell(3).ToString().Trim().Length > 0)
                        {
                            commodity.TrayTypeId = row.GetCell(3).ToString();
                        }
                        Data.Add(commodity);
                    }
                    var listLocalCodes = Data.Select(s => s.LocalId).Distinct().ToList();
                    //s => .Select(s.Trim())
                    var dicLocal = _pB_TrayBus.GetQueryable<PB_Location>().Where(w => listLocalCodes.Contains(w.Code)).ToDictionary(k => k.Code, v => v.Id);

                    var listTrayTypeCodes = Data.Select(s => s.TrayTypeId).Select(s => s.Trim()).Distinct().ToList();
                    var dicTrayType = _pB_TrayBus.GetQueryable<PB_TrayType>().Where(w => listTrayTypeCodes.Contains(w.Code)).ToDictionary(k => k.Code, v => v.Id);


                    foreach (var item in Data)
                    {
                        if (dicTrayType.ContainsKey(item.TrayTypeId.Trim()))
                            item.TrayTypeId = dicTrayType[item.TrayTypeId.Trim()];
                        else
                            throw new Exception("托盘类型编号不存在！");

                        if (item.LocalId == null)
                        {
                            item.LocalId = null;
                        }
                        else if (dicLocal.ContainsKey(item.LocalId.Trim()))
                        {
                            item.LocalId = dicLocal[item.LocalId.Trim()];
                        }

                    }
                    if (Data.Count > 0)
                    {
                        int j = 1000;

                        for (int i = 0; i < Data.Count; i += 1000)

                        {

                            var cList = new List<PB_Tray>();

                            cList = Data.Take(j).Skip(i).ToList();

                            j += 1000;

                            await _pB_TrayBus.AddDataExlAsync(cList);

                        }
                        ReturnValue = $"数据导入成功,共导入{CountRow - 1}条数据。";
                    }
                }

                if (!flag)
                {
                    return Error(ReturnValue = "数据存在问题！" + ReturnValue);
                }
            }
            catch (Exception)
            {
                return Error("数据异常！");
            }

            return Success(ReturnValue);

        }

        /// <summary>
        /// 托盘信息表模板导出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [NoCheckJWT]
        public IActionResult ExportToExcel()
        {
            //创建EXCEL工作薄
            IWorkbook workBook = new XSSFWorkbook();
            //创建sheet文件表
            ISheet sheet = workBook.CreateSheet("托盘信息");

            var expDir = string.Format("{0}Export\\{1}", System.AppDomain.CurrentDomain.BaseDirectory,
                    DateTime.Now.ToString("yyyyMM"));

            if (!Directory.Exists(expDir)) Directory.CreateDirectory(expDir);

            string filePath = string.Format("{0}\\CD{1}.xlsx", expDir, DateTime.Now.ToString("yyyyMMddHHmmss"));

            #region 创建Excel表头
            //创建表头
            IRow header = sheet.CreateRow(0);
            ICell cell = header.CreateCell(0);
            cell.SetCellValue("托盘编号");

            cell = header.CreateCell(1);
            cell.SetCellValue("托盘名称");

            cell = header.CreateCell(2);
            cell.SetCellValue("货位编号");

            cell = header.CreateCell(3);
            cell.SetCellValue("托盘类型");

            #endregion            
            //工作流写入，通过流的方式进行创建生成文件
            using (MemoryStream stream = new MemoryStream())
            {
                workBook.Write(stream);
                byte[] buffer = stream.ToArray();

                return File(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", string.Format("托盘信息表_{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss")));
            }

        }

        /// <summary>
        /// 空托盘自动出库
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<AjaxResult<PB_Tray>> OutAutoTray(OutAutoByTary data)
        {
            var traySvc = this._provider.GetRequiredService<IPB_TrayBusiness>();
            var tray = await traySvc.GetByLocation(data.TrayTypeId);
            if (tray == null) return new AjaxResult<PB_Tray>() { Success = false, Msg = "托盘号为空" };

            var StorId = _Op.Property.DefaultStorageId;
            var listOut = await this._pB_TrayBus.ReqBlankTray(StorId, data.TrayTypeId);
            if (listOut.Local == null || listOut.Tray == null) return new AjaxResult<PB_Tray>() { Success = false, Msg = "无空托盘" };

            var entity = new PB_Tray()
            {
                Code = listOut.Tray.Code,
                PB_Location = listOut.Local
            };
            
            return new AjaxResult<PB_Tray>() { Success = true, Msg = "空托盘自动出库成功", Data = entity };
        }

        /// <summary>
        /// 空托盘手动出库
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<AjaxResult<PB_Tray>> OutManualTray(OutAutoByTary data)
        {
            var StorId = _Op.Property.DefaultStorageId;

            var traySvc = this._provider.GetRequiredService<IPB_TrayBusiness>();
            var tray = await traySvc.GetByTrayId(data.TrayCode);
            if (tray == null) return new AjaxResult<PB_Tray>() { Success = false, Msg = "托盘号为空" };
            if (tray.LocalId == null) return new AjaxResult<PB_Tray>() { Success = false, Msg = "货位编号为空" };

            var localSvc = this._provider.GetRequiredService<IPB_LocationBusiness>();
            var local = await localSvc.GetTheDataAsync(tray.LocalId);

            var entity = new PB_Tray()
            {
                Code = tray.Code,
                LocalId = tray.LocalId,
                PB_Location = local
            };
            return new AjaxResult<PB_Tray>() { Success = true, Msg = "空托盘手动出库成功", Data = entity };
        }

        /// <summary>
        /// 空托盘自动入库
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<AjaxResult<PB_Tray>> InAutoTray(InAutoByTary data)
        {
            var traySvc = this._provider.GetRequiredService<IPB_TrayBusiness>();
            var tray = await traySvc.GetByTrayId(data.TrayCode);
            if (tray == null) return new AjaxResult<PB_Tray>() { Success = false, Msg = "托盘号为空" };

            var StorId = _Op.Property.DefaultStorageId;
            var local = await this._pB_TrayBus.InNullTray(StorId, data.TrayCode);
            if (local.IsNullOrEmpty()) return new AjaxResult<PB_Tray>() { Success = false, Msg = "没有可以入库的货位" };

            var entity = new PB_Tray()
            {
                Id = tray.Id,
                LocalId = local.Id,
                Code = tray.Code,
                Name = tray.Name,
                TrayTypeId = tray.TrayTypeId,
                StartTime = tray.StartTime,
                Status = tray.Status,
                Remarks = tray.Remarks,
                CreateTime = tray.CreateTime,
                CreatorId = tray.CreatorId,
                Deleted = tray.Deleted,
                PB_Location = local,                
            };
            if (!entity.Id.IsNullOrEmpty())
            {
                await _pB_TrayBus.UpdateDataAsync(entity);
            }
            return new AjaxResult<PB_Tray>() { Success = true, Msg = "空托盘自动入库成功", Data = entity };//, 
        }

        /// <summary>
        /// 空托盘手动入库
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<AjaxResult<PB_Tray>> InManualTray(InAutoByTary data)
        {
            var StorId = _Op.Property.DefaultStorageId;

            var traySvc = this._provider.GetRequiredService<IPB_TrayBusiness>();
            var tray = await traySvc.GetByTrayId(data.TrayCode);
            if (tray == null) return new AjaxResult<PB_Tray>() { Success = false, Msg = "托盘号为空" };

            var localSvc = this._provider.GetRequiredService<IPB_LocationBusiness>();
            var local = await localSvc.GetByCode(StorId,data.LocalCode);
            if (local == null) return new AjaxResult<PB_Tray>() { Success = false, Msg = "货位编号为空" };

            var entity = new PB_Tray()
            {
                Code = tray.Code,
                PB_Location = local
            };
            return new AjaxResult<PB_Tray>() { Success = true, Msg = "空托盘手动入库成功", Data = entity }; 
        }
        /// <summary>
        /// 空托盘查询
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        //public async Task<AjaxResult<PB_Tray>> QueryTray(InAutoByTary data)
        //{
        //    var traySvc = this._provider.GetRequiredService<IPB_TrayBusiness>();
        //    var tray = await traySvc.GetByTrayId(data.TrayId);
        //    if (tray == null) return new AjaxResult<PB_Tray>() { Success = false, Msg = "此托盘不存在" };

        //    return new AjaxResult<PB_Tray>() { Success = true, Msg = "托盘号查询成功", Data = tray };
        //}
        #endregion
    }
}