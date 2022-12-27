namespace SmartScaleMinimalApi
{
    /// <summary>
    /// 人脸特征
    /// </summary>
    public class FaceEigen
    {
        /// <summary>
        /// 数据库自动编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 人脸编号
        /// </summary>
        public string FaceNo { get; set; }
        /// <summary>
        /// 人脸特征值
        /// </summary>
        public string Eigen { get; set; }

    }
}
