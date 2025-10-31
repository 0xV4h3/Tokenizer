namespace Tokenizer.Core.Models
{
    // ---------------- Operator Precedence Constants ----------------
    public static class SQLOperatorPrecedence
    {
        public const int HIGHEST = 100;
        public const int MEMBER = 90;
        public const int TYPECAST = 85;
        public const int UNARY = 80;
        public const int MULTIPLICATIVE = 70;
        public const int ADDITIVE = 60;
        public const int SHIFT = 55;
        public const int BITWISE_AND = 50;
        public const int BITWISE_XOR = 45;
        public const int BITWISE_OR = 40;
        public const int COMPARISON = 35;
        public const int NULL_TEST = 30;
        public const int BETWEEN = 25;
        public const int IN = 20;
        public const int PATTERN = 15;
        public const int AND = 10;
        public const int OR = 5;
        public const int ASSIGNMENT = 1;
        public const int LOWEST = 0;
    }

    // ---------------- Core Token Types ----------------
    public enum TokenType
    {
        UNKNOWN = 0,
        KEYWORD,
        FUNCTION,
        IDENTIFIER,
        LITERAL,
        LITERAL_CATEGORY,
        OPERATOR,
        PUNCTUATOR,
        DATETIMEPART,
        COMMENT,
        END_OF_FILE
    }

    // ---------------- SQL Statement Keywords ----------------
    public enum DMLKeyword
    {
        UNKNOWN = 0,
        SELECT, INSERT, UPDATE, DELETE,
        MERGE, EXECUTE, VALUES,
        OUTPUT, DEFAULT, INTO, RETURNING, USING
    }

    public enum DDLKeyword
    {
        UNKNOWN = 0,
        CREATE, ALTER, DROP, TRUNCATE,
        TABLE, VIEW, INDEX, SEQUENCE, CONSTRAINT, TRIGGER,
        PRIMARY, FOREIGN, REFERENCES, UNIQUE, CHECK,
        PARTITION, COLUMN, DATABASE, SCHEMA, TYPE, KEY
    }

    public enum ClauseKeyword
    {
        UNKNOWN = 0,
        FROM, WHERE, GROUP, HAVING, ORDER,
        JOIN, INNER, LEFT, RIGHT, FULL, CROSS,
        OUTER, ON, USING,
        DISTINCT, TOP, LIMIT, OFFSET,
        WINDOW, PARTITION, OVER,
        AS, BY, DO, END,
        UNION, APPLY
    }

    public enum CTEKeyword { UNKNOWN = 0, WITH, RECURSIVE }
    public enum SetOpKeyword { UNKNOWN = 0, UNION, INTERSECT, EXCEPT }

    public enum SessionOptionKeyword
    {
        UNKNOWN = 0,
        SET, ON, OFF, AUTOCOMMIT, IMPLICIT_TRANSACTIONS, ISOLATION_LEVEL
    }

    public enum PredicateKeyword
    {
        UNKNOWN = 0,
        IN, IS, LIKE, BETWEEN,
        ALL, ANY, SOME, EXISTS, NOT, UNIQUE
    }

    public enum LogicalConstantKeyword { UNKNOWN = 0, NULL_KEYWORD, TRUE_KEYWORD, FALSE_KEYWORD }

    public enum TransactionKeyword
    {
        UNKNOWN = 0,
        TRANSACTION, BEGIN, COMMIT, ROLLBACK, SAVEPOINT, RELEASE, CHAIN
    }

    public enum SecurityKeyword { UNKNOWN = 0, GRANT, REVOKE, DENY, ON, TO }

    public enum ProgStmtKeyword
    {
        UNKNOWN = 0,
        DECLARE, SET, PRINT, RETURN, THROW, TRY, CATCH,
        IF, ELSE, LOOP, WHILE, FOR, BREAK, CONTINUE, EXEC, GO
    }

    public enum MiscKeyword
    {
        UNKNOWN = 0,
        CASE, WHEN, THEN, ELSE, END,
        ASC, DESC, GENERATED, AUTOINCREMENT,
        CASCADE, RESTRICT, DEFERRABLE, EXPLAIN, ANALYZE, VACUUM
    }

    // ---------------- Function Categories ----------------
    public enum FunctionCategory { UNKNOWN = 0, AGGREGATE, SCALAR, STRING, DATETIME, MATHEMATICAL, SYSTEM, WINDOW }

    public enum AggregateFunction
    {
        UNKNOWN = 0, COUNT, SUM, AVG, MIN, MAX, GROUP_CONCAT, LISTAGG, ARRAY_AGG, STDDEV, VARIANCE
    }

    public enum ScalarFunction
    {
        UNKNOWN = 0, CONVERT, CAST, COALESCE, NULLIF, IFNULL, LEAST, GREATEST, FORMAT,
        LENGTH, POSITION, ABS, ROUND, FLOOR, CEILING, DATE_TRUNC, DATE_ADD, DATE_SUB, EXTRACT
    }

    public enum StringFunction
    {
        UNKNOWN = 0, UPPER, LOWER, SUBSTRING, TRIM, LTRIM, RTRIM, CONCAT, REPLACE, SPLIT_PART,
        LEFT, RIGHT, REPEAT, REVERSE, CHAR_LENGTH, CHARACTER_LENGTH, POSITION
    }

    public enum DateTimeFunction
    {
        UNKNOWN = 0, DATEPART, GETDATE, NOW, CURRENT_DATE, CURRENT_TIME, CURRENT_TIMESTAMP,
        LOCALTIME, LOCALTIMESTAMP, AGE, TO_DATE, TO_TIMESTAMP
    }

    public enum MathFunction
    {
        UNKNOWN = 0, ABS, CEILING, FLOOR, ROUND, POWER, SQRT, EXP, LN, LOG, MOD, SIGN, TRUNC,
        RANDOM, PI, SIN, COS, TAN, ASIN, ACOS, ATAN, DEGREES, RADIANS
    }

    public enum SystemFunction
    {
        UNKNOWN = 0, SUSER_SNAME, CURRENT_USER, SESSION_USER, USER, SYSTEM_USER, TRANCOUNT, VERSION, DATABASE, SCHEMA_NAME
    }

    public enum WindowFunction
    {
        UNKNOWN = 0, ROW_NUMBER, RANK, DENSE_RANK, NTILE, PERCENT_RANK, CUME_DIST, LEAD, LAG, FIRST_VALUE, LAST_VALUE, NTH_VALUE
    }

    // ---------------- Categories and Types ----------------
    public enum KeywordCategory
    {
        UNKNOWN = 0,
        DML,
        DDL,
        CLAUSE,
        CTE,
        SETOP,
        SESSION_OPTION,
        PREDICATE,
        LOGICAL_CONST,
        TRANSACTION,
        SECURITY,
        PROG_STMT,
        MISC,
        DATA_TYPE
    }

    public enum OperatorCategory
    {
        UNKNOWN = 0, ARITHMETIC, ASSIGN, COMPARISON, LOGICAL, BITWISE, CONCAT, JSON, REGEX, ARRAY, TYPECAST
    }

    // ---------------- Operators ----------------
    public enum ArithmeticOp { UNKNOWN = 0, PLUS, MINUS, MULTIPLY, DIVIDE, MOD }
    public enum AssignOp { UNKNOWN = 0, ASSIGN, COLON_ASSIGN }
    public enum ComparisonOp
    {
        UNKNOWN = 0, LESS, GREATER, LESS_EQUAL, GREATER_EQUAL, NOT_EQUAL, EQUAL,
        IS_DISTINCT_FROM, IS_NOT_DISTINCT_FROM, LIKE, NOT_LIKE, ILIKE, NOT_ILIKE, SIMILAR_TO, NOT_SIMILAR_TO
    }

    public enum LogicalOp { UNKNOWN = 0, AND, OR, NOT, XOR, IMPLIES }
    public enum BitwiseOp { UNKNOWN = 0, BITWISE_AND, BITWISE_OR, BITWISE_XOR, BITWISE_NOT, LEFT_SHIFT, RIGHT_SHIFT }
    public enum ConcatOp { UNKNOWN = 0, CONCAT }

    public enum JsonOp
    {
        UNKNOWN = 0, ARROW, ARROW2, HASH_ARROW, HASH_ARROW2, AT, QUESTION, QUESTION_PIPE, QUESTION_AMP, HASH_MINUS
    }

    public enum RegexOp { UNKNOWN = 0, TILDE, NOT_TILDE, TILDE_STAR, NOT_TILDE_STAR }
    public enum TypecastOp { UNKNOWN = 0, TYPECAST }

    // ---------------- Identifiers ----------------
    public enum IdentifierCategory
    {
        UNKNOWN = 0, TABLE, VIEW, PROCEDURE, FUNCTION, TRIGGER, INDEX, CONSTRAINT, SEQUENCE,
        SCHEMA, DATABASE, USER_DEFINED_TYPE, ROLE, USER, EXTERNAL_TABLE, TEMP_TABLE, GLOBAL_TEMP_TABLE,
        USER_VARIABLE, SYSTEM_VARIABLE, COLUMN, PARAMETER, LABEL
    }

    // ---------------- Literals ----------------
    public enum LiteralCategory
    {
        UNKNOWN = 0, STRING, ESCAPE_STRING, CHAR, INTEGER, FLOAT, BINARY, HEX,
        DATE, TIME, DATETIME, INTERVAL, UUID, ARRAY, JSON, XML, BOOLEAN, NULL_VALUE
    }

    // ---------------- Date/Time Components ----------------
    public enum DateTimePart
    {
        UNKNOWN = 0, YEAR, QUARTER, MONTH, DAY_OF_YEAR, DAY, WEEK, ISO_WEEK, WEEKDAY,
        HOUR, MINUTE, SECOND, MILLISECOND, MICROSECOND, NANOSECOND, TIMEZONE_OFFSET
    }

    // ---------------- Delimiters and Punctuation ----------------
    public enum CommonSymbol
    {
        UNKNOWN = 0, COMMA, SEMICOLON, LPAREN, RPAREN, LBRACE, RBRACE, LBRACKET, RBRACKET, DOT, COLON, PARAM_MARKER
    }

    public enum TSQLSymbol { UNKNOWN = 0, DOT, COLON }

    public enum StringDelimiter { UNKNOWN = 0, SINGLE_QUOTE, DOUBLE_QUOTE, BACKTICK, DOLLAR_QUOTE }

    // ---------------- Comments ----------------
    public enum CommentType { UNKNOWN = 0, SINGLE_LINE, MULTI_LINE }
}