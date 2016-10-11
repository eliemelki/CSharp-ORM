using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBLibrary.Repository.Views
{
    public interface ViewRepository<T> : Repository<T> where T : class, new() 
    {
    }
}