using Microsoft.EntityFrameworkCore;
using SmartScaleMinimalApi;

namespace SmartScaleMinimalAPI
{

    class SmartScaleDb : DbContext
    {
        public SmartScaleDb(DbContextOptions<SmartScaleDb> options)
            : base(options) { }

        public DbSet<SmartScale> Scales => Set<SmartScale>();
        public DbSet<PersonInfo> PersonInfos => Set<PersonInfo>();
        public DbSet<FaceEigen> FaceEigens => Set<FaceEigen>();
    }
}
