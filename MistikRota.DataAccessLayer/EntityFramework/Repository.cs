
using MistikRota.Entites;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MistikRota.Common;
using MistikRota.Core.DataAcces;
using MistikRota.DataAccessLayer.EntityFramework;

namespace MistikRota.DataAccessLayer.EntityFramework
{
    public class Repository<T> : RepositoryBase, IDataAcces<T> where T : class //yanlış bir tipe set işlemi yapmaması için bir T'ye bir kısıtlama koyuyoruz. yani class ve newlenen bir tip olmalıdır diyoruz. 
    {
        private DbSet<T> _objectSet;

        public Repository()
        {
            _objectSet = context.Set<T>(); //her methodun altında set etmemek için, ctor'da bir kere set edip değişkene atadık. o değişken üzerinde işlemleri yapacağız.
        }


        public List<T> List()
        {
            return _objectSet.ToList();
        }

        public IQueryable<T> ListQueryable() //list yerine başka bir sorgu dönmek istediğim zaman kullanacağım method. 
            //bu methodda istediğim sorguyu yazıp tolist'e çevirmeden uygulayabilirim.
        {
            return _objectSet.AsQueryable<T>();
        }

        public List<T> List(Expression<Func<T, bool>> where) //bu lambda ifade kullanarak istediim kritere göre listelemem için gerekli olan bir method. 
            //objectset.where dediğim zaman benden expression ister, t tipinde ve bool dönen. 
        {
            return _objectSet.Where(where).ToList();
        }

        public int Insert(T obj)
        {
            _objectSet.Add(obj);

            if (obj is MyEntityBase) //Bize gelen obje bir MyEntity base ise (diğer tablolarda miras ald. için)
            {
                MyEntityBase o = obj as MyEntityBase; //Bu cast işlemi sadece myentitybase içindeki prorpertyleri kullanmak için
                DateTime now = DateTime.Now;

                o.CreatedOn = now;
                o.ModifiedDate = now;
                o.ModifiedUsername = App.common.GetCurrentUsername();
            }

            return Save();
        }

        public int Update(T obj)
        {
            if (obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;

                o.ModifiedDate = DateTime.Now;
                o.ModifiedUsername = App.common.GetCurrentUsername();
            }

            return Save();
        }

        public int Delete(T obj)
        {
            //if (obj is MyEntityBase)
            //{
            //    MyEntityBase o = obj as MyEntityBase;

            //    o.ModifiedOn = DateTime.Now;
            //    o.ModifiedUsername = App.Common.GetUsername();
            //}

            _objectSet.Remove(obj);
            return Save();
        }

        public int Save()
        {
            return context.SaveChanges();
        }

        public T Find(Expression<Func<T, bool>> where) //tek bir tipi döndüren method. verilen koşula uygun nesne bulunup geri döner. 
        {
            return _objectSet.FirstOrDefault(where);
        }
    }
}