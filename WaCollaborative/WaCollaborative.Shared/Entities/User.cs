﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using WaCollaborative.Shared.Enums;

namespace WaCollaborative.Shared.Entities
{
    public class User : IdentityUser
    {
        [Display(Name = "Documento")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Document { get; set; } = null!;

        [Display(Name = "Nombres")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Apellidos")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string LastName { get; set; } = null!;

        [Display(Name = "Dirección")]
        [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Address { get; set; } = null!;

        [Display(Name = "Foto")]
        public string? Photo { get; set; }

        [Display(Name = "Tipo de usuario")]
        public UserType UserType { get; set; }

        public City? City { get; set; }

        [Display(Name = "Ciudad")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una {0}.")]
        public int CityId { get; set; }

        [Display(Name = "Usuario")]
        public string FullName => $"{FirstName} {LastName}";

        public InternalRole? InternalRole { get; set; }

        [Display(Name = "Rol")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una {0}.")]
        public int? InternalRoleId { get; set; }

        public Portfolio? Portfolio { get; set; }

        [Display(Name = "Portafolio")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una {0}.")]
        public int? PortfolioId { get; set; }        
        public ICollection<CollaborativeDemandComponentsDetail>? CollaborativeDemandComponentsDetail { get; set; }

        public ICollection<UserCollaborativeDemand>? UserCollaborativeDemands { get; set; }

        public int UserCollaborativeDemandsNumber => UserCollaborativeDemands == null ? 0 : UserCollaborativeDemands.Count;

        public ICollection<CollaborativeDemandUsers>? CollaborativeDemandUsers { get; set; }

        public int CollaborativeDemandUsersNumber => CollaborativeDemandUsers == null ? 0 : CollaborativeDemandUsers.Count;
    }
}