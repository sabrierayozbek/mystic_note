using MistikRota.DataAccessLayer.EntityFramework;
using MistikRota.Entites;
using MistikRota.Entites.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MistikRota.BusinessLayer.Abstract;
using MistikRota.BusinessLayer.Results;
using MistikRota.Common.Helper;
using MistikRota.Common.Helpers;
using MistikRota.Entites.Messages;

namespace MistikRota.BusinessLayer
{
    public class MistikRotaUserManager : ManagerBase<MistikRotaUser>
    {
        //(bundan kurtulabiliriz artık)private Repository<MistikRotaUser> repo_user = new Repository<MistikRotaUser>();
        public BusinessLayerResult<MistikRotaUser> RegisterUser (RegisterViewModel data) //Kayıt olan kullanıcıyı dönecek. BusinessLayerResult olarak..
        {
            //Giriş kontrolü ve yönlendirme yap..
            //Ehm, hata bastırma işlemlerini yap. Bunun için kontrol işlemlerini.
            //Session'a kullanıcı kayıt. 
            //Aktivasyon e-postası...

            MistikRotaUser user = Find(x => x.Username == data.Username || x.Email == data.EMail); //Bi kullanıcı nesnesi dönecek bize database'den.
            BusinessLayerResult<MistikRotaUser> layerResult = new BusinessLayerResult<MistikRotaUser>(); //layerResult değişkeni ile artık MistikRotaUser değil, BusinessLayerResult<MistikRotaUser> nesnesi dönüyoruz.
            if (user != null) 
            {
                //peki hangileri eşleşti?
                if (user.Username == data.Username)
                {
                    layerResult.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı."); //BusinessLayerResult'tan gelen hata propertysi
                }

                if (user.Email == data.EMail)
                {
                    layerResult.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }
            }

            else
            {
                //Entitydeki tablomuzdan oluşturduğumuz instance. Eğer eşleşen kayıt yoksa, kullanıcı eklenecek.
               int dbResult = base.Insert(new MistikRotaUser()  //benden generic bi obj ister. 
                {
                    Username = data.Username,
                    Email = data.EMail,
                    Password = data.Password,
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = false,
                    IsAdmin = false
                }); //database insert işlemi

                if (dbResult > 0) //başarlıysa 1 döner insert komutu.
                {
                    layerResult.Result = Find(x => x.Email == data.EMail && x.Username == data.Username); //insert edilen kullanıcı database'den çekilip result nesnesine atılır. 

                    string siteUrl = ConfigHelper.Get<string>("SiteRootUrl");
                    string activeUrl = $"{siteUrl}/Home/UserActivate/{layerResult.Result.ActivateGuid}";
                    string body = $"Merhaba {layerResult.Result.Username};<br><br>Hesabınızı aktifleştirmek için <a href='{activeUrl}' target='_blank'>tıklayınız</a>.";

                    MailHelper.SendMail(body, layerResult.Result.Email, "Mistik Rota Hesap Aktivasyonu");



                    //TODO : aktivasyon mailini hallet!
                    // layerResult.Result.ActivateGuid 
                }

            }

            return layerResult;

        }

        public BusinessLayerResult<MistikRotaUser> LoginUser(LoginViewModel data)
        {
            //giriş kontrolü..
            //hesap aktive edilmiş mi?

          
            BusinessLayerResult<MistikRotaUser> res = new BusinessLayerResult<MistikRotaUser>();
            res.Result = Find(x => x.Username == data.Username && x.Password == data.Password);

           

            if (res.Result != null)
            {
                if (!res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserIsNotActive, "Kullanıcı aktifleştirilmemiştir.");
                    res.AddError(ErrorMessageCode.CheckYourEmail, "Lütfen e-posta adresinizi kontrol ediniz.");

                }
            }

            //Hataların yönetimini de yapacaksın. Hangi hata olduğuna belirlenmesine göre, kullanıcıya o hataya göre yönlendireceksin. Çok dilli bir yapı kullanacağın için
            //bunu direkt businesslayerresulttaki errors property'sinin error indisleri üzerinden hareket etmek yerine, ayrı bir enum oluşturup, her oluşabilecek hatayı orada belirteceksin.
            //yani en azından her bir hatayı eklerken bir de hata kodu gibi bir şey verebiliyor olsan iyi olur. kod değişmez, dilden bağımsızdır, kolayca if ile kontrol edebilirsin.
            


            else
            {
                res.AddError(ErrorMessageCode.UsernameOrPassWrong, "Kullanıcı adı yada şifre uyuşmuyor.");
            }

            return res;
        }

        public BusinessLayerResult<MistikRotaUser> ActivateUser(Guid activateId)
        {
            BusinessLayerResult<MistikRotaUser> res = new BusinessLayerResult<MistikRotaUser>();
            res.Result = Find(x => x.ActivateGuid == activateId);
            if (res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullancı zaten aktif edilmiştir.");
                    return res;
                }

                res.Result.IsActive = true;
                Update(res.Result);
            }

            else
            {
                res.AddError(ErrorMessageCode.UserAlreadyActive, "Aktifleştirelecek kullanıcı bulunamadı.");
            }

            return res;
            
        }

        public BusinessLayerResult<MistikRotaUser> getUserById(int id)
        {
            BusinessLayerResult<MistikRotaUser> res = new BusinessLayerResult<MistikRotaUser>();
            res.Result = Find(x => x.Id == id); //veritabanından gelen id'yi al, get ten gelen id ile karşılaştır ve aynıysa kullanıcı döndürcez.

            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı bulunamadı.");

            }
            return res;
        }

        public BusinessLayerResult<MistikRotaUser> UpdateProfile(MistikRotaUser data)
        {
            MistikRotaUser db_user = Find(x => x.Id != data.Id && (x.Username == data.Username || x.Email == data.Email)); //eğer varolan başka bir kullanıcının e-mailini veya username'ini yazarsa diye yazıldı. yanlışlık set edemesin diye.
            BusinessLayerResult<MistikRotaUser> res = new BusinessLayerResult<MistikRotaUser>();

            if (db_user != null && db_user.Id != data.Id) //database bulduğum kullanıcıyla eşleşiyorsa ve bu kullanıcının id'si update yapan kullanıcının id'sine eşit değilse
            {
                if (db_user.Username == data.Username) 
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }
                

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }

                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;

            if (string.IsNullOrEmpty(data.ProfileImageFileName) == false)
            {
                res.Result.ProfileImageFileName = data.ProfileImageFileName;
            }

            if (base.Update(res.Result) == 0) //0 geldiyse bir şeyler ters gitti, çünkü 1 gelmesi gerekirdi.
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Profil güncellenemedi.");
            }

            return res;
        }

        public BusinessLayerResult<MistikRotaUser> RemoveUserById(int id)
        {
            BusinessLayerResult<MistikRotaUser> res = new BusinessLayerResult<MistikRotaUser>();
            MistikRotaUser user = Find(x => x.Id == id); //kullanıcıyı buluyorum.

            if (user != null)
            {
                if (Delete(user) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı Silinemedi");
                    return res;
                }
            }

            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı bulunamadı.");
            }

            return res;
        }

        // Method hiding..
        public new BusinessLayerResult<MistikRotaUser> Insert(MistikRotaUser data)
        {
            MistikRotaUser user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<MistikRotaUser> res = new BusinessLayerResult<MistikRotaUser>();

            res.Result = data;

            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }
            }
            else
            {
                res.Result.ProfileImageFileName = "user_default.png";
                res.Result.ActivateGuid = Guid.NewGuid();

                if (base.Insert(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotInserted, "Kullanıcı eklenemedi.");
                }
            }

            return res;
        }

        public new BusinessLayerResult<MistikRotaUser> Update(MistikRotaUser data)
        {
            MistikRotaUser db_user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<MistikRotaUser> res = new BusinessLayerResult<MistikRotaUser>();
            res.Result = data;

            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }

                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;
            res.Result.IsActive = data.IsActive;
            res.Result.IsAdmin = data.IsAdmin;

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdated, "Kullanıcı güncellenemedi.");
            }

            return res;
           
        }

    }
}
