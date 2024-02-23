using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verba.Stock.Domain.ModelsForElastic.Entities.Files;

public class File
{
    public string? Guid { get; set; }

    public string? Filename { get; set; }

    public string? FileBucket { get; set; }
}
