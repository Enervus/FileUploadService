using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUploadService.Domain.Interfaces
{
    public interface IAuditable
    {
      public DateTime CreatedAt { get; set; }
      public string CreatedBy { get; set; }
    }
}
