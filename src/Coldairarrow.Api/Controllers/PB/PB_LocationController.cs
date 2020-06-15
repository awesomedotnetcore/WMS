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
using System.Text;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;
//using NPOI.SS.Formula.Functions;

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

        //SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
        //{
        //    ConnectionString = "database='" + "ZEQP_WMS" + "';Data Source = '" + "10.76.20.194" + "'; User Id = '" + "root" + "'; pwd='" + "ABCabc123" + "';charset='utf8';pooling=true",
        //    //"Data Source=10.76.20.194;Initial Catalog=ZEQP_WMS;User id=root;Password=ABCabc123",
        //    // Data Source=localhost;Initial Catalog=test;User id=sa;Password=123456
        //    //server = 10.76.20.194; user id = root; password=ABCabc123;persistsecurityinfo=True;database=ZEQP_WMS;SslMode=none
        //    DbType = SqlSugar.DbType.MySql,
        //    IsAutoCloseConnection = true,
        //    InitKeyType = InitKeyType.Attribute
        //});

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_Location>> GetDataList(PageInput<ConditionDTO> input)
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
        public async Task<List<PB_Location>> GetQueryData(SelectQueryDTO search)
        {
            return await _pB_LocationBus.GetQueryData(search, _Op.Property.DefaultStorageId);
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
        /// 数据导入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [NoCheckJWT]
        public async Task<string> Import(IFormFile file)// file
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
                    return "Excel列表数据项为空!";

                }
                #region 循环验证
                for (int i = 1; i < CountRow; i++)
                {
                    //获取第i行的数据
                    var row = sheet.GetRow(i);
                    if (row != null)
                    {
                        //循环的验证单元格中的数据
                        for (int j = 0; j < 5; j++)
                        {
                            if (row.GetCell(j) == null || row.GetCell(j).ToString().Trim().Length == 0)
                            {
                                flag = false;
                                ReturnValue += $"第{i + 1}行,第{j + 1}列数据不能为空。";
                            }
                        }
                    }
                }
                #endregion
                if (flag)
                {
                    for (int i = 1; i < CountRow; i++)
                    {
                        //实例化实体对象
                        PB_Location commodity = new PB_Location();
                       // List<PB_Location> commodity = new List<PB_Location>();
                        var row = sheet.GetRow(i);
                        if (row.GetCell(0) != null && row.GetCell(0).ToString().Trim().Length > 0)
                        {
                            commodity.Id = IdHelper.GetId();
                            commodity.CreatorId = "Admin";
                        }

                        if (row.GetCell(0) != null && row.GetCell(0).ToString().Trim().Length > 0)
                        {
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
                        if (row.GetCell(7) != null && row.GetCell(7).ToString().Trim().Length > 0)
                        {
                            commodity.IsForbid = Convert.ToBoolean(row.GetCell(7).ToString());
                        }
                        if (row.GetCell(8) != null && row.GetCell(8).ToString().Trim().Length > 0)
                        {
                            commodity.IsDefault = Convert.ToBoolean(row.GetCell(8).ToString());
                        }
                        else
                        {
                            commodity.Remarks = "暂无";
                        }
                        Data.Add(commodity);
                    }
                    //var data = db.Insertable<PB_Location>(Data).ExecuteCommand();
                    //ReturnValue = $"数据导入成功,共导入{CountRow - 1}条数据。";
                    if (Data.Count > 0) 
                    {
                        await _pB_LocationBus.AddDataExlAsync(Data);
                        ReturnValue = $"数据导入成功,共导入{CountRow - 1}条数据。";
                    }
                    
                }

                if (!flag)
                {
                    ReturnValue = "数据存在问题！" + ReturnValue;
                }
            }
            catch (Exception)
            {
                return "服务器异常";
            }

            return ReturnValue;
        }
    

    ///// <summary>
    ///// 导入Excel
    ///// </summary>
    ///// <param name="file">导入文件</param>
    //[HttpPost]
    //public static List<PB_Location> InputExcel(IFormFile file)
    //{
    //    List<PB_Location> list = new List<PB_Location> { };

    //    MemoryStream ms = new MemoryStream();
    //    file.CopyTo(ms);
    //    ms.Seek(0, SeekOrigin.Begin);

    //    IWorkbook workbook = new XSSFWorkbook(ms);
    //    ISheet sheet = workbook.GetSheetAt(0);
    //    IRow cellNum = sheet.GetRow(0);
    //    var propertys = typeof(PB_Location).GetProperties();
    //    string value = null;
    //    int num = cellNum.LastCellNum;

    //    for (int i = 1; i <= sheet.LastRowNum; i++)
    //    {
    //        IRow row = sheet.GetRow(i);
    //        var obj = new PB_Location();
    //        for (int j = 0; j < num; j++)
    //        {
    //            value = row.GetCell(j).ToString();
    //            string str = (propertys[j].PropertyType).FullName;
    //            if (str == "System.String")
    //            {
    //                propertys[j].SetValue(obj, value, null);
    //            }
    //            else if (str == "System.DateTime")
    //            {
    //                DateTime pdt = Convert.ToDateTime(value, CultureInfo.InvariantCulture);
    //                propertys[j].SetValue(obj, pdt, null);
    //            }
    //            else if (str == "System.Boolean")
    //            {
    //                bool pb = Convert.ToBoolean(value);
    //                propertys[j].SetValue(obj, pb, null);
    //            }
    //            else if (str == "System.Int16")
    //            {
    //                short pi16 = Convert.ToInt16(value);
    //                propertys[j].SetValue(obj, pi16, null);
    //            }
    //            else if (str == "System.Int32")
    //            {
    //                int pi32 = Convert.ToInt32(value);
    //                propertys[j].SetValue(obj, pi32, null);
    //            }
    //            else if (str == "System.Int64")
    //            {
    //                long pi64 = Convert.ToInt64(value);
    //                propertys[j].SetValue(obj, pi64, null);
    //            }
    //            else if (str == "System.Byte")
    //            {
    //                byte pb = Convert.ToByte(value);
    //                propertys[j].SetValue(obj, pb, null);
    //            }
    //            else
    //            {
    //                propertys[j].SetValue(obj, null, null);
    //            }
    //        }

    //        list.Add(obj);
    //    }

    //    return list;
    //}


    ///// <summary>
    ///// 库位数据导入
    ///// </summary>
    ///// <param name="excelfile"></param>
    ///// <returns></returns>
    //[HttpPost]
    //public async Task Import(IFormFile excelfile)
    //{
    //    string sWebRootFolder = _hostingEnvironment.WebRootPath;
    //    string sFileName = $"{Guid.NewGuid()}.xlsx";
    //    FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
    //    try
    //    {
    //        //把excelfile中的数据复制到file中
    //        using (FileStream fs = new FileStream(file.ToString(), FileMode.Create)) //初始化一个指定路径和创建模式的FileStream
    //        {
    //            excelfile.CopyTo(fs);
    //            fs.Flush();  //清空stream的缓存，并且把缓存中的数据输出到file
    //        }

    //        using (ExcelPackage package = new ExcelPackage(file))
    //        {
    //            StringBuilder sb = new StringBuilder();
    //            ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
    //            int rowCount = worksheet.Dimension.Rows;
    //            int ColCount = worksheet.Dimension.Columns;

    //            List<PB_Location> list = new List<PB_Location>();
    //            for (int row = 2; row <= rowCount; row++)
    //            {
    //                var item = new PB_Location();
    //                item.Id = IdHelper.GetId();
    //                item.Code = worksheet.Cells[row, 1].Value.ToString();
    //                item.Name= worksheet.Cells[row, 2].Value.ToString();
    //                item.StorId = worksheet.Cells[row, 3].Value.ToString();
    //                item.AreaId = worksheet.Cells[row, 4].Value.ToString();
    //                item.LanewayId = worksheet.Cells[row, 5].Value.ToString();
    //                item.RackId = worksheet.Cells[row, 6].Value.ToString();
    //                item.OverVol = Convert.ToDouble(worksheet.Cells[row, 7].Value.ToString());
    //                item.IsForbid = Convert.ToBoolean(worksheet.Cells[row, 8].Value.ToString());
    //                item.IsDefault = Convert.ToBoolean(worksheet.Cells[row, 9].Value.ToString());
    //                item.CreatorId = "Admin";
    //                list.Add(item);
    //            }
    //            if(list.Count>0)
    //            {
    //                await _pB_LocationBus.AddDataExlAsync(list);
    //            }
    //            //return Content(sb.ToString());
    //        }
    //    }
    //    catch (Exception )
    //    {
    //        //return Content(ex.Message);
    //    }
    //}

    #endregion
}
}