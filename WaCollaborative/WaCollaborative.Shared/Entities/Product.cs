using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaCollaborative.Shared.Entities
{
    public class Product
    {
        #region Attributes
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;

        [Display(Name = "Código")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string Code { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "FactorConversión ")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal ConversionFactor { get; set; }

        public Category? Category { get; set; }

        public int CategoryId { get; set; }

        public MeasurementUnit? MeasurementUnit { get; set; }

        public int MeasurementUnitId { get; set; }

        public Segment? Segment { get; set; }

        public int SegmentId { get; set; }

        #endregion
    }
}
