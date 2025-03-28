using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Xrm.Sdk;

namespace Role_Switcher.Services
{
    /// <summary>
    /// Provides utility functions for managing the user grid and converting CRM data.
    /// </summary>
    public class UserGridBuilder
    {
        /// <summary>
        /// Converts a list of user entities into a DataTable.
        /// </summary>
        /// <param name="users">The list of CRM user entities.</param>
        /// <returns>A DataTable for binding to a DataGridView.</returns>
        public DataTable BuildUserTable(IEnumerable<Entity> users)
        {
            var data = users.Select(e =>
                e.Attributes.ToDictionary(
                    a => a.Key,
                    a => e.FormattedValues.ContainsKey(a.Key)
                        ? (object)e.FormattedValues[a.Key]
                        : a.Value
                )).ToList();

            return ConvertToDataTable(data);
        }

        /// <summary>
        /// Extracts user IDs from selected rows in a DataGridView.
        /// </summary>
        /// <param name="userGrid">The DataGridView containing selected user rows.</param>
        /// <returns>A list of GUIDs representing selected user IDs.</returns>
        public List<Guid> ExtractUserIds(DataGridView userGrid)
        {
            var ids = new List<Guid>();
            int idColumnIndex = userGrid.Columns.IndexOf(userGrid.Columns["systemuserid"]);

            foreach (DataGridViewRow row in userGrid.SelectedRows)
            {
                var idValue = row.Cells[idColumnIndex].Value as string;
                if (Guid.TryParse(idValue, out Guid id))
                {
                    ids.Add(id);
                }
            }

            return ids;
        }

        /// <summary>
        /// Searches the DataGridView for the first row that matches the search text in any cell.
        /// </summary>
        /// <param name="userGrid">The DataGridView to search.</param>
        /// <param name="searchText">The search string to match.</param>
        public void SearchGrid(DataGridView userGrid, string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText)) return;

            string searchValue = searchText.ToLowerInvariant();

            foreach (DataGridViewRow row in userGrid.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value?.ToString().ToLowerInvariant().Contains(searchValue) == true)
                    {
                        userGrid.CurrentCell = row.Cells[0];
                        return;
                    }
                }
            }
        }

        private DataTable ConvertToDataTable(List<Dictionary<string, object>> data)
        {
            DataTable dataTable = new DataTable();
            if (data.Count == 0) return dataTable;

            AddColumnsToDataTable(data, dataTable);
            AddRowsToDataTable(data, dataTable);
            return dataTable;
        }

        private void AddColumnsToDataTable(List<Dictionary<string, object>> data, DataTable dataTable)
        {
            var allKeys = data.SelectMany(dict => dict.Keys).Distinct().ToList();

            if (allKeys.Contains("systemuserid"))
            {
                AddColumn(dataTable, "systemuserid");
                allKeys.Remove("systemuserid");
            }

            if (allKeys.Contains("fullname"))
            {
                AddColumn(dataTable, "fullname");
                allKeys.Remove("fullname");
            }

            foreach (var key in allKeys)
            {
                AddColumn(dataTable, key);
            }
        }

        private void AddRowsToDataTable(List<Dictionary<string, object>> data, DataTable dataTable)
        {
            foreach (var dict in data)
            {
                var row = dataTable.NewRow();
                foreach (var key in dict.Keys)
                {
                    string sanitizedColumnName = SanitizeColumnName(key);
                    row[sanitizedColumnName] = dict[key]?.ToString() ?? string.Empty;
                }
                dataTable.Rows.Add(row);
            }
        }

        private void AddColumn(DataTable dataTable, string columnName)
        {
            string sanitized = SanitizeColumnName(columnName);
            dataTable.Columns.Add(sanitized, typeof(string));
            dataTable.Columns[sanitized].Caption = columnName;
        }

        private string SanitizeColumnName(string columnName)
        {
            return columnName.Replace(".", "_");
        }
    }
}