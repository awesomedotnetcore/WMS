using Coldairarrow.Business.TD;
using Coldairarrow.Entity.TD;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Coldairarrow.IBusiness;

namespace Coldairarrow.Api.Controllers.TD
{
    public partial class TD_CheckDataController 
    {
        [HttpPost]
        public async Task<PageResult<TDCheckDataDTO>> QueryDataList(PageInput<TDCheckDataConditionDTO> input)
        {
            return await _tD_CheckDataBus.QueryDataListAsync(input);
        }

        [HttpPost]
        public async Task ModifyCheckNum(TDCheckNumModifyDTO data)
        {
            await _tD_CheckDataBus.ModifyCheckNumAsync(_provider.GetRequiredService<IOperator>().UserId, data);
        }

        [HttpGet]
        [NoCheckJWT]
        public async Task<IActionResult> ExportToExcel(string checkId)
        {
            var data = await _tD_CheckDataBus.QueryDataListAsync(checkId);

            //创建EXCEL工作薄
            IWorkbook workBook = new XSSFWorkbook();
            //创建sheet文件表
            ISheet sheet = workBook.CreateSheet("盘点清单");

            var expDir = string.Format("{0}Export\\{1}", System.AppDomain.CurrentDomain.BaseDirectory,
                    DateTime.Now.ToString("yyyyMM"));

            if (!Directory.Exists(expDir)) Directory.CreateDirectory(expDir);

            string filePath = string.Format("{0}\\CD{1}.xlsx", expDir, DateTime.Now.ToString("yyyyMMddHHmmss"));

            #region 创建Excel表头
            //创建表头
            IRow header = sheet.CreateRow(0);
            ICell cell = header.CreateCell(0);
            cell.SetCellValue("库区");

            cell = header.CreateCell(1);
            cell.SetCellValue("巷道");

            cell = header.CreateCell(2);
            cell.SetCellValue("货架");

            cell = header.CreateCell(3);
            cell.SetCellValue("托盘");

            cell = header.CreateCell(4);
            cell.SetCellValue("托盘分区");

            cell = header.CreateCell(5);
            cell.SetCellValue("货位编码");

            cell = header.CreateCell(6);
            cell.SetCellValue("货位名称");

            cell = header.CreateCell(7);
            cell.SetCellValue("批次号");

            cell = header.CreateCell(8);
            cell.SetCellValue("物料编码");

            cell = header.CreateCell(9);
            cell.SetCellValue("物料名称");

            cell = header.CreateCell(10);
            cell.SetCellValue("库存数量");

            cell = header.CreateCell(10);
            cell.SetCellValue("盘点数量");

            cell = header.CreateCell(11);
            cell.SetCellValue("盘差数量");
            #endregion
            #region 填充Excel单元格中的数据
            //给工作薄中非表头填充数据，遍历行数据并进行创建和填充表格
            for (int i = 0; i < data.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);//表示从整张数据表的第二行开始创建并填充数据，第一行已经创建。
                                                  //将数据读到cell单元格中
                cell = row.CreateCell(0);
                cell.SetCellValue(data[i].AreaName);

                cell = row.CreateCell(1);
                cell.SetCellValue(data[i].LanewayName);

                cell = row.CreateCell(2);
                cell.SetCellValue(data[i].RackName);

                cell = row.CreateCell(3);
                cell.SetCellValue(data[i].TrayName);

                cell = row.CreateCell(4);
                cell.SetCellValue(data[i].ZoneName);

                cell = row.CreateCell(5);
                cell.SetCellValue(data[i].LocationCode);

                cell = row.CreateCell(6);
                cell.SetCellValue(data[i].LocationName);

                cell = row.CreateCell(7);
                cell.SetCellValue(data[i].BatchNo);

                cell = row.CreateCell(8);
                cell.SetCellValue(data[i].MaterialCode);

                cell = row.CreateCell(9);
                cell.SetCellValue(data[i].MaterialName);

                cell = row.CreateCell(10);
                cell.SetCellValue("-");

                cell = row.CreateCell(10);
                cell.SetCellValue("");

                cell = row.CreateCell(11);
                cell.SetCellValue("-");
            }
            #endregion
            #region 工作流创建Excel文件
            //工作流写入，通过流的方式进行创建生成文件
            using (MemoryStream stream = new MemoryStream())
            {
                workBook.Write(stream);
                byte[] buffer = stream.ToArray();

                return File(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", string.Format("盘点清单_{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss")));
            }
            #endregion
        }
    }
}