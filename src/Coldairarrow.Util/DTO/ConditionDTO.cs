namespace Coldairarrow.Util
{
    /// <summary>
    /// 通用条件查询DTO
    /// </summary>
    public class ConditionDTO
    {
        public string Condition { get; set; }
        public string Keyword { get; set; }

        public string StorageId { set; get; }

        public string AreaType { set; get; }
    }
}
