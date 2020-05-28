using Coldairarrow.Entity.PB;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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
            foreach (var rule in rules)
            {
                var ruleCode = this.GenerateByRule(codeType, rule, para);
                if (!ruleCode.IsNullOrEmpty())
                    listCode.Add(ruleCode);
            }
            await this.UpdateDataAsync(codeType);
            var code = string.Join(codeType.JoinChar, listCode);
            //把新增的BarCode插入条码管理
            var codeSvc = _serviceProvider.GetRequiredService<IPB_BarCodeBusiness>();
            var op = _serviceProvider.GetRequiredService<IOperator>();
            var barCode = new PB_BarCode();
            barCode.Id = IdHelper.GetId();
            barCode.BarCode = code;
            barCode.BarCodeTypeId = codeType.Id;
            barCode.CreateTime = DateTime.Now;
            barCode.CreatorId = op.UserId;
            barCode.Deleted = false;
            await codeSvc.AddDataAsync(barCode);
            return code;
        }
        private string GenerateByRule(PB_BarCodeType type, PB_BarCodeRule rule, Dictionary<string, string> para = null)
        {
            var now = DateTime.Now;
            var code = "";
            switch (rule.Type)
            {
                case "Const": code = rule.Rule; break;
                case "Date": code = now.ToString(rule.Rule.IsNullOrEmpty() ? "YYYYMMDD" : rule.Rule); break;
                case "Serial":
                    {
                        var seq = type.SeqNum.GetValueOrDefault(0) + 1;
                        code = seq.ToString();
                        if (rule.length.HasValue)
                            code = code.PadLeft(rule.length.Value, '0');
                        type.SeqNum = seq;
                    }
                    break;
                case "Daily":
                    {
                        if (type.SeqDate.HasValue && (now - type.SeqDate.Value).Days == 0)
                        {
                            var seq = type.SeqNum.GetValueOrDefault(0) + 1;
                            code = seq.ToString();
                            if (rule.length.HasValue)
                                code = code.PadLeft(rule.length.Value, '0');
                            type.SeqNum = seq;
                        }
                        else
                        {
                            var seq = 1;
                            code = seq.ToString();
                            if (rule.length.HasValue)
                                code = code.PadLeft(rule.length.Value, '0');
                            type.SeqNum = seq;
                            type.SeqDate = now.Date;
                        }
                    }
                    break;
                case "PerMonth":
                    {
                        if (type.SeqDate.HasValue && now.Year == type.SeqDate.Value.Year && now.Month == type.SeqDate.Value.Month)
                        {
                            var seq = type.SeqNum.GetValueOrDefault(0) + 1;
                            code = seq.ToString();
                            if (rule.length.HasValue)
                                code = code.PadLeft(rule.length.Value, '0');
                            type.SeqNum = seq;
                        }
                        else
                        {
                            var seq = 1;
                            code = seq.ToString();
                            if (rule.length.HasValue)
                                code = code.PadLeft(rule.length.Value, '0');
                            type.SeqNum = seq;
                            type.SeqDate = now.Date;
                        }
                    };
                    break;
                case "PerYear":
                    {
                        if (type.SeqDate.HasValue && now.Year == type.SeqDate.Value.Year)
                        {
                            var seq = type.SeqNum.GetValueOrDefault(0) + 1;
                            code = seq.ToString();
                            if (rule.length.HasValue)
                                code = code.PadLeft(rule.length.Value, '0');
                            type.SeqNum = seq;
                        }
                        else
                        {
                            var seq = 1;
                            code = seq.ToString();
                            if (rule.length.HasValue)
                                code = code.PadLeft(rule.length.Value, '0');
                            type.SeqNum = seq;
                            type.SeqDate = now.Date;
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