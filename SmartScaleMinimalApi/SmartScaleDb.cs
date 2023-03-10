using Microsoft.EntityFrameworkCore;
using SmartScaleMinimalApi;
using System.Reflection.Metadata;

namespace SmartScaleMinimalAPI
{

    class SmartScaleDb : DbContext
    {
        public SmartScaleDb(DbContextOptions<SmartScaleDb> options)
            : base(options) { }

        public DbSet<SmartScale> Scales => Set<SmartScale>();
        public DbSet<PersonInfo> PersonInfos => Set<PersonInfo>();
        public DbSet<FaceEigen> FaceEigens => Set<FaceEigen>();
        public DbSet<FaceImage> FaceImages => Set<FaceImage>();

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<PersonInfo>()
        //        .HasIndex(b => b.FaceNo)
        //        .IsUnique();
        //}
    }
}
