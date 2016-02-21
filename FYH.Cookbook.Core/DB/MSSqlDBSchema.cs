using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FYH.Cookbook.Core.DB
{
    public class MSSqlDBSchema
    {
        public IList<TableInfo> Tables { get; private set; }
        public IDictionary<string, IEnumerable<ColumnInfo>> ColumnDic { get; private set; }
        public IDictionary<string, IEnumerable<ForeignKeyInfo>> ForeignDic { get; private set; }


        public MSSqlDBSchema(string connString)
        {
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                Tables = GetTableList(conn);
                var columns = GetColumnList(conn);
                ColumnDic = Tables.ToDictionary(t => t.TableName, t => columns.Where(c => c.TableName == t.TableName));
                var foreigns = GetForeignList(conn);
                ForeignDic = Tables.ToDictionary(t => t.TableName, t => foreigns.Where(f => f.TableName == t.TableName));
            }
        }


        private IList<TableInfo> GetTableList(IDbConnection conn)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = string.Format(@"
SELECT Isnull (table_name, '') AS TABLENAME,
       Isnull ( (SELECT value
                   FROM ::fn_listextendedproperty (NULL,
                                                   'user',
                                                   'dbo',
                                                   'table',
                                                   table_name,
                                                   DEFAULT,
                                                   DEFAULT)),
               '')
          AS DESCRIPTIONS
  FROM information_schema.tables
 WHERE table_catalog = '{0}' AND table_name <> 'sysdiagrams'",
            conn.Database);

            var tableList = new List<TableInfo>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tableList.Add(new TableInfo
                    {
                        TableName = Convert.ToString(reader[0]),
                        Description = Convert.ToString(reader[1])
                    });
                }
            }
            return tableList;
        }

        private IList<ColumnInfo> GetColumnList(IDbConnection conn)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = string.Format(@"
SELECT Isnull (OBJ.NAME, '') AS TABLENAME,
       Isnull (COL.NAME, '') AS COLUMNNAME,
       LOWER (Isnull (T.NAME, '')) AS COLUMNTYPE,
       Isnull (
          CASE
             WHEN EXISTS
                     (SELECT 1
                        FROM dbo.sysindexes SI
                             INNER JOIN dbo.sysindexkeys SIK
                                ON SI.id = SIK.id AND SI.indid = SIK.indid
                             INNER JOIN dbo.syscolumns SC
                                ON SC.id = SIK.id AND SC.colid = SIK.colid
                             INNER JOIN dbo.sysobjects SO
                                ON SO.NAME = SI.NAME AND SO.xtype = 'PK'
                       WHERE SC.id = COL.id AND SC.colid = COL.colid)
             THEN
                1
             ELSE
                0
          END,
          0)
          AS ISPRIMARYKEY,
       Isnull (
          CASE
             WHEN Columnproperty (COL.id, COL.NAME, 'ISIDENTITY') = 1 THEN 1
             ELSE 0
          END,
          0)
          AS ISIDENTITY,
       Isnull (CASE WHEN COL.isnullable = 1 THEN 1 ELSE 0 END, 1)
          AS ISALLOWNULL,
       Isnull (COL.length, 0) AS LENGTH,
       Isnull (EP.[value], '') AS DESCRIPTIONS
  FROM dbo.syscolumns COL
       LEFT JOIN dbo.systypes T ON COL.xtype = T.xusertype
       INNER JOIN dbo.sysobjects OBJ
          ON COL.id = OBJ.id AND OBJ.xtype = 'U' AND OBJ.status >= 0
       LEFT JOIN dbo.syscomments COMM ON COL.cdefault = COMM.id
       LEFT JOIN sys.extended_properties EP
          ON     COL.id = EP.major_id
             AND COL.colid = EP.minor_id
             AND EP.NAME = 'MS_DESCRIPTION'
       LEFT JOIN sys.extended_properties EPTWO
          ON     OBJ.id = EPTWO.major_id
             AND EPTWO.minor_id = 0
             AND EPTWO.NAME = 'MS_DESCRIPTION'
 WHERE EXISTS
          (SELECT 1
             FROM information_schema.columns
            WHERE table_name = OBJ.NAME AND table_catalog = '{0}')
ORDER BY COL.colorder",
            conn.Database);

            var columnList = new List<ColumnInfo>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    columnList.Add(new ColumnInfo
                    {
                        TableName = Convert.ToString(reader[0]),
                        ColumnName = Convert.ToString(reader[1]),
                        ColumnType = Convert.ToString(reader[2]),
                        IsPrimaryKey = Convert.ToBoolean(reader[3]),
                        IsIdentity = Convert.ToBoolean(reader[4]),
                        IsAllowNull = Convert.ToBoolean(reader[5]),
                        Length = Convert.ToInt32(reader[6]),
                        Description = Convert.ToString(reader[7])
                    });
                }
            }
            return columnList;
        }

        private IList<ForeignKeyInfo> GetForeignList(IDbConnection conn)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
SELECT Isnull (Object_name (b.fkeyid), '') AS TableName,
       Isnull ( (SELECT NAME
                   FROM syscolumns
                  WHERE colid = b.fkey AND id = b.fkeyid),
               '')
          AS ColumnName,
       Isnull (Object_name (b.rkeyid), '') AS FK_TableName,
       Isnull ( (SELECT NAME
                   FROM syscolumns
                  WHERE colid = b.rkey AND id = b.rkeyid),
               '')
          AS FK_ColumnName,
       Isnull (a.NAME, '') AS ConstraintName
  FROM sysobjects a
       JOIN sysforeignkeys b ON a.id = b.constid
       JOIN sysobjects c ON a.parent_obj = c.id
 WHERE a.xtype = 'f' AND c.xtype = 'U'";

            var infoList = new List<ForeignKeyInfo>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    infoList.Add(new ForeignKeyInfo
                    {
                        TableName = Convert.ToString(reader[0]),
                        ColumnName = Convert.ToString(reader[1]),
                        FK_TableName = Convert.ToString(reader[2]),
                        FK_ColumnName = Convert.ToString(reader[3]),
                        ConstraintName = Convert.ToString(reader[4])
                    });
                }
            }
            return infoList;
        }
    }
}
