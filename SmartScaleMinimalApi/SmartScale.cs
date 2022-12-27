namespace SmartScaleMinimalAPI
{
    /// <summary>
    /// 测量数据
    /// </summary>
    class SmartScale
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
        public string FaceName { get; set; }
        /// <summary>
        /// 身高
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// 体重
        /// </summary>
        public int Weight { get; set; }
        /// <summary>
        /// 建立时间
        /// </summary>
        public int Ctime { get; set; }
    }
}
