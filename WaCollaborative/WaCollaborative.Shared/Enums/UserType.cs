#region Using

using System.ComponentModel;

#endregion Using

namespace WaCollaborative.Shared.Enums
{
    public enum UserType
    {
        [Description("Planeador")]
        Planner,

        [Description("Colaborador")]
        Collaborator       
    }
}