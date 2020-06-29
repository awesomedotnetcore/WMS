using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Business.PB
{
    public partial class PB_BarCodeTypeBusiness : BaseBusiness<PB_BarCodeType>, IPB_BarCodeTypeBusiness, ITransientDependency
    {
        public static readonly char[] r = new char[] { 'Q', 'A', 'Z', 'W', 'S', 'X', '9', 'E', 'D', '1', 'C', 'R', '8', 'F', 'V', '2', 'T', 'G', '7', 'B', 'Y', '3', 'H', 'N', '6', 'U', 'J', '4', 'M', 'K', '5', 'L', 'P' };

        [Transactional]
        public async Task<string> Generate(string typeCode, Dictionary<string, string> para = null)
        {
            var codeType = await this.GetIQueryable()
                .Include(w => w.BarCodeRules)
                .Where(w => w.Code == typeCode)
                .SingleAsync();
            var listCode = new List<string>();
            var rules = codeType.BarCodeRules.OrderBy(o => o.Sort);

            var sysParaName = new List<string>() { "Serial", "Daily", "PerMonth", "PerYear" };
            var serialWhere = LinqHelper.True<PB_BarCodeSerial>();
            serialWhere = serialWhere.And(w => w.TypeId == codeType.Id && sysParaName.Contains(w.ParaName));
            foreach (var rule in rules.Where(w => w.Type == "ParaSerial"))
            {
                if (para.ContainsKey(rule.Rule))
                {
                    var paraVal = para[rule.Rule];
                    serialWhere = serialWhere.Or(w => w.TypeId == codeType.Id && w.ParaName == rule.Rule && w.ParaValue == paraVal);
                }
            }
            var listSerial = await Db.GetIQueryable<PB_BarCodeSerial>().Where(serialWhere).ToListAsync();

            foreach (var rule in rules)
            {
                var ruleCode = this.GenerateByRule(listSerial, rule, para);
                if (!ruleCode.IsNullOrEmpty())
                    listCode.Add(ruleCode);
            }

            var serialSvc = _serviceProvider.GetRequiredService<IPB_BarCodeSerialBusiness>();
            var listUpdate = listSerial.Where(w => string.IsNullOrEmpty(w.TypeId) && !string.IsNullOrEmpty(w.Id)).ToList();
            if (listUpdate.Count > 0)
            {
                listUpdate.ForEach(item => { item.TypeId = codeType.Id; });
                await serialSvc.UpdateDataAsync(listUpdate);
            }
            var listAdd=listSerial.Where(w => string.IsNullOrEmpty(w.TypeId) && string.IsNullOrEmpty(w.Id)).ToList();
            if (listAdd.Count > 0)
            {
                listAdd.ForEach(item =>
                {
                    item.Id = IdHelper.GetId();
                    item.TypeId = codeType.Id;
                });
                await serialSvc.AddDataAsync(listAdd);
            }
            var code = string.Join(codeType.JoinChar, listCode);
            //把新增的BarCode插入条码管理
            //var codeSvc = _serviceProvider.GetRequiredService<IPB_BarCodeBusiness>();
            //var op = _serviceProvider.GetRequiredService<IOperator>();
            //var barCode = new PB_BarCode();
            //barCode.Id = IdHelper.GetId();
            //barCode.BarCode = code;
            //barCode.BarCodeTypeId = codeType.Id;
            //barCode.CreateTime = DateTime.Now;
            //barCode.CreatorId = op.UserId;
            //barCode.Deleted = false;
            //await codeSvc.AddDataAsync(barCode);
            return code;
        }
        private string GenerateByRule(List<PB_BarCodeSerial> listSerial, PB_BarCodeRule rule, Dictionary<string, string> para = null)
        {
            var now = DateTime.Now;
            var code = "";
            switch (rule.Type)
            {
                case "Const": code = rule.Rule; break;
                case "Date": code = now.ToString(rule.Rule.IsNullOrEmpty() ? "YYYYMMDD" : rule.Rule); break;
                case "Serial":
                    {
                        var serial = listSerial.SingleOrDefault(w => w.ParaName == "Serial");
                        if (serial == null)
                        {
                            serial = new PB_BarCodeSerial()
                            {
                                ParaName = "Serial",
                                ParaValue = "",
                                SerialNum = 0
                            };
                            listSerial.Add(serial);
                        }
                        else serial.TypeId = null;
                        var seq = serial.SerialNum + 1;
                        code = seq.ToString();
                        if (rule.length.HasValue)
                            code = code.PadLeft(rule.length.Value, '0');
                        serial.SerialNum = seq;
                    }
                    break;
                case "Daily":
                    {
                        var serial = listSerial.SingleOrDefault(w => w.ParaName == "Daily");
                        if (serial == null)
                        {
                            serial = new PB_BarCodeSerial()
                            {
                                ParaName = "Daily",
                                ParaValue = now.Date.ToString("yyyy-MM-dd HH:mm:ss"),
                                SerialNum = 0
                            };
                            listSerial.Add(serial);
                        }
                        else serial.TypeId = null;
                        var seqDate = serial.ParaValue.ToDateTime();
                        if ((now - seqDate).Days == 0)
                        {
                            var seq = serial.SerialNum + 1;
                            code = seq.ToString();
                            if (rule.length.HasValue)
                                code = code.PadLeft(rule.length.Value, '0');
                            serial.SerialNum = seq;
                        }
                        else
                        {
                            var seq = 1;
                            code = seq.ToString();
                            if (rule.length.HasValue)
                                code = code.PadLeft(rule.length.Value, '0');
                            serial.SerialNum = seq;
                            serial.ParaValue = now.Date.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                    break;
                case "PerMonth":
                    {
                        var serial = listSerial.SingleOrDefault(w => w.ParaName == "PerMonth");
                        if (serial == null)
                        {
                            serial = new PB_BarCodeSerial()
                            {
                                ParaName = "PerMonth",
                                ParaValue = now.Date.ToString("yyyy-MM-dd HH:mm:ss"),
                                SerialNum = 0
                            };
                            listSerial.Add(serial);
                        }
                        else serial.TypeId = null;
                        var seqDate = serial.ParaValue.ToDateTime();
                        if (now.Year == seqDate.Year && now.Month == seqDate.Month)
                        {
                            var seq = serial.SerialNum + 1;
                            code = seq.ToString();
                            if (rule.length.HasValue)
                                code = code.PadLeft(rule.length.Value, '0');
                            serial.SerialNum = seq;
                        }
                        else
                        {
                            var seq = 1;
                            code = seq.ToString();
                            if (rule.length.HasValue)
                                code = code.PadLeft(rule.length.Value, '0');
                            serial.SerialNum = seq;
                            serial.ParaValue = now.Date.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    };
                    break;
                case "PerYear":
                    {
                        var serial = listSerial.SingleOrDefault(w => w.ParaName == "PerYear");
                        if (serial == null)
                        {
                            serial = new PB_BarCodeSerial()
                            {
                                ParaName = "PerYear",
                                ParaValue = now.Date.ToString("yyyy-MM-dd HH:mm:ss"),
                                SerialNum = 0
                            };
                            listSerial.Add(serial);
                        }
                        else serial.TypeId = null;
                        var seqDate = serial.ParaValue.ToDateTime();
                        if (now.Year == seqDate.Year)
                        {
                            var seq = serial.SerialNum + 1;
                            code = seq.ToString();
                            if (rule.length.HasValue)
                                code = code.PadLeft(rule.length.Value, '0');
                            serial.SerialNum = seq;
                        }
                        else
                        {
                            var seq = 1;
                            code = seq.ToString();
                            if (rule.length.HasValue)
                                code = code.PadLeft(rule.length.Value, '0');
                            serial.SerialNum = seq;
                            serial.ParaValue = now.Date.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    };
                    break;
                case "Random":
                    {
                        var newId = IdHelper.GetLongId();
                        var buf = new char[32];
                        var binLen = 33;
                        int charPos = 32;
                        while ((newId / binLen) > 0)
                        {
                            int ind = (int)(newId % binLen);
                            buf[--charPos] = r[ind];
                            newId /= binLen;
                        }
                        buf[--charPos] = r[(int)(newId % binLen)];
                        code = new string(buf, charPos, (32 - charPos));
                        if (rule.length.HasValue)
                            code = code.PadLeft(rule.length.Value, '0');
                    }
                    break;
                case "Parameter":
                    {
                        if (para.ContainsKey(rule.Rule))
                            code = para[rule.Rule];
                        else
                            code = "";
                        if (rule.length.HasValue)
                            code = code.PadLeft(rule.length.Value, '0');
                    }
                    break;
                case "ParaSerial":
                    {
                        if (para.ContainsKey(rule.Rule))
                        {
                            var serial = listSerial.SingleOrDefault(w => w.ParaName == rule.Rule && w.ParaValue == para[rule.Rule]);
                            if (serial == null)
                            {
                                serial = new PB_BarCodeSerial()
                                {
                                    ParaName = rule.Rule,
                                    ParaValue = para[rule.Rule],
                                    SerialNum = 0
                                };
                                listSerial.Add(serial);
                            }
                            else serial.TypeId = null;
                            var seq = serial.SerialNum + 1;
                            code = seq.ToString();
                            if (rule.length.HasValue)
                                code = code.PadLeft(rule.length.Value, '0');
                            serial.SerialNum = seq;
                        }
                    }
                    break;
                default: break;
            }
            return code;
        }

        public async Task<List<PB_BarCodeType>> GetAllData()
        {
            return await this.GetIQueryable().ToListAsync();
        }
    }
}