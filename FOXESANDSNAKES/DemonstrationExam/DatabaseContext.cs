using System.Data.Entity;

namespace DemonstrationExam
{
    public class DatabaseContext : DbContext
    {
        /// <summary>
        /// Публичный класс для тонкой работы с базой данных, ссылка на которую передается через name=[ADO EDM entities]
        /// Содержит публичные ссылки на таблицы для удобного обращения и работой с ними
        /// </summary>

        public DatabaseContext() : base("name=ExamDB_2Entities")
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }

        public virtual DbSet<Workshops> Workshops { get; set; }
        public virtual DbSet<ProductWorkshops> ProductWorkshops { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<ProductType> ProductTypes { get; set; }
        public virtual DbSet<MaterialType> MaterialTypes { get; set; }
    }
}
