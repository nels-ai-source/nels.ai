using Nels.Abp.SysMng.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nels.Abp.SysMng.Files;

public class FileContentTypeProvider
{
    public IDictionary<string, DocumentType> Mappings { get; private set; }
    public FileContentTypeProvider()
    : this(new Dictionary<string, DocumentType>(StringComparer.OrdinalIgnoreCase)
    {
            { ".art", DocumentType.Image },
            { ".bmp",  DocumentType.Image },
            { ".cmx",  DocumentType.Image },
            { ".cod",  DocumentType.Image },
            { ".dib",  DocumentType.Image },
            { ".gif",  DocumentType.Image },
            { ".ico",  DocumentType.Image },
            { ".ief",  DocumentType.Image },
            { ".jfif",  DocumentType.Image },
            { ".jpe",  DocumentType.Image },
            { ".jpeg",  DocumentType.Image },
            { ".jpg",  DocumentType.Image },
            { ".ras",  DocumentType.Image },
            { ".rf",  DocumentType.Image },
            { ".rgb",  DocumentType.Image },
            { ".pbm",  DocumentType.Image },
            { ".png",  DocumentType.Image },
            { ".pnm",  DocumentType.Image },
            { ".pnz",  DocumentType.Image },
            { ".pgm",  DocumentType.Image },
            { ".ppm",  DocumentType.Image },
            { ".svg",  DocumentType.Image },
            { ".svgz",  DocumentType.Image },
            { ".tif",  DocumentType.Image },
            { ".tiff",  DocumentType.Image },
            { ".wbmp",  DocumentType.Image },
            { ".webp",  DocumentType.Image },
            { ".xbm",  DocumentType.Image },
            { ".xpm",  DocumentType.Image },
            { ".xwd",  DocumentType.Image },

            { ".xla", DocumentType.Excel },
            { ".xlam", DocumentType.Excel },
            { ".xlc", DocumentType.Excel },
            { ".xlm", DocumentType.Excel },
            { ".xls", DocumentType.Excel },
            { ".xlsb", DocumentType.Excel },
            { ".xlsm", DocumentType.Excel },
            { ".xlt", DocumentType.Excel },
            { ".xltm", DocumentType.Excel },
            { ".xlw", DocumentType.Excel },
            { ".xlsx", DocumentType.Excel },

            { ".ppt",DocumentType.Powerpoint },
            { ".pptx",DocumentType.Powerpoint },

            { ".doc", DocumentType.Word },
            { ".docx", DocumentType.Word },

            { ".pdf", DocumentType.Pdf },

            { ".markdown", DocumentType.Markdown },
            { ".md", DocumentType.Markdown },

            { ".asm", DocumentType.Text },
            { ".bas", DocumentType.Text },
            { ".c", DocumentType.Text },
            { ".cnf", DocumentType.Text },
            { ".cpp", DocumentType.Text },
            { ".h", DocumentType.Text },
            { ".map", DocumentType.Text },
            { ".txt", DocumentType.Text },
            { ".vcs", DocumentType.Text },
            { ".xdr", DocumentType.Text },


    })
    {
    }

    public FileContentTypeProvider(IDictionary<string, DocumentType> mapping)
    {
        if (mapping == null)
        {
            throw new ArgumentNullException("mapping");
        }

        Mappings = mapping;
    }
}
