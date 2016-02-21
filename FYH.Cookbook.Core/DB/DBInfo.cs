using System.Collections.Generic;
using System.Linq;

namespace FYH.Cookbook.Core.DB
{
    public class TableInfo
    {
        private string _tableName;
        /// <summary>
        /// Table Name
        /// </summary>
        public string TableName
        {
            get { return DBInfoHelper.ConvertTableName(_tableName); }
            set { _tableName = value; }
        }

        /// <summary>
        /// Table Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Table Description List
        /// </summary>
        public IEnumerable<string> Descriptions
        {
            get { return DBInfoHelper.ConvertDescriptions(Description); }
        }
    }


    public class ColumnInfo
    {
        private string _tableName;
        /// <summary>
        /// Table Name
        /// </summary>
        public string TableName
        {
            get { return DBInfoHelper.ConvertTableName(_tableName); }
            set { _tableName = value; }
        }

        /// <summary>
        /// Column Name
        /// </summary>
        public string ColumnName { get; set; }

        private string _columnType;
        /// <summary>
        /// Column Type
        /// </summary>
        public string ColumnType
        {
            get { return DBInfoHelper.ConvertColumnType(_columnType); }
            set { _columnType = value; }
        }

        /// <summary>
        /// Is Primary Key
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// Is Identity
        /// </summary>
        public bool IsIdentity { get; set; }

        /// <summary>
        /// Is Allow Null
        /// </summary>
        public bool IsAllowNull { get; set; }

        /// <summary>
        /// Is Column Value Nullable
        /// </summary>
        public bool IsColumnValueNullable
        {
            get { return IsPrimaryKey || IsAllowNull; }
        }

        /// <summary>
        /// Is Column Type Nullable
        /// </summary>
        public bool IsColumnTypeNullable
        {
            get
            { return ColumnType != "string" && IsColumnValueNullable; }
        }

        private int _length { get; set; }
        /// <summary>
        /// Column Length
        /// </summary>
        public int Length
        {
            get { return DBInfoHelper.ConvertColumnTypeLength(_columnType, _length); }
            set { _length = value; }
        }

        /// <summary>
        /// Column Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Column Description List
        /// </summary>
        public IEnumerable<string> Descriptions
        {
            get { return DBInfoHelper.ConvertDescriptions(Description); }
        }
    }


    public class ForeignKeyInfo
    {
        private string _tableName;
        /// <summary>
        /// Foreign Key Table Name
        /// </summary>
        public string TableName
        {
            get { return DBInfoHelper.ConvertTableName(_tableName); }
            set { _tableName = value; }
        }

        /// <summary>
        /// Foreign Key Column Name
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Foreign Key Column Property Name
        /// </summary>
        public string ColumnPropertyName
        {
            get { return DBInfoHelper.ConvertFKPropertyName(ColumnName); }
        }

        private string _fkTableName;
        /// <summary>
        /// Foreign Key Refer Table Name
        /// </summary>
        public string FK_TableName
        {
            get { return DBInfoHelper.ConvertTableName(_fkTableName); }
            set { _fkTableName = value; }
        }

        /// <summary>
        /// Foreign Key Refer Table Primary Key
        /// </summary>
        public string FK_ColumnName { get; set; }

        /// <summary>
        /// Foreign Key Constraint Name
        /// </summary>
        public string ConstraintName { get; set; }
    }


    public static class DBInfoHelper
    {
        private static string[] idSigns = new string[] { "id", "Id", "ID", "iD" };
        private static string[] endSigns = new string[] { ".", ";", ":" };
        private static string[] doubleByteSigns = new string[] { "nchar", "nvarchar", "ntext" };

        internal static string ConvertTableName(string tableName)
        {
            return tableName.Replace("$", "_");
        }

        internal static IEnumerable<string> ConvertDescriptions(string description)
        {
            if (string.IsNullOrEmpty(description))
                return new string[] { string.Empty };

            return description.Replace("\n", "").Split('\r').Select(d => d.Trim());
        }

        internal static int ConvertColumnTypeLength(string columnType, int length)
        {
            return length == -1 ?
                int.MaxValue :
                (doubleByteSigns.Contains(columnType) ? length / 2 : length);
        }

        internal static string ConvertFKPropertyName(string columnName)
        {
            foreach (var idSign in idSigns)
                columnName = columnName.Replace(idSign, string.Empty);

            return columnName;
        }

        internal static string ConvertColumnType(string type)
        {
            switch (type)
            {
                case "bfile":
                case "binary":
                case "image":
                case "varbinary":
                case "mediumtext":
                case "long":
                    return "byte[]";

                case "char":
                case "nchar":
                case "varchar":
                case "nvarchar":
                case "text":
                case "ntext":
                case "longtext":
                case "sysname":
                case "string":
                    return "string";

                case "time":
                case "date":
                case "datetime":
                case "smalldatetime":
                case "timestamp":
                    return "DateTime";

                case "uniqueide":
                case "uniqueidentifier":
                    return "Guid";

                case "bit":
                case "boolean":
                    return "bool";

                case "integer":
                case "int":
                case "smallint":
                case "tinyint":
                    return "int";

                case "bigint":
                    return "long";

                case "numeric":
                case "money":
                case "real":
                case "smallmoney":
                case "float":
                case "decimal":
                case "number":
                    return "double";

                default:
                    return type;
            }
        }


        public static IEnumerable<string> ConvertFKPropertyDescriptions(string columnName, string fk_tableName)
        {
            foreach (var endSign in endSigns)
                columnName = columnName.Replace(endSign, string.Empty);

            return new string[] { string.Format("Foreign Key {0} Refer {1} Entity Instance;", columnName, fk_tableName) };
        }
    }
}
