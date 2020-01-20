using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MistikRota.Entites;

namespace MistikRota.DataAccessLayer.EntityFramework
{
    //Test classım. Veritabanı inşaa edilirken kullanılacak datalar. İçerisinde FakeData isimli dll'ler var. Hızlıca veri oluşturmam için. 
    public class MyInitializer : CreateDatabaseIfNotExists<DatabaseContext> //Ne zaman çalışacak? //Database yok ise çalışacak.
    {
        //override edeceğimiz iki method var. biri *initialize database*, database oluşurken, diğeri de seed database oluştuktan sonra örnek data basımında
        //kullanılan method. burada örnek data basıyoruz.
        //miras aldığım sınıflar idatabaseinitalizer'dan implement olmuştur.
        protected override void Seed(DatabaseContext context)
        {
            // Adding admin user..
            MistikRotaUser admin = new MistikRotaUser()
        {
            Name = "Eray",
            Surname = "Özbek",
            Email = "erayozbek95@gmail.com",
            ActivateGuid = Guid.NewGuid(),
            IsActive = true,
            IsAdmin = true,
            Username = "erayozbek",
            ProfileImageFileName = "user_default.png",
            Password = "123456",
            CreatedOn = DateTime.Now,
            ModifiedDate = DateTime.Now.AddMinutes(5),
            ModifiedUsername = "erayozbek"
        };

        // Adding standart user..
        MistikRotaUser standartUser = new MistikRotaUser()
        {
            Name = "Sabri",
            Surname = "Özbek",
            Email = "sabri.ozbek@sakarya.edu.tr",
            ActivateGuid = Guid.NewGuid(),
            IsActive = true,
            IsAdmin = false,
            Username = "sabriozbek",
            Password = "654321",
            ProfileImageFileName = "user_default.png",
            CreatedOn = DateTime.Now.AddHours(1),
            ModifiedDate = DateTime.Now.AddMinutes(65),
            ModifiedUsername = "sabriozbek"
        };

        context.MistikRotaUsers.Add(admin);
            context.MistikRotaUsers.Add(standartUser);

            for (int i = 0; i< 8; i++)
            {
                MistikRotaUser user = new MistikRotaUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ProfileImageFileName = "user_default.png",
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    Username = $"user{i}", //dolar işareti ile string bir ifadenin içine istediğim csharp ifadesini kullanabiliyorum.
                    Password = "123",
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUsername = $"user{i}"
                };

        context.MistikRotaUsers.Add(user);
            }

    context.SaveChanges();

            //userlist for user
            List<MistikRotaUser> userlist = context.MistikRotaUsers.ToList();

            List<string> categories = new List<string>(new string[]{"Fantastik", "Bilim Kurgu", "Polisiye", "Felsefi", "Distopya/Ütopya"});

            // Adding fake categories..
            for (int i = 0; i< 5; i++)
            {
                Category cat = new Category()
                {
                    Title = categories[i],
                    Description = FakeData.PlaceData.GetAddress(),
                    CreatedOn = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    ModifiedUsername = "erayozbek"
                };

    context.Categories.Add(cat);

                // Adding fake story..
                
              

                for (int k = 0; k<FakeData.NumberData.GetNumber(5, 9); k++)
                {
                    MistikRotaUser owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];

Story story = new Story()
    {
        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
        Content = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(3, 5)),
        IsDraft = false,
        LikeCount = FakeData.NumberData.GetNumber(1, 9),
        Owner = owner,
        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
        ModifiedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
        ModifiedUsername = owner.Username
    };

    cat.Stories.Add(story);

                    // Adding fake comments
                    for (int j = 0; j<FakeData.NumberData.GetNumber(3, 5); j++)
                    {

                        MistikRotaUser comment_owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];
                        Comment comment = new Comment()
    {
        Text = FakeData.TextData.GetSentence(),
        Owner = comment_owner,
        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
        ModifiedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
        ModifiedUsername = comment_owner.Username
    };

    story.Comments.Add(comment);
                    }

                    // Adding fake likes..
                    
                    for (int m = 0; m<story.LikeCount; m++)
                    {
                        Liked liked = new Liked()
                        {
                            LikedUser = userlist[m]
                        };

story.Likes.Add(liked);
                    }

                }

            }

            context.SaveChanges();
        }
    }
}
