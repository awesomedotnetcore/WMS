using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
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
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using Coldairarrow.IBusiness;

namespace Coldairarrow.Api.Controllers.PB
{
    [Route("/PB/[controller]/[action]")]
    //[ApiController]
    public class PB_LocationController : BaseApiController
    {
        #region DI

        public PB_LocationController(IPB_LocationBusiness pB_LocationBus, IOperator op, IServiceProvider provider, IWebHostEnvironment hostingEnvironment)
        // IWebHostEnvironment  IHostingEnvironment
        {
            _pB_LocationBus = pB_LocationBus;
            _Op = op;
            _provider = provider;
            _hostingEnvironment = hostingEnvironment;

        }
        IOperator _Op { get; }
        IPB_LocationBusiness _pB_LocationBus { get; }
        IServiceProvider _provider { get; }

        IWebHostEnvironment _hostingEnvironment { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_Location>> GetDataList(PageInput<PB_LocationQM> input)
        {
            return await _pB_LocationBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<List<PB_Location>> GetAllData()
        {
            return await _pB_LocationBus.GetDataListAsync();
        }

        [HttpPost]
        public async Task<PB_Location> GetTheData(IdInputDTO input)
        {
            return await _pB_LocationBus.GetTheDataAsync(input.id);
        }

        [HttpPost]
        public async Task<List<PB_Location>> GetQueryData(PB_LocationSelectQueryDTO search)
        {
            return await _pB_LocationBus.GetQueryData(search);
        }

        /// <summary>
        /// 查询仓库
        /// </summary>
        [HttpPost]
        public async Task<List<PB_Location>> Query()
        {
            return await _pB_LocationBus.QueryAsync();
        }
        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_Location data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);
                if (data.Code.IsNullOrWhiteSpace())
                {
                    data.Code = await _provider.GetRequiredService<IPB_BarCodeTypeBusiness>().Generate("PB_Location");
                }

                await _pB_LocationBus.AddDataAsync(data);
            }
            else
            {
                await _pB_LocationBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _pB_LocationBus.DeleteDataAsync(ids);
        }

        /// <summary>
        /// 货位信息数据导入
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
                string fileExt = Path.GetExtension(file.FileName).ToLower();
                //定义一个集合一会儿将数据存储进来,全部一次丢到数据库中保存
                var Data = new List<PB_Location>();
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
                        PB_Location commodity = new PB_Location();
                        var row = sheet.GetRow(i);
                        if (row.GetCell(0) != null && row.GetCell(0).ToString().Trim().Length > 0)
                        {
                            commodity.Id = IdHelper.GetId();
                            commodity.CreatorId = _Op.UserId; //"Admin";//_Op.UserId;
                            commodity.IsForbid = true;//导入默认启用
                            commodity.IsDefault = false;//导入无默认

                            commodity.Code = row.GetCell(0).ToString();

                        }
                        if (row.GetCell(1) != null && row.GetCell(1).ToString().Trim().Length > 0)
                        {
                            commodity.Name = row.GetCell(1).ToString();
                        }
                        if (row.GetCell(2) != null && row.GetCell(2).ToString().Trim().Length > 0)
                        {
                            commodity.StorId = row.GetCell(2).ToString();
                        }
                        if (row.GetCell(3) != null && row.GetCell(3).ToString().Trim().Length > 0)
                        {
                            commodity.AreaId = row.GetCell(3).ToString();
                        }
                        if (row.GetCell(4) != null && row.GetCell(4).ToString().Trim().Length > 0)
                        {
                            commodity.LanewayId = row.GetCell(4).ToString().ToString();
                        }
                        if (row.GetCell(5) != null && row.GetCell(5).ToString().Trim().Length > 0)
                        {
                            commodity.RackId = row.GetCell(5).ToString();
                        }
                        if (row.GetCell(6) != null && row.GetCell(6).ToString().Trim().Length > 0)
                        {
                            commodity.OverVol = Convert.ToDouble(row.GetCell(6).ToString());
                        }
                        Data.Add(commodity);
                    }
                    var listStorCodes = Data.Select(s => s.StorId).Select(s => s.Trim()).Distinct().ToList();
                    var dicStor = _pB_LocationBus.GetQueryable<PB_Storage>().Where(w => listStorCodes.Contains(w.Code)).ToDictionary(k => k.Code, v => v.Id);

                    var listAreaCodes = Data.Select(s => s.AreaId).Select(s => s.Trim()).Distinct().ToList();
                    var dicArea = _pB_LocationBus.GetQueryable<PB_StorArea>().Where(w => listAreaCodes.Contains(w.Code)).ToDictionary(k => k.Code, v => v.Id);


                    var listLanewayCodes = Data.Select(s => s.LanewayId).Select(s => s.Trim()).Distinct().ToList();
                    var dicLaneway = _pB_LocationBus.GetQueryable<PB_Laneway>().Where(w => listLanewayCodes.Contains(w.Code)).ToDictionary(k => k.Code, v => v.Id);

                    var listRackCodes = Data.Select(s => s.RackId).Select(s => s.Trim()).Distinct().ToList();
                    var dicRack = _pB_LocationBus.GetQueryable<PB_Rack>().Where(w => listRackCodes.Contains(w.Code)).ToDictionary(k => k.Code, v => v.Id);

                    foreach (var item in Data)
                    {
                        if (dicStor.ContainsKey(item.StorId.Trim()))
                            item.StorId = dicStor[item.StorId.Trim()];
                        else
                            throw new Exception("仓库编号不存在！");

                        if (dicArea.ContainsKey(item.AreaId.Trim()))
                            item.AreaId = dicArea[item.AreaId.Trim()];
                        else
                            throw new Exception("货区编号不存在！");

                        if (item.LanewayId == null)
                        {
                            item.LanewayId = null;
                        }
                        else if (dicLaneway.ContainsKey(item.LanewayId.Trim()))
                        {
                            item.LanewayId = dicLaneway[item.LanewayId.Trim()];
                        }

                        if (item.RackId == null)
                        {
                            item.RackId = null;
                        }
                        else if (dicRack.ContainsKey(item.RackId.Trim()))
                        {
                            item.RackId = dicRack[item.RackId.Trim()];
                        }

                    }
                    if (Data.Count > 0)
                    {
                        int j = 1000;

                        for (int i = 0; i < Data.Count; i += 1000)

                        {

                            var cList = new List<PB_Location>();

                            cList = Data.Take(j).Skip(i).ToList();

                            j += 1000;

                            await _pB_LocationBus.AddDataExlAsync(cList);

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
        /// 货位信息表模板导出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [NoCheckJWT]
        public IActionResult ExportToExcel()//async Task<IActionResult>
        {
            //var data = await _tD_CheckDataBus.QueryDataListAsync(checkId);

            //创建EXCEL工作薄
            IWorkbook workBook = new XSSFWorkbook();
            //创建sheet文件表
            ISheet sheet = workBook.CreateSheet("货位信息");

            var expDir = string.Format("{0}Export\\{1}", System.AppDomain.CurrentDomain.BaseDirectory,
                    DateTime.Now.ToString("yyyyMM"));

            if (!Directory.Exists(expDir)) Directory.CreateDirectory(expDir);

            string filePath = string.Format("{0}\\CD{1}.xlsx", expDir, DateTime.Now.ToString("yyyyMMddHHmmss"));

            #region 创建Excel表头
            //创建表头
            IRow header = sheet.CreateRow(0);
            ICell cell = header.CreateCell(0);
            cell.SetCellValue("货位编号");

            cell = header.CreateCell(1);
            cell.SetCellValue("货位名称");

            cell = header.CreateCell(2);
            cell.SetCellValue("仓库编号");

            cell = header.CreateCell(3);
            cell.SetCellValue("货区编号");

            cell = header.CreateCell(4);
            cell.SetCellValue("巷道编号");

            cell = header.CreateCell(5);
            cell.SetCellValue("货架编号");

            cell = header.CreateCell(6);
            cell.SetCellValue("剩余容量");

            //cell = header.CreateCell(7);
            //cell.SetCellValue("是否默认(true/false)");
            #endregion            
            //工作流写入，通过流的方式进行创建生成文件
            using (MemoryStream stream = new MemoryStream())
            {
                workBook.Write(stream);
                byte[] buffer = stream.ToArray();

                return File(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", string.Format("货位信息表_{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss")));
            }
            
        }
        #endregion
    }

}