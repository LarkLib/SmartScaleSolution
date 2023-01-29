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
            {
                return Convert.ToBase64String(File.ReadAllBytes("image.png"));
                //Convert.ToBase64String(Encoding.UTF8.GetBytes("base64 string")));
            });

            app.MapGet("/api/GetPersonInfoByfaceNo/{gatewayId}/{faceNo}",
            [SwaggerOperation(Summary = "查询个人数据", Description = "根据faceNo查询数据,返回对应个人数据")]
            async ([SwaggerParameter("设备编号", Required = true)] string gatewayId, [SwaggerParameter("人脸编号", Required = true)] string faceNo, SmartScaleDb db) =>
            //await db.PersonInfos.ToListAsync());
            //db.PersonInfos.Select(p => p.FaceNo == faceNo));
            (from p in db.PersonInfos where p.FaceNo == faceNo select p).FirstOrDefault());


            app.MapGet("/api/GetUpdatePersonInfoUrl/",
            [SwaggerOperation(Summary = "修改个人信息链接", Description = "返回修改个人信息页面链接")]
            ([SwaggerParameter("设备编号", Required = true)] string gatewayId) =>
            "http://192.168.1.220:8090/UpdatePersonInfo");


            app.MapPost("/api/UpdatePersonInfo/", [SwaggerOperation(Summary = "更改用户数据", Description = "更改用户数据到数据库")]
            [SwaggerResponse(200, "PersonInfo", typeof(PersonInfo))]
            async ([SwaggerParameter("用户信息", Required = true)] PersonInfo personInfo, SmartScaleDb db) =>
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
            [SwaggerOperation(Summary = "下载人脸库", Description = "返回人脸库全部数据，每批数据最多500条，超过500条分批传送")]
            async ([SwaggerParameter("设备编号", Required = true)] string gatewayId, [SwaggerParameter("批次号", Required = false)] int batchNo, SmartScaleDb db) =>
            await db.FaceEigens.ToListAsync());

            app.MapPost("/api/GetFaceDbByFaceNoList/{faceNos}",
            [SwaggerOperation(Summary = "下载人脸库", Description = "根据人脸编号列表返回人脸库数据，每批数据最多500条，超过500条分批传送")]
            async ([SwaggerParameter("设备编号", Required = true)] string gatewayId, [SwaggerParameter("人脸编号列表", Required = true)] List<string> faceNos, [SwaggerParameter("批次号", Required = false)] int batchNo, SmartScaleDb db) =>
            await db.FaceEigens.ToListAsync());


            app.MapGet("/api/ScaleItems/{id}", [SwaggerOperation(Summary = "查询数据库单条数据", Description = "按数据库ID查询数据")]
            [SwaggerResponse(200, "scaleitems", typeof(SmartScale))]
            async ([SwaggerParameter("数据库id", Required = true)] int id, SmartScaleDb db) =>
            await db.Scales.FindAsync(id)
                is SmartScale scale
                    ? Results.Ok(scale)
                    : Results.NotFound());

            app.MapPost("/api/ScaleItems", [SwaggerOperation(Summary = "保存测量数据", Description = "保存测量数据到数据库")]
            [SwaggerResponse(200, "ScaleItems", typeof(SmartScale))]
            async ([SwaggerParameter("测量数据", Required = true)] SmartScale scale, SmartScaleDb db) =>
            {
                db.Scales.Add(scale);
                await db.SaveChangesAsync();

                return Results.Created($"/api/ScaleItems/{scale.Id}", scale);
            });

            app.MapGet("/api/PersonInfos/{id}", [SwaggerOperation(Summary = "查询用户数据单条数据", Description = "按数据库ID查询数据")]
            [SwaggerResponse(200, "PersonInfos", typeof(PersonInfo))]
            async ([SwaggerParameter("数据库id", Required = true)] int id, SmartScaleDb db) =>
            await db.PersonInfos.FindAsync(id)
                is PersonInfo personInfo
                    ? Results.Ok(personInfo)
                    : Results.NotFound());

            app.MapPost("/api/PersonInfos", [SwaggerOperation(Summary = "保存用户数据", Description = "保存用户数据到数据库")]
            [SwaggerResponse(200, "PersonInfos", typeof(PersonInfo))]
            async ([SwaggerParameter("用户信息", Required = true)] PersonInfo personInfo, SmartScaleDb db) =>
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

            db.PersonInfos.Add(new PersonInfo() { GatewayId = "HZKkp6eOKZL8W3iPzADj", Age = 19, FaceNo = "12", Beauty = 85, Gender = "Man", Glass = false, Hat = false, Name = "张三", Id = 1 });
            db.PersonInfos.Add(new PersonInfo() { GatewayId = "HZKkp6eOKZL8W3iPzADj", FaceNo = "13", Id = 2 });
            db.Scales.Add(new SmartScale() { GatewayId = "HZKkp6eOKZL8W3iPzADj", Ctime = 1672275733, FaceName = "张三", FaceNo = "12", Height = 170, Id = 1, Weight = 65000 });
            db.Scales.Add(new SmartScale() { GatewayId = "HZKkp6eOKZL8W3iPzADj", Ctime = 1672275899, FaceName = "张三", FaceNo = "12", Height = 170, Id = 2, Weight = 65230 });
            db.SaveChanges();
        }
    }
}
