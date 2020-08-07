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
    public partial class PB_MaterialController : BaseApiController
    {
        #region DI

        public PB_MaterialController(IPB_MaterialBusiness pB_MaterialBus,IServiceProvider provider, IOperator op)
        {
            _pB_MaterialBus = pB_MaterialBus;
            _provider = provider;
            _Op = op;
        }

        IPB_MaterialBusiness _pB_MaterialBus { get; }

        IServiceProvider _provider { get; }

        IOperator _Op { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_Material>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _pB_MaterialBus.GetDataListAsync(input);
        }


        [HttpPost]
        public async Task<List<PB_Material>> GetAllDataList()
        {
            return await _pB_MaterialBus.GetDataListAsync();
        }


        [HttpPost]
        public async Task<PB_Material> GetTheData(IdInputDTO input)
        {
            return await _pB_MaterialBus.GetTheDataAsync(input.id);
        }

        [HttpPost]
        public async Task<List<PB_Material>> GetQueryData(SelectQueryDTO search)
        {
            return await _pB_MaterialBus.GetQueryData(search);
        }

        /// <summary>
        /// 物料数据导入
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
                var Data = new List<PB_Material>();
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
                        PB_Material commodity = new PB_Material();
                        var row = sheet.GetRow(i);
                        if (row.GetCell(0) != null && row.GetCell(0).ToString().Trim().Length > 0)
                        {
                            commodity.Id = IdHelper.GetId();
                            commodity.CreatorId = _Op.UserId; //"Admin";//_Op.UserId;

                            commodity.Code = row.GetCell(0).ToString();

                        }
                        if (row.GetCell(1) != null && row.GetCell(1).ToString().Trim().Length > 0)
                        {
                            commodity.Name = row.GetCell(1).ToString();
                        }
                        if (row.GetCell(2) != null && row.GetCell(2).ToString().Trim().Length > 0)
                        {
                            commodity.BarCode = row.GetCell(2).ToString();
                        }
                        if (row.GetCell(3) != null && row.GetCell(3).ToString().Trim().Length > 0)
                        {
                            commodity.SimpleName = row.GetCell(3).ToString();
                        }
                        if (row.GetCell(4) != null && row.GetCell(4).ToString().Trim().Length > 0)
                        {
                            commodity.MaterialTypeId = row.GetCell(4).ToString();
                        }
                        if (row.GetCell(5) != null && row.GetCell(5).ToString().Trim().Length > 0)
                        {
                            commodity.MeasureId = row.GetCell(5).ToString();
                        }
                        if (row.GetCell(6) != null && row.GetCell(6).ToString().Trim().Length > 0)
                        {
                            commodity.Spec = row.GetCell(6).ToString();
                        }
                        if (row.GetCell(7) != null && row.GetCell(7).ToString().Trim().Length > 0)
                        {
                            commodity.Max = Convert.ToDouble(row.GetCell(7).ToString());
                        }
                        if (row.GetCell(8) != null && row.GetCell(8).ToString().Trim().Length > 0)
                        {
                            commodity.Min = Convert.ToDouble(row.GetCell(8).ToString());
                        }
                        if (row.GetCell(9) != null && row.GetCell(9).ToString().Trim().Length > 0)
                        {
                            commodity.CusId = row.GetCell(9).ToString();
                        }
                        if (row.GetCell(10) != null && row.GetCell(10).ToString().Trim().Length > 0)
                        {
                            commodity.SupId = row.GetCell(10).ToString();
                        }
                        if (row.GetCell(11) != null && row.GetCell(11).ToString().Trim().Length > 0)
                        {
                            commodity.StorId = row.GetCell(11).ToString();
                        }
                        if (row.GetCell(12) != null && row.GetCell(12).ToString().Trim().Length > 0)
                        {
                            commodity.Price = Convert.ToDouble(row.GetCell(12).ToString());
                        }
                        Data.Add(commodity);
                    }
                    var listMaterialTypeCodes = Data.Select(s => s.MaterialTypeId).Select(s => s.Trim()).Distinct().ToList();
                    var dicMaterialType = _pB_MaterialBus.GetQueryable<PB_MaterialType>().Where(w => listMaterialTypeCodes.Contains(w.Code)).ToDictionary(k => k.Code, v => v.Id);

                    var listMeasureCodes = Data.Select(s => s.MeasureId).Select(s => s.Trim()).Distinct().ToList();
                    var dicMeasure = _pB_MaterialBus.GetQueryable<PB_Measure>().Where(w => listMeasureCodes.Contains(w.Code)).ToDictionary(k => k.Code, v => v.Id);

                    var listCusCodes = Data.Select(s => s.CusId).Distinct().ToList();
                    var dicCus = _pB_MaterialBus.GetQueryable<PB_Customer>().Where(w => listCusCodes.Contains(w.Code)).ToDictionary(k => k.Code, v => v.Id);

                    var listSupCodes = Data.Select(s => s.SupId).Distinct().ToList();
                    var dicSup = _pB_MaterialBus.GetQueryable<PB_Supplier>().Where(w => listSupCodes.Contains(w.Code)).ToDictionary(k => k.Code, v => v.Id);

                    var listStorCodes = Data.Select(s => s.StorId).Select(s => s.Trim()).Distinct().ToList();
                    var dicStor = _pB_MaterialBus.GetQueryable<PB_Storage>().Where(w => listStorCodes.Contains(w.Code)).ToDictionary(k => k.Code, v => v.Id);

                    foreach (var item in Data)
                    {
                        if (dicMaterialType.ContainsKey(item.MaterialTypeId.Trim()))
                            item.MaterialTypeId = dicMaterialType[item.MaterialTypeId.Trim()];
                        else
                            throw new Exception("物料类型编号不存在！");

                        if (dicMeasure.ContainsKey(item.MeasureId.Trim()))
                            item.MeasureId = dicMeasure[item.MeasureId.Trim()];
                        else
                            throw new Exception("单位编号不存在！");

                        if (item.CusId == null)
                        {
                            item.CusId = null;
                        } else if (dicCus.ContainsKey(item.CusId.Trim()))
                            item.CusId = dicCus[item.CusId.Trim()];
                        else
                            throw new Exception("客户编号不存在！");

                        if (item.SupId == null)
                        {
                            item.SupId = null;
                        }
                        else if (dicSup.ContainsKey(item.SupId.Trim()))
                            item.SupId = dicSup[item.SupId.Trim()];
                        else
                            throw new Exception("供应商编号不存在！");

                        if (dicStor.ContainsKey(item.StorId.Trim()))
                            item.StorId = dicStor[item.StorId.Trim()];
                        else
                            throw new Exception("仓库编号不存在！");

                        if (item.BarCode == null)
                        {
                            item.BarCode = null;
                        }
                        if (item.SimpleName == null)
                        {
                            item.SimpleName = null;
                        }
                        if (item.Spec == null)
                        {
                            item.Spec = null;
                        }
                        if (item.Max == null)
                        {
                            item.Max = null;
                        }
                        if (item.Min == null)
                        {
                            item.Min = null;
                        }
                        if (item.Price == null)
                        {
                            item.Price = null;
                        }

                    }
                    if (Data.Count > 0)
                    {
                        int j = 1000;
                        for (int i = 0; i < Data.Count; i += 1000)
                        {
                            var cList = new List<PB_Material>();
                            cList = Data.Take(j).Skip(i).ToList();
                            j += 1000;
                            await _pB_MaterialBus.AddDataExlAsync(cList);
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
            ISheet sheet = workBook.CreateSheet("物料信息");

            var expDir = string.Format("{0}Export\\{1}", System.AppDomain.CurrentDomain.BaseDirectory,
                    DateTime.Now.ToString("yyyyMM"));

            if (!Directory.Exists(expDir)) Directory.CreateDirectory(expDir);

            string filePath = string.Format("{0}\\CD{1}.xlsx", expDir, DateTime.Now.ToString("yyyyMMddHHmmss"));

            #region 创建Excel表头
            //创建表头
            IRow header = sheet.CreateRow(0);
            ICell cell = header.CreateCell(0);
            cell.SetCellValue("物料编号");

            cell = header.CreateCell(1);
            cell.SetCellValue("物料名称");

            cell = header.CreateCell(2);
            cell.SetCellValue("物料条码");

            cell = header.CreateCell(3);
            cell.SetCellValue("物料简称");

            cell = header.CreateCell(4);
            cell.SetCellValue("物料类型");

            cell = header.CreateCell(5);
            cell.SetCellValue("单位编号");

            cell = header.CreateCell(6);
            cell.SetCellValue("物料规格");

            cell = header.CreateCell(7);
            cell.SetCellValue("上限数量");

            cell = header.CreateCell(8);
            cell.SetCellValue("下限数量");

            cell = header.CreateCell(9);
            cell.SetCellValue("客户编号");

            cell = header.CreateCell(10);
            cell.SetCellValue("供应商编号");

            cell = header.CreateCell(11);
            cell.SetCellValue("仓库编号");

            cell = header.CreateCell(12);
            cell.SetCellValue("价格");
            #endregion            
            //工作流写入，通过流的方式进行创建生成文件
            using (MemoryStream stream = new MemoryStream())
            {
                workBook.Write(stream);
                byte[] buffer = stream.ToArray();

                return File(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", string.Format("物料信息表_{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss")));
            }

        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_Material data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);
                if (data.Code.IsNullOrWhiteSpace())
                {
                    data.Code = await _provider.GetRequiredService<IPB_BarCodeTypeBusiness>().Generate("PB_Material");
                }

                await _pB_MaterialBus.AddDataAsync(data);
            }
            else
            {
                await _pB_MaterialBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _pB_MaterialBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}