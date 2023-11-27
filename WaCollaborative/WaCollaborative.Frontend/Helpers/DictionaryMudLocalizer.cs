using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace WaCollaborative.Frontend.Helpers
{
    internal class DictionaryMudLocalizer : MudLocalizer
    {
        private Dictionary<string, string> _localization;

        public DictionaryMudLocalizer()
        {
            _localization = new()
        {
            { "MudDataGrid.AddFilter", "Adicionar filtro" },
            { "MudDataGrid.Apply", "Aplicar" },
            { "MudDataGrid.Cancel", "Cancelar" },
            { "MudDataGrid.Clear", "Limpiar" },
            { "MudDataGrid.CollapseAllGroups", "Contraer todos los grupos" },
            { "MudDataGrid.Column", "Columna" },
            { "MudDataGrid.Columns", "Columnas" },
            { "MudDataGrid.contains", "Contiene" },
            { "MudDataGrid.ends with", "Termina con" },
            { "MudDataGrid.equals", "Igual" },
            { "MudDataGrid.ExpandAllGroups", "Expandir todos los grupos" },
            { "MudDataGrid.Filter", "Filtro" },
            { "MudDataGrid.False", "Falso" },
            { "MudDataGrid.FilterValue", "Valor filtro" },
            { "MudDataGrid.Group", "Grupo" },
            { "MudDataGrid.Hide", "Ocultar" },
            { "MudDataGrid.HideAll", "Ocultar todos" },
            { "MudDataGrid.is", "é" },
            { "MudDataGrid.is after", "Es después" },
            { "MudDataGrid.is before", "Es antes" },
            { "MudDataGrid.is empty", "Es vacío" },
            { "MudDataGrid.is not", "No es" },
            { "MudDataGrid.is not empty", "No es vacío" },
            { "MudDataGrid.is on or after", "Es en o después" },
            { "MudDataGrid.is on or before", "Es en o antes" },
            { "MudDataGrid.not contains", "no contiene" },
            { "MudDataGrid.not equals", "no es igual" },
            { "MudDataGrid.Operator", "Operador" },
            { "MudDataGrid.RefreshData", "Atualizar datos" },
            { "MudDataGrid.Save", "Guardar" },
            { "MudDataGrid.ShowAll", "Mostrar todo" },
            { "MudDataGrid.starts with", "Comienza con" },
            { "MudDataGrid.True", "Verdadero" },
            { "MudDataGrid.Ungroup", "Desagrupar" },
            { "MudDataGrid.Unsort", "Ordenar" },
            { "MudDataGrid.Value", "Valor" }
        };
        }

        public override LocalizedString this[string key]
        {
            get
            {
                var currentCulture = Thread.CurrentThread.CurrentUICulture.Parent.TwoLetterISOLanguageName;
                if (currentCulture.Equals("es", StringComparison.InvariantCultureIgnoreCase)
                    && _localization.TryGetValue(key, out var res))
                {
                    return new(key, res);
                }
                else
                {
                    return new(key, key, true);
                }
            }
        }
    }
}
