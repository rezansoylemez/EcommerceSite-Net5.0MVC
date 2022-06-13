using RamenCo.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamenCo.Data.Repositories.Abstract
{
    interface IUserRepository : IGenericRepository<UserDetail>
    {
        IEnumerable<UserDetail> GetAllIncludeArticle();
        UserDetail GetArticleByArticleID(int _articleID);
        IEnumerable<UserDetail> GetArticlesByUserID(int _articleID);
        IEnumerable<UserDetail> GetMostViewsArticle();
    }
}
