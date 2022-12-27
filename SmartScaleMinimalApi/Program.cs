using SmartScaleMinimalAPI;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Text;

namespace SmartScaleMinimalApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), true);
                options.EnableAnnotations();
            });

            builder.Services.AddDbContext<SmartScaleDb>(opt => opt.UseInMemoryDatabase("ScaleList"));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapGet("/", () => "SmartScaleMinimalApi");

            app.MapPost("/api/GetTelemetryByfaceNo/{faceNo}",
            [SwaggerOperation(Summary = "查询个人历史数据", Description = "根据faceNo查询数据,返回对应测量数据")]
            async (
                [SwaggerParameter("设备编号", Required = true)] string gatewayId,
                [SwaggerParameter("人脸编号", Required = true)] string faceNo,
                [SwaggerParameter("起始时间", Required = true)] string startTime,
                [SwaggerParameter("结束时间")] string endTime, SmartScaleDb db) =>
            await db.Scales.ToListAsync());

            app.MapPost("/api/GetTelemetryImageByfaceNo/{faceNo}",
            [SwaggerOperation(Summary = "查询个人历史数据", Description = "根据faceNo查询数据,返回对应图片")]
            (
                [SwaggerParameter("设备编号", Required = true)] string gatewayId,
                [SwaggerParameter("人脸编号", Required = true)] string faceNo,
                [SwaggerParameter("起始时间", Required = true)] string startTime,
                [SwaggerParameter("结束时间")] string endTime, SmartScaleDb db) =>
            Convert.ToBase64String(Encoding.UTF8.GetBytes("base64 string")));

            app.MapGet("/api/GetPersonInfoByfaceNo/{gatewayId}/{faceNo}",
            [SwaggerOperation(Summary = "查询个人数据", Description = "根据faceNo查询数据,返回对应个人数据")]
            async ([SwaggerParameter("设备编号", Required = true)] string gatewayId, [SwaggerParameter("人脸编号", Required = true)] string faceNo, SmartScaleDb db) =>
            await db.PersonInfos.ToListAsync());

            app.MapGet("/api/GetFullFaceDb/{gatewayId}",
            [SwaggerOperation(Summary = "下载人脸库", Description = "返回人脸库全部数据，每批数据最多500条，超过500条分批传送")]
            async ([SwaggerParameter("设备编号", Required = true)] string gatewayId, [SwaggerParameter("批次号", Required = false)] int batchNo, SmartScaleDb db) =>
            await db.FaceEigens.ToListAsync());

            app.MapPost("/api/GetFaceDbByFaceNoList/{faceNos}",
            [SwaggerOperation(Summary = "下载人脸库", Description = "根据人脸编号列表返回人脸库数据，每批数据最多500条，超过500条分批传送")]
            async ([SwaggerParameter("设备编号", Required = true)] string gatewayId, [SwaggerParameter("人脸编号列表", Required = true)] List<string> faceNos, [SwaggerParameter("批次号", Required = false)] int batchNo, SmartScaleDb db) =>
            await db.FaceEigens.ToListAsync());

            app.Run();
        }
    }
}
