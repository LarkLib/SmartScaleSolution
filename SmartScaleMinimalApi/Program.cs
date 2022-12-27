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
            [SwaggerOperation(Summary = "��ѯ������ʷ����", Description = "����faceNo��ѯ����,���ض�Ӧ��������")]
            async (
                [SwaggerParameter("�豸���", Required = true)] string gatewayId,
                [SwaggerParameter("�������", Required = true)] string faceNo,
                [SwaggerParameter("��ʼʱ��", Required = true)] string startTime,
                [SwaggerParameter("����ʱ��")] string endTime, SmartScaleDb db) =>
            await db.Scales.ToListAsync());

            app.MapPost("/api/GetTelemetryImageByfaceNo/{faceNo}",
            [SwaggerOperation(Summary = "��ѯ������ʷ����", Description = "����faceNo��ѯ����,���ض�ӦͼƬ")]
            (
                [SwaggerParameter("�豸���", Required = true)] string gatewayId,
                [SwaggerParameter("�������", Required = true)] string faceNo,
                [SwaggerParameter("��ʼʱ��", Required = true)] string startTime,
                [SwaggerParameter("����ʱ��")] string endTime, SmartScaleDb db) =>
            Convert.ToBase64String(Encoding.UTF8.GetBytes("base64 string")));

            app.MapGet("/api/GetPersonInfoByfaceNo/{gatewayId}/{faceNo}",
            [SwaggerOperation(Summary = "��ѯ��������", Description = "����faceNo��ѯ����,���ض�Ӧ��������")]
            async ([SwaggerParameter("�豸���", Required = true)] string gatewayId, [SwaggerParameter("�������", Required = true)] string faceNo, SmartScaleDb db) =>
            await db.PersonInfos.ToListAsync());

            app.MapGet("/api/GetFullFaceDb/{gatewayId}",
            [SwaggerOperation(Summary = "����������", Description = "����������ȫ�����ݣ�ÿ���������500��������500����������")]
            async ([SwaggerParameter("�豸���", Required = true)] string gatewayId, [SwaggerParameter("���κ�", Required = false)] int batchNo, SmartScaleDb db) =>
            await db.FaceEigens.ToListAsync());

            app.MapPost("/api/GetFaceDbByFaceNoList/{faceNos}",
            [SwaggerOperation(Summary = "����������", Description = "������������б������������ݣ�ÿ���������500��������500����������")]
            async ([SwaggerParameter("�豸���", Required = true)] string gatewayId, [SwaggerParameter("��������б�", Required = true)] List<string> faceNos, [SwaggerParameter("���κ�", Required = false)] int batchNo, SmartScaleDb db) =>
            await db.FaceEigens.ToListAsync());

            app.Run();
        }
    }
}
