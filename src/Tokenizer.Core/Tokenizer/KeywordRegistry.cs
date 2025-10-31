using System;
using System.Collections.Generic;
using Tokenizer.Core.Models;

namespace Tokenizer.Core.Tokenizer
{
    /// <summary>
    /// Registry of built-in SQL keywords (lexeme -> KeywordInfo). Case-insensitive.
    /// </summary>
    public class KeywordRegistry
    {
        private readonly Dictionary<string, KeywordInfo> _map = new(StringComparer.OrdinalIgnoreCase);

        public KeywordRegistry()
        {
            // DMLKeyword
            Add("SELECT", KeywordCategory.DML);
            Add("INSERT", KeywordCategory.DML);
            Add("UPDATE", KeywordCategory.DML);
            Add("DELETE", KeywordCategory.DML);
            Add("MERGE", KeywordCategory.DML);
            Add("EXECUTE", KeywordCategory.DML);
            Add("VALUES", KeywordCategory.DML);
            Add("OUTPUT", KeywordCategory.DML);
            Add("DEFAULT", KeywordCategory.DML);
            Add("INTO", KeywordCategory.DML);
            Add("RETURNING", KeywordCategory.DML);
            Add("USING", KeywordCategory.DML);

            // DDLKeyword
            Add("CREATE", KeywordCategory.DDL);
            Add("ALTER", KeywordCategory.DDL);
            Add("DROP", KeywordCategory.DDL);
            Add("TRUNCATE", KeywordCategory.DDL);
            Add("TABLE", KeywordCategory.DDL);
            Add("VIEW", KeywordCategory.DDL);
            Add("INDEX", KeywordCategory.DDL);
            Add("SEQUENCE", KeywordCategory.DDL);
            Add("CONSTRAINT", KeywordCategory.DDL);
            Add("TRIGGER", KeywordCategory.DDL);
            Add("PRIMARY", KeywordCategory.DDL);
            Add("FOREIGN", KeywordCategory.DDL);
            Add("REFERENCES", KeywordCategory.DDL);
            Add("UNIQUE", KeywordCategory.DDL);
            Add("CHECK", KeywordCategory.DDL);
            Add("PARTITION", KeywordCategory.DDL);
            Add("COLUMN", KeywordCategory.DDL);
            Add("DATABASE", KeywordCategory.DDL);
            Add("SCHEMA", KeywordCategory.DDL);
            Add("TYPE", KeywordCategory.DDL);
            Add("KEY", KeywordCategory.DDL);

            // ClauseKeyword
            Add("FROM", KeywordCategory.CLAUSE);
            Add("WHERE", KeywordCategory.CLAUSE);
            Add("GROUP", KeywordCategory.CLAUSE);
            Add("HAVING", KeywordCategory.CLAUSE);
            Add("ORDER", KeywordCategory.CLAUSE);
            Add("JOIN", KeywordCategory.CLAUSE);
            Add("INNER", KeywordCategory.CLAUSE);
            Add("LEFT", KeywordCategory.CLAUSE);
            Add("RIGHT", KeywordCategory.CLAUSE);
            Add("FULL", KeywordCategory.CLAUSE);
            Add("CROSS", KeywordCategory.CLAUSE);
            Add("OUTER", KeywordCategory.CLAUSE);
            Add("ON", KeywordCategory.CLAUSE);
            Add("USING", KeywordCategory.CLAUSE);
            Add("DISTINCT", KeywordCategory.CLAUSE);
            Add("TOP", KeywordCategory.CLAUSE);
            Add("LIMIT", KeywordCategory.CLAUSE);
            Add("OFFSET", KeywordCategory.CLAUSE);
            Add("WINDOW", KeywordCategory.CLAUSE);
            Add("PARTITION", KeywordCategory.CLAUSE);
            Add("OVER", KeywordCategory.CLAUSE);
            Add("AS", KeywordCategory.CLAUSE);
            Add("BY", KeywordCategory.CLAUSE);
            Add("DO", KeywordCategory.CLAUSE);
            Add("END", KeywordCategory.CLAUSE);
            Add("UNION", KeywordCategory.CLAUSE);
            Add("APPLY", KeywordCategory.CLAUSE);

            // CTEKeyword
            Add("WITH", KeywordCategory.CTE);
            Add("RECURSIVE", KeywordCategory.CTE);

            // SetOpKeyword
            Add("UNION", KeywordCategory.SETOP);
            Add("INTERSECT", KeywordCategory.SETOP);
            Add("EXCEPT", KeywordCategory.SETOP);

            // SessionOptionKeyword
            Add("SET", KeywordCategory.SESSION_OPTION);
            Add("ON", KeywordCategory.SESSION_OPTION);
            Add("OFF", KeywordCategory.SESSION_OPTION);
            Add("AUTOCOMMIT", KeywordCategory.SESSION_OPTION);
            Add("IMPLICIT_TRANSACTIONS", KeywordCategory.SESSION_OPTION);
            Add("ISOLATION_LEVEL", KeywordCategory.SESSION_OPTION);

            // PredicateKeyword
            Add("IN", KeywordCategory.PREDICATE);
            Add("IS", KeywordCategory.PREDICATE);
            Add("LIKE", KeywordCategory.PREDICATE);
            Add("BETWEEN", KeywordCategory.PREDICATE);
            Add("ALL", KeywordCategory.PREDICATE);
            Add("ANY", KeywordCategory.PREDICATE);
            Add("SOME", KeywordCategory.PREDICATE);
            Add("EXISTS", KeywordCategory.PREDICATE);
            Add("NOT", KeywordCategory.PREDICATE);
            Add("UNIQUE", KeywordCategory.PREDICATE);

            // LogicalConstantKeyword
            Add("NULL", KeywordCategory.LOGICAL_CONST);
            Add("TRUE", KeywordCategory.LOGICAL_CONST);
            Add("FALSE", KeywordCategory.LOGICAL_CONST);

            // TransactionKeyword
            Add("TRANSACTION", KeywordCategory.TRANSACTION);
            Add("BEGIN", KeywordCategory.TRANSACTION);
            Add("COMMIT", KeywordCategory.TRANSACTION);
            Add("ROLLBACK", KeywordCategory.TRANSACTION);
            Add("SAVEPOINT", KeywordCategory.TRANSACTION);
            Add("RELEASE", KeywordCategory.TRANSACTION);
            Add("CHAIN", KeywordCategory.TRANSACTION);

            // SecurityKeyword
            Add("GRANT", KeywordCategory.SECURITY);
            Add("REVOKE", KeywordCategory.SECURITY);
            Add("DENY", KeywordCategory.SECURITY);
            Add("ON", KeywordCategory.SECURITY);
            Add("TO", KeywordCategory.SECURITY);

            // ProgStmtKeyword
            Add("DECLARE", KeywordCategory.PROG_STMT);
            Add("SET", KeywordCategory.PROG_STMT);
            Add("PRINT", KeywordCategory.PROG_STMT);
            Add("RETURN", KeywordCategory.PROG_STMT);
            Add("THROW", KeywordCategory.PROG_STMT);
            Add("TRY", KeywordCategory.PROG_STMT);
            Add("CATCH", KeywordCategory.PROG_STMT);
            Add("IF", KeywordCategory.PROG_STMT);
            Add("ELSE", KeywordCategory.PROG_STMT);
            Add("LOOP", KeywordCategory.PROG_STMT);
            Add("WHILE", KeywordCategory.PROG_STMT);
            Add("FOR", KeywordCategory.PROG_STMT);
            Add("BREAK", KeywordCategory.PROG_STMT);
            Add("CONTINUE", KeywordCategory.PROG_STMT);
            Add("EXEC", KeywordCategory.PROG_STMT);
            Add("GO", KeywordCategory.PROG_STMT);

            // MiscKeyword
            Add("CASE", KeywordCategory.MISC);
            Add("WHEN", KeywordCategory.MISC);
            Add("THEN", KeywordCategory.MISC);
            Add("ELSE", KeywordCategory.MISC);
            Add("END", KeywordCategory.MISC);
            Add("ASC", KeywordCategory.MISC);
            Add("DESC", KeywordCategory.MISC);
            Add("GENERATED", KeywordCategory.MISC);
            Add("AUTOINCREMENT", KeywordCategory.MISC);
            Add("CASCADE", KeywordCategory.MISC);
            Add("RESTRICT", KeywordCategory.MISC);
            Add("DEFERRABLE", KeywordCategory.MISC);
            Add("EXPLAIN", KeywordCategory.MISC);
            Add("ANALYZE", KeywordCategory.MISC);
            Add("VACUUM", KeywordCategory.MISC);

            // DATA_TYPE (common SQL types)
            Add("INT", KeywordCategory.DATA_TYPE);
            Add("INTEGER", KeywordCategory.DATA_TYPE);
            Add("BIGINT", KeywordCategory.DATA_TYPE);
            Add("SMALLINT", KeywordCategory.DATA_TYPE);
            Add("TINYINT", KeywordCategory.DATA_TYPE);
            Add("MEDIUMINT", KeywordCategory.DATA_TYPE);
            Add("DECIMAL", KeywordCategory.DATA_TYPE);
            Add("NUMERIC", KeywordCategory.DATA_TYPE);
            Add("FLOAT", KeywordCategory.DATA_TYPE);
            Add("REAL", KeywordCategory.DATA_TYPE);
            Add("DOUBLE", KeywordCategory.DATA_TYPE);
            Add("CHAR", KeywordCategory.DATA_TYPE);
            Add("VARCHAR", KeywordCategory.DATA_TYPE);
            Add("TEXT", KeywordCategory.DATA_TYPE);
            Add("CLOB", KeywordCategory.DATA_TYPE);
            Add("BINARY", KeywordCategory.DATA_TYPE);
            Add("VARBINARY", KeywordCategory.DATA_TYPE);
            Add("BLOB", KeywordCategory.DATA_TYPE);
            Add("DATE", KeywordCategory.DATA_TYPE);
            Add("TIME", KeywordCategory.DATA_TYPE);
            Add("TIMESTAMP", KeywordCategory.DATA_TYPE);
            Add("DATETIME", KeywordCategory.DATA_TYPE);
            Add("INTERVAL", KeywordCategory.DATA_TYPE);
            Add("BOOLEAN", KeywordCategory.DATA_TYPE);
            Add("UUID", KeywordCategory.DATA_TYPE);
            Add("JSON", KeywordCategory.DATA_TYPE);
            Add("XML", KeywordCategory.DATA_TYPE);
            Add("ARRAY", KeywordCategory.DATA_TYPE);
        }

        private void Add(string lexeme, KeywordCategory category, int subKind = 0)
        {
            _map[lexeme] = new KeywordInfo(lexeme, category, subKind);
        }

        public bool TryGet(string lexeme, out KeywordInfo info) => _map.TryGetValue(lexeme, out info);

        public void RegisterCustom(KeywordInfo info)
        {
            if (info is null) throw new ArgumentNullException(nameof(info));
            _map[info.Lexeme] = info;
        }

        public IReadOnlyDictionary<string, KeywordInfo> AllKeywords() => new Dictionary<string, KeywordInfo>(_map);
    }
}