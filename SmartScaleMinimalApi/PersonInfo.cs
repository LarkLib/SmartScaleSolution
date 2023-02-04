using Microsoft.EntityFrameworkCore;
using System;

namespace SmartScaleMinimalApi
{
    /// <summary>
    /// 个人信息
    /// </summary>
    [Index(nameof(FaceNo), IsUnique = true)]
    public class PersonInfo
    {
        /// <summary>
        /// 数据库编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 终端设备编号
        /// </summary>
        public string GatewayId { get; set; }
        /// <summary>
        /// 人脸编号
        /// </summary>
        public string FaceNo { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int? Age { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string? Gender { get; set; }
        /// <summary>
        /// 是否戴帽子
        /// </summary>
        //public bool? Hat { get; set; }
        /// <summary>
        /// 是否戴眼镜
        /// </summary>
        //public bool? Glass { get; set; }
        /// <summary>
        /// 颜值
        /// </summary>
        public float? Beauty { get; set; }
        /// <summary>
        /// 建立时间
        /// </summary>
        public long Ctime { get; set; }

    }
}
