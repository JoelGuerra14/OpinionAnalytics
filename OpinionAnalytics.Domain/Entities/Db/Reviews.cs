using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpinionAnalytics.Domain.Entities.Dw
{
    [Keyless]
    [Table("WebReviews")]
    public class Reviews
    {
        public string IdReview { get; set; }

        public string IdCliente { get; set; }

        public string IdProducto { get; set; }

        public DateTime? Fecha { get; set; }

        public string Comentario { get; set; }

        public int? Rating { get; set; }
    }
}
