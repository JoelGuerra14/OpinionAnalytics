using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpinionAnalytics.Domain.Entities.Api
{
    [Keyless]
    [Table("SocialComments")]
    public class Comment
    {
        public string? IdComment { get; set; }

        public string? IdCliente { get; set; }

        public string? IdProducto { get; set; }

        public string? Fuente { get; set; }

        public DateTime? Fecha { get; set; }

        public string? Comentario { get; set; }
    }
}
