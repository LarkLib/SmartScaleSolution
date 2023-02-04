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
        public string FaceName { get; set; }
        /// <summary>
        /// 身高,单位：厘米
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// 体重，单位：克
        /// </summary>
        public int Weight { get; set; }
        /// <summary>
        /// 建立时间
        /// </summary>
        public long Ctime { get; set; }
    }
}
