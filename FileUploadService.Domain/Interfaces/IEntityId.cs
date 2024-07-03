using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUploadService.Domain.Interfaces
{
    public interface IEntityId<T> where T : class
    {
       public T Id { get; set; }
    }
}
