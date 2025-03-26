using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Npgsql;
using Npgsql.NameTranslation;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace cloudscribe.EFCore.PostgreSql.Conventions
{
    public static class SnakeCaseHelper
    {
        public static void ApplySnakeCaseConventions(this ModelBuilder modelBuilder)
        {
            var mapper = new NpgsqlSnakeCaseNameTranslator();
            foreach (var table in modelBuilder.Model.GetEntityTypes())
            {
                ConvertToSnake(mapper, table);

                var newTableName = table.GetTableName();

                foreach (var property in table.GetProperties())
                {
                    ConvertToSnake(mapper, property, newTableName);
                }

                foreach (var primaryKey in table.GetKeys())
                {
                    ConvertToSnake(mapper, primaryKey);
                }

                foreach (var foreignKey in table.GetForeignKeys())
                {
                    ConvertToSnake(mapper, foreignKey);
                }

                foreach (var indexKey in table.GetIndexes())
                {
                    ConvertToSnake(mapper, indexKey);
                }
            }
        }


        private static void ConvertToSnake(INpgsqlNameTranslator mapper, object entity, string newTableName = "")
        {
            switch (entity)
            {
                case IMutableEntityType table:
                    var tableName = ConvertGeneralToSnake(mapper, table.GetTableName()).Replace("__", "_");
                    table.SetTableName(tableName);

                    break;
                case IMutableProperty property:
                    var currentName = property.GetColumnName(StoreObjectIdentifier.Table(newTableName, null));

                    if (currentName.IndexOf("_") == -1)
                    {
                        var propName = ConvertGeneralToSnake(mapper, property.Name);
                        property.SetColumnName(propName);
                    }
                    

                    break;
                case IMutableKey primaryKey:
                    primaryKey.SetName(ConvertKeyToSnake(mapper, primaryKey.GetName()));

                    break;
                case IMutableForeignKey foreignKey:
                    foreignKey.SetConstraintName(ConvertKeyToSnake(mapper, foreignKey.GetConstraintName()));
                    break;
                case IMutableIndex indexKey:
                    indexKey.SetDatabaseName(ConvertKeyToSnake(mapper, indexKey.GetDatabaseName()));  // new - jk

                    break;
                default:
                    throw new NotImplementedException("Unexpected type was provided to snake case converter");
            }
        }

        private static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }

            var sb = new StringBuilder();
            
            bool start_of_word = true;
            foreach (char ch in input)
            {
                if (char.IsUpper(ch))
                {
                    if (!start_of_word) sb.Append("_");

                    
                }
                else
                {
                    start_of_word = false;
                }

                sb.Append(ch.ToString().ToLower());

            }

            return sb.ToString();
        }

        private static string ConvertKeyToSnake(INpgsqlNameTranslator mapper, string keyName) =>
            ConvertGeneralToSnake(mapper, _keysRegex.Replace(keyName, match => match.Value.ToLower()));

        private static  string ConvertGeneralToSnake(INpgsqlNameTranslator mapper, string entityName) =>
            mapper.TranslateMemberName(ModifyNameBeforeConvertion(mapper, entityName));

        private static string ModifyNameBeforeConvertion(INpgsqlNameTranslator mapper, string entityName) => entityName;

        private static readonly Regex _keysRegex = new Regex("^(PK|FK|IX)_", RegexOptions.Compiled);

    }
}
