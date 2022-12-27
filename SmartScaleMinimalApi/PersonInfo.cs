namespace SmartScaleMinimalApi
{
    /// <summary>
    /// 个人信息
    /// </summary>
    public class PersonInfo
    {
        /// <summary>
        /// 数据库编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 人脸编号
        /// </summary>
        public string FaceNo { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public string Age { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 是否戴帽子
        /// </summary>
        public string Hat { get; set; }
        /// <summary>
        /// 是否戴眼镜
        /// </summary>
        public string Glass { get; set; }
        /// <summary>
        /// 颜值
        /// </summary>
        public string Beauty { get; set; }
    }
}
