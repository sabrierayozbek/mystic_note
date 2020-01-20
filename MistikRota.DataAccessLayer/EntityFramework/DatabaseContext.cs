using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MistikRota.Entites;

namespace MistikRota.DataAccessLayer.EntityFramework
{
    public class DatabaseContext : DbContext //Entity Framework palketiyle gelen DbContext isimli sınıftan türettim.
    {
        //Veritabanındaki tablolarımıza karşılık gleen DBsetler. 
        public DbSet<MistikRotaUser> MistikRotaUsers { get; set; }
        public DbSet<Story> Stories { get; set; } 
        public DbSet<Category> Categories { get; set; }
        public DbSet<Liked> Likes { get; set; }

        //oluşturduğun initializer'ımın set edilmesi lazım. constructor içinde database.setinitailzer diye bir method var
        //bu methodun için idatabaseinitalizer interface'inden türeyen bir class ister. myinitializirimi ona veriyorum.
        public DatabaseContext()
        {
            Database.SetInitializer(new MyInitializer()); 
        }
    }

           
}
