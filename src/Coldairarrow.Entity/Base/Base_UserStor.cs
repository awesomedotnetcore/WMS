using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.Base
{
    /// <summary>
    /// 用户仓库权限表
    /// </summary>
    [Table("Base_UserStor")]
    public class Base_UserStor
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public String UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public Base_Manage.Base_User User { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public String StorId { get; set; }

        [ForeignKey(nameof(StorId))]
        public PB.PB_Storage Storage { get; set; }
        /// <summary>
        /// 是否默认仓库
        /// </summary>
        public Boolean IsDefault { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        public String CreatorId { get; set; }

        /// <summary>
        /// 删除状态：0=未删除；1＝已删除；
        /// </summary>
        public Boolean Deleted { get; set; }

    }
}