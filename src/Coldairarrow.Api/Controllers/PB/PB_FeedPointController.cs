using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.IBusiness;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    [Route("/PB/[controller]/[action]")]
    public class PB_FeedPointController : BaseApiController
    {
        #region DI

        public PB_FeedPointController(IPB_FeedPointBusiness pB_FeedPointBus, IOperator op)
        {
            _pB_FeedPointBus = pB_FeedPointBus;
            _Op = op;
        }

        IPB_FeedPointBusiness _pB_FeedPointBus { get; }

        IOperator _Op { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_FeedPoint>> GetDataList(PageInput<PB_FeedPointQM> input)
        {
            return await _pB_FeedPointBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<PB_FeedPoint> GetTheData(IdInputDTO input)
        {
            return await _pB_FeedPointBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_FeedPoint data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _pB_FeedPointBus.AddDataAsync(data);
            }
            else
            {
                await _pB_FeedPointBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _pB_FeedPointBus.DeleteDataAsync(ids);
        }

        [HttpPost]
        public async Task Enable(string id,bool enable)
        {
            await _pB_FeedPointBus.Enable(id, enable);
        }

        /// <summary>
        /// 上下料点数据导入
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
                var Data = new List<PB_FeedPoint>();
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
                        PB_FeedPoint commodity = new PB_FeedPoint();
                        var row = sheet.GetRow(i);
                        if (row.GetCell(0) != null && row.GetCell(0).ToString().Trim().Length > 0)
                        {
                            commodity.Id = IdHelper.GetId();
                            commodity.CreatorId = _Op.UserId; //"Admin";//_Op.UserId;
                            commodity.IsEnable = false;//默认停用

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
                            commodity.LaneId = row.GetCell(3).ToString();
                        }
                        if (row.GetCell(4) != null && row.GetCell(4).ToString().Trim().Length > 0)
                        {
                            commodity.Type = row.GetCell(4).ToString().ToString();
                        }
                        Data.Add(commodity);
                    }
                    var listStorCodes = Data.Select(s => s.StorId).Select(s => s.Trim()).Distinct().ToList();
                    var dicStor = _pB_FeedPointBus.GetQueryable<PB_Storage>().Where(w => listStorCodes.Contains(w.Code)).ToDictionary(k => k.Code, v => v.Id);

                    var listLaneCodes = Data.Select(s => s.LaneId).Select(s => s.Trim()).Distinct().ToList();
                    var dicLane = _pB_FeedPointBus.GetQueryable<PB_Laneway>().Where(w => listLaneCodes.Contains(w.Code)).ToDictionary(k => k.Code, v => v.Id);

                    foreach (var item in Data)
                    {
                        if (dicStor.ContainsKey(item.StorId.Trim()))
                            item.StorId = dicStor[item.StorId.Trim()];
                        else
                            throw new Exception("仓库不存在！");

                        if (dicLane.ContainsKey(item.LaneId.Trim()))
                            item.LaneId = dicLane[item.LaneId.Trim()];
                        else
                            throw new Exception("巷道不存在！");

                    }
                    if (Data.Count > 0)
                    {
                        int j = 1000;

                        for (int i = 0; i < Data.Count; i += 1000)

                        {

                            var cList = new List<PB_FeedPoint>();

                            cList = Data.Take(j).Skip(i).ToList();

                            j += 1000;

                            await _pB_FeedPointBus.AddDataExlAsync(cList);

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
        public IActionResult ExportToExcel()
        {
            //创建EXCEL工作薄
            IWorkbook workBook = new XSSFWorkbook();
            //创建sheet文件表
            ISheet sheet = workBook.CreateSheet("上下料点信息");

            var expDir = string.Format("{0}Export\\{1}", System.AppDomain.CurrentDomain.BaseDirectory,
                    DateTime.Now.ToString("yyyyMM"));

            if (!Directory.Exists(expDir)) Directory.CreateDirectory(expDir);

            string filePath = string.Format("{0}\\CD{1}.xlsx", expDir, DateTime.Now.ToString("yyyyMMddHHmmss"));

            #region 创建Excel表头
            //创建表头
            IRow header = sheet.CreateRow(0);
            ICell cell = header.CreateCell(0);
            cell.SetCellValue("料点编号");

            cell = header.CreateCell(1);
            cell.SetCellValue("料点名称");

            cell = header.CreateCell(2);
            cell.SetCellValue("仓库编号");

            cell = header.CreateCell(3);
            cell.SetCellValue("巷道编号");

            cell = header.CreateCell(4);
            cell.SetCellValue("料点类型");

            ISheet sheet1 = workBook.GetSheetAt(0);//获得第一个工作表
            CellRangeAddressList regions = new CellRangeAddressList(1, 65535, 4, 4);//设定位置 行起，行止,列起,列终
            XSSFDataValidationHelper helper = new XSSFDataValidationHelper((XSSFSheet)sheet1);//获得一个数据验证Helper
            IDataValidation validation = helper.CreateValidation(helper.CreateExplicitListConstraint(new string[] { "In", "Out", "InOut", "OutBack" }), regions);//创建一个特定约束范围内的公式列表约束（即第一节里说的"自定义"方式）
            validation.CreateErrorBox("错误", "请按右侧下拉箭头选择!");//不符合约束时的提示
            validation.ShowErrorBox = true;//显示上面提示 = True
            sheet1.AddValidationData(validation);//添加进去
            sheet1.ForceFormulaRecalculation = true;

            #endregion            
            //工作流写入，通过流的方式进行创建生成文件
            using (MemoryStream stream = new MemoryStream())
            {
                workBook.Write(stream);
                byte[] buffer = stream.ToArray();

                return File(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", string.Format("上下料点信息表_{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss")));
            }

        }

        #endregion
    }
}