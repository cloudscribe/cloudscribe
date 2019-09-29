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
                foreach (var property in table.GetProperties())
                {
                    ConvertToSnake(mapper, property);
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


        private static void ConvertToSnake(INpgsqlNameTranslator mapper, object entity)
        {
            switch (entity)
            {
                case IMutableEntityType table:
                    //var relationalTable = table.Relational();
                    var tableName = ConvertGeneralToSnake(mapper, table.GetTableName()).Replace("__", "_");
                    table.SetTableName(tableName);

                    break;
                case IMutableProperty property:
                    //property.Relational().ColumnName = ConvertGeneralToSnake(mapper, property.Relational().ColumnName);
                    var currentName = property.GetColumnName();
                    if(currentName.IndexOf("_") == -1)
                    {
                        var propName = ConvertGeneralToSnake(mapper, property.Name);
                        property.SetColumnName(propName);
                    }
                    

                    break;
                case IMutableKey primaryKey:
                    //primaryKey.Relational().Name = ConvertKeyToSnake(mapper, primaryKey.Relational().Name);
                    //var keyName = ConvertGeneralToSnake(mapper, primaryKey.);
                    primaryKey.SetName(ConvertKeyToSnake(mapper, primaryKey.GetName()));

                    break;
                case IMutableForeignKey foreignKey:
                    //foreignKey.Relational().Name = ConvertKeyToSnake(mapper, foreignKey.Relational().Name);
                    foreignKey.SetConstraintName(ConvertKeyToSnake(mapper, foreignKey.GetConstraintName()));
                    break;
                case IMutableIndex indexKey:
                    //indexKey.Relational().Name = ConvertKeyToSnake(mapper, indexKey.Relational().Name);
                    indexKey.SetName(ConvertKeyToSnake(mapper, indexKey.GetName()));
                    break;
                default:
                    throw new NotImplementedException("Unexpected type was provided to snake case converter");
            }
        }

        //private static void ConvertToSnake1(INpgsqlNameTranslator mapper, object entity)
        //{
        //    switch (entity)
        //    {
        //        case IMutableEntityType table:
        //            var relationalTable = table.Relational();
        //            relationalTable.TableName = ConvertGeneralToSnake(mapper, relationalTable.TableName);

        //            break;
        //        case IMutableProperty property:
        //            property.Relational().ColumnName = ConvertGeneralToSnake(mapper, property.Relational().ColumnName);
        //            break;
        //        case IMutableKey primaryKey:
        //            primaryKey.Relational().Name = ConvertKeyToSnake(mapper, primaryKey.Relational().Name);
        //            break;
        //        case IMutableForeignKey foreignKey:
        //            foreignKey.Relational().Name = ConvertKeyToSnake(mapper, foreignKey.Relational().Name);
        //            break;
        //        case IMutableIndex indexKey:
        //            indexKey.Relational().Name = ConvertKeyToSnake(mapper, indexKey.Relational().Name);
        //            break;
        //        default:
        //            throw new NotImplementedException("Unexpected type was provided to snake case converter");
        //    }
        //}


        //private static void ConvertToSnake2(INpgsqlNameTranslator mapper, object entity)
        //{
        //    switch (entity)
        //    {
        //        case IMutableEntityType table:

        //            var tableName = ConvertGeneralToSnake(mapper, table.GetTableName()).Replace("__", "_");
        //            table.SetTableName(tableName);

           
        //            //var relationalTable = table.Relational();
        //            //relationalTable.TableName = ConvertGeneralToSnake(mapper, table.GetTableName());

        //            break;

        //        case IMutableProperty property:
        //            //column c.require2fa does not exist
        //            //var colname = ConvertGeneralToSnake(mapper, property.GetColumnName());
        //            var colname = property.GetColumnName().ToSnakeCase();
        //            property.SetColumnName(colname);
        //            break;

        //        case IMutableKey primaryKey:
        //            primaryKey.SetName(ConvertKeyToSnake(mapper, primaryKey.GetName()));
        //            break;
        //        case IMutableForeignKey foreignKey:
        //            foreignKey.SetConstraintName(ConvertKeyToSnake(mapper, foreignKey.GetConstraintName()));
        //            break;
        //        case IMutableIndex indexKey:
        //            indexKey.SetName(ConvertKeyToSnake(mapper, indexKey.GetName()));
        //            break;
        //        default:
        //            throw new NotImplementedException("Unexpected type was provided to snake case converter");
        //    }
        //}

        private static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }

            //var startUnderscores = Regex.Match(input, @"^_+");
            //return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();

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


                //start_of_word = char.IsWhiteSpace(ch);
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
