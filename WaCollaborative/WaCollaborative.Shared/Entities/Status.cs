﻿#region Using

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion Using

namespace WaCollaborative.Shared.Entities
{
    /// <summary>
    /// The class entity Status
    /// </summary>

    [Table("Status")]
    public class Status
    {

        #region Attributes

        public int Id { get; set; }

        [Display(Name = "Estado")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;

        public int StatusTypeId { get; set; }

        public StatusType? StatusType { get; set; }

        #endregion Attributes

    }
}