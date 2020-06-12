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
using OfficeOpenXml;
using Microsoft.AspNetCore.Hosting;

namespace Coldairarrow.Api.Controllers.PB
{
    [Route("/PB/[controller]/[action]")]
    //[ApiController]
    public class PB_LocationController : BaseApiController
    {
        #region DI

        public PB_LocationController(IPB_LocationBusiness pB_LocationBus,IOperator op, IServiceProvider provider, IWebHostEnvironment hostingEnvironment)
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
        /// 库位数据导入
        /// </summary>
        /// <param name="excelfile"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Import(IFormFile excelfile)
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = $"{Guid.NewGuid()}.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            try
            {
                //把excelfile中的数据复制到file中
                using (FileStream fs = new FileStream(file.ToString(), FileMode.Create)) //初始化一个指定路径和创建模式的FileStream
                {
                    excelfile.CopyTo(fs);
                    fs.Flush();  //清空stream的缓存，并且把缓存中的数据输出到file
                }

                using (ExcelPackage package = new ExcelPackage(file))
                {
                    StringBuilder sb = new StringBuilder();
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    int rowCount = worksheet.Dimension.Rows;
                    int ColCount = worksheet.Dimension.Columns;

                    List<PB_Location> list = new List<PB_Location>();
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var item = new PB_Location();
                        item.Id = IdHelper.GetId();
                        item.Code = worksheet.Cells[row, 1].Value.ToString();
                        item.Name= worksheet.Cells[row, 2].Value.ToString();
                        item.StorId = worksheet.Cells[row, 3].Value.ToString();
                        item.AreaId = worksheet.Cells[row, 4].Value.ToString();
                        item.LanewayId = worksheet.Cells[row, 5].Value.ToString();
                        item.RackId = worksheet.Cells[row, 6].Value.ToString();
                        item.OverVol = Convert.ToDouble(worksheet.Cells[row, 7].Value.ToString());
                        item.IsForbid = Convert.ToBoolean(worksheet.Cells[row, 8].Value.ToString());
                        item.IsDefault = Convert.ToBoolean(worksheet.Cells[row, 9].Value.ToString());
                        item.CreatorId = "Admin";
                        list.Add(item);
                    }
                    if(list.Count>0)
                    {
                        await _pB_LocationBus.AddDataExlAsync(list);
                    }
                    //return Content(sb.ToString());
                }
            }
            catch (Exception )
            {
                //return Content(ex.Message);
            }
        }

        #endregion
    }
}