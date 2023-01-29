using SmartScaleMinimalAPI;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;
//using System.web.Helpers;
//using System.Drawing;

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

            //builder.Services.AddDbContext<SmartScaleDb>(opt => opt.UseInMemoryDatabase("ScaleList"));
            builder.Services.AddDbContext<SmartScaleDb>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("WebApiDatabase")));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });
            var app = builder.Build();

            //if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("CorsPolicy");

            feedData(app);

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
            {
                return Convert.ToBase64String(File.ReadAllBytes("image.png"));
                //Convert.ToBase64String(Encoding.UTF8.GetBytes("base64 string")));
            });

            app.MapGet("/api/GetPersonInfoByfaceNo/{gatewayId}/{faceNo}",
            [SwaggerOperation(Summary = "��ѯ��������", Description = "����faceNo��ѯ����,���ض�Ӧ��������")]
            async ([SwaggerParameter("�豸���", Required = true)] string gatewayId, [SwaggerParameter("�������", Required = true)] string faceNo, SmartScaleDb db) =>
            //await db.PersonInfos.ToListAsync());
            //db.PersonInfos.Select(p => p.FaceNo == faceNo));
            (from p in db.PersonInfos where p.FaceNo == faceNo select p).FirstOrDefault());


            app.MapGet("/api/GetUpdatePersonInfoUrl/",
            [SwaggerOperation(Summary = "�޸ĸ�����Ϣ����", Description = "�����޸ĸ�����Ϣҳ������")]
            ([SwaggerParameter("�豸���", Required = true)] string gatewayId) =>
            "http://192.168.1.220:8090/UpdatePersonInfo");


            app.MapPost("/api/UpdatePersonInfo/", [SwaggerOperation(Summary = "�����û�����", Description = "�����û����ݵ����ݿ�")]
            [SwaggerResponse(200, "PersonInfo", typeof(PersonInfo))]
            async ([SwaggerParameter("�û���Ϣ", Required = true)] PersonInfo personInfo, SmartScaleDb db) =>
            {
                var persion = db.PersonInfos.Single(info => info.Id == personInfo.Id);
                persion.Name = personInfo.Name;
                persion.Age = personInfo.Age;
                persion.Beauty = personInfo.Beauty;
                persion.Gender = personInfo.Gender;
                persion.Ctime = personInfo.Ctime;
                persion.FaceNo = personInfo.FaceNo;
                persion.GatewayId = personInfo.GatewayId;
                persion.Glass = personInfo.Glass;
                persion.Hat = personInfo.Hat;

                db.PersonInfos.Update(persion);
                await db.SaveChangesAsync();

                return Results.Created($"/api/PersonInfos/{personInfo.Id}", personInfo);
            });

            app.MapGet("/api/GetFullFaceDb/{gatewayId}",
            [SwaggerOperation(Summary = "����������", Description = "����������ȫ�����ݣ�ÿ���������500��������500����������")]
            async ([SwaggerParameter("�豸���", Required = true)] string gatewayId, [SwaggerParameter("���κ�", Required = false)] int batchNo, SmartScaleDb db) =>
            await db.FaceEigens.ToListAsync());

            app.MapPost("/api/GetFaceDbByFaceNoList/{faceNos}",
            [SwaggerOperation(Summary = "����������", Description = "������������б������������ݣ�ÿ���������500��������500����������")]
            async ([SwaggerParameter("�豸���", Required = true)] string gatewayId, [SwaggerParameter("��������б�", Required = true)] List<string> faceNos, [SwaggerParameter("���κ�", Required = false)] int batchNo, SmartScaleDb db) =>
            await db.FaceEigens.ToListAsync());


            app.MapGet("/api/ScaleItems/{id}", [SwaggerOperation(Summary = "��ѯ���ݿⵥ������", Description = "�����ݿ�ID��ѯ����")]
            [SwaggerResponse(200, "scaleitems", typeof(SmartScale))]
            async ([SwaggerParameter("���ݿ�id", Required = true)] int id, SmartScaleDb db) =>
            await db.Scales.FindAsync(id)
                is SmartScale scale
                    ? Results.Ok(scale)
                    : Results.NotFound());

            app.MapPost("/api/ScaleItems", [SwaggerOperation(Summary = "�����������", Description = "����������ݵ����ݿ�")]
            [SwaggerResponse(200, "ScaleItems", typeof(SmartScale))]
            async ([SwaggerParameter("��������", Required = true)] SmartScale scale, SmartScaleDb db) =>
            {
                db.Scales.Add(scale);
                await db.SaveChangesAsync();

                return Results.Created($"/api/ScaleItems/{scale.Id}", scale);
            });

            app.MapGet("/api/PersonInfos/{id}", [SwaggerOperation(Summary = "��ѯ�û����ݵ�������", Description = "�����ݿ�ID��ѯ����")]
            [SwaggerResponse(200, "PersonInfos", typeof(PersonInfo))]
            async ([SwaggerParameter("���ݿ�id", Required = true)] int id, SmartScaleDb db) =>
            await db.PersonInfos.FindAsync(id)
                is PersonInfo personInfo
                    ? Results.Ok(personInfo)
                    : Results.NotFound());

            app.MapPost("/api/PersonInfos", [SwaggerOperation(Summary = "�����û�����", Description = "�����û����ݵ����ݿ�")]
            [SwaggerResponse(200, "PersonInfos", typeof(PersonInfo))]
            async ([SwaggerParameter("�û���Ϣ", Required = true)] PersonInfo personInfo, SmartScaleDb db) =>
            {
                db.PersonInfos.Add(personInfo);
                await db.SaveChangesAsync();

                return Results.Created($"/PersonInfos/{personInfo.Id}", personInfo);
            });

            app.Run();
        }

        private static void feedData(WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetService<SmartScaleDb>();
            if (db == null) return;

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.PersonInfos.Add(new PersonInfo() { GatewayId = "HZKkp6eOKZL8W3iPzADj", Age = 19, FaceNo = "12", Beauty = 85, Gender = "Man", Glass = false, Hat = false, Name = "����", Id = 1 });
            db.PersonInfos.Add(new PersonInfo() { GatewayId = "HZKkp6eOKZL8W3iPzADj", FaceNo = "13", Id = 2 });
            db.Scales.Add(new SmartScale() { GatewayId = "HZKkp6eOKZL8W3iPzADj", Ctime = 1672275733, FaceName = "����", FaceNo = "12", Height = 170, Id = 1, Weight = 65000 });
            db.Scales.Add(new SmartScale() { GatewayId = "HZKkp6eOKZL8W3iPzADj", Ctime = 1672275899, FaceName = "����", FaceNo = "12", Height = 170, Id = 2, Weight = 65230 });
            db.SaveChanges();
        }
    }
}
