using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamenCo.Data.Repositories.Abstract
{
    interface IUserRepository : IGenericRepository<User>
    {
        IEnumerable<User> GetAllIncludeArticle();
        User GetArticleByArticleID(int _articleID);
        IEnumerable<User> GetArticlesByUserID(int _articleID);
        IEnumerable<User> GetMostViewsArticle();
    }
}
