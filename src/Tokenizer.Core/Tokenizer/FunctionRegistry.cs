using System;
using System.Collections.Generic;
using Tokenizer.Core.Models;

namespace Tokenizer.Core.Tokenizer
{
    /// <summary>
    /// Registry of built-in functions (name -> FunctionInfo). Case-insensitive.
    /// </summary>
    public class FunctionRegistry
    {
        private readonly Dictionary<string, FunctionInfo> _map = new(StringComparer.OrdinalIgnoreCase);

        public FunctionRegistry()
        {
            // AggregateFunction
            Add("COUNT", FunctionCategory.AGGREGATE, 1, 1, "Count rows or expression occurrences");
            Add("SUM", FunctionCategory.AGGREGATE, 1, 1, "Sum of values");
            Add("AVG", FunctionCategory.AGGREGATE, 1, 1, "Average");
            Add("MIN", FunctionCategory.AGGREGATE, 1, 1, "Minimum");
            Add("MAX", FunctionCategory.AGGREGATE, 1, 1, "Maximum");
            Add("GROUP_CONCAT", FunctionCategory.AGGREGATE, 1, -1, "String aggregate (group concat)");
            Add("LISTAGG", FunctionCategory.AGGREGATE, 1, -1, "List aggregate");
            Add("ARRAY_AGG", FunctionCategory.AGGREGATE, 1, -1, "Aggregate into array");
            Add("STDDEV", FunctionCategory.AGGREGATE, 1, 1, "Standard deviation");
            Add("VARIANCE", FunctionCategory.AGGREGATE, 1, 1, "Variance");

            // ScalarFunction
            Add("CONVERT", FunctionCategory.SCALAR, 2, 3, "Convert type");
            Add("CAST", FunctionCategory.SCALAR, 2, 2, "Cast type");
            Add("COALESCE", FunctionCategory.SCALAR, 1, -1, "Return first non-null");
            Add("NULLIF", FunctionCategory.SCALAR, 2, 2, "Null if equal");
            Add("IFNULL", FunctionCategory.SCALAR, 2, 2, "If null then value");
            Add("LEAST", FunctionCategory.SCALAR, 2, -1, "Least value");
            Add("GREATEST", FunctionCategory.SCALAR, 2, -1, "Greatest value");
            Add("FORMAT", FunctionCategory.SCALAR, 2, 2, "Format value");
            Add("LENGTH", FunctionCategory.SCALAR, 1, 1, "Length");
            Add("POSITION", FunctionCategory.SCALAR, 2, 2, "Position of substring");
            Add("ABS", FunctionCategory.SCALAR, 1, 1, "Absolute value");
            Add("ROUND", FunctionCategory.SCALAR, 1, 2, "Round value");
            Add("FLOOR", FunctionCategory.SCALAR, 1, 1, "Floor value");
            Add("CEILING", FunctionCategory.SCALAR, 1, 1, "Ceiling value");
            Add("DATE_TRUNC", FunctionCategory.SCALAR, 2, 2, "Truncate date to part");
            Add("DATE_ADD", FunctionCategory.SCALAR, 2, 2, "Add interval to date");
            Add("DATE_SUB", FunctionCategory.SCALAR, 2, 2, "Subtract interval from date");
            Add("EXTRACT", FunctionCategory.SCALAR, 2, 2, "Extract part from date/time");

            // StringFunction
            Add("UPPER", FunctionCategory.STRING, 1, 1, "Upper-case");
            Add("LOWER", FunctionCategory.STRING, 1, 1, "Lower-case");
            Add("SUBSTRING", FunctionCategory.STRING, 2, 3, "Substring");
            Add("TRIM", FunctionCategory.STRING, 1, 2, "Trim");
            Add("LTRIM", FunctionCategory.STRING, 1, 1, "Left trim");
            Add("RTRIM", FunctionCategory.STRING, 1, 1, "Right trim");
            Add("CONCAT", FunctionCategory.STRING, 2, -1, "Concatenate");
            Add("REPLACE", FunctionCategory.STRING, 3, 3, "Replace substring");
            Add("SPLIT_PART", FunctionCategory.STRING, 3, 3, "Split part");
            Add("LEFT", FunctionCategory.STRING, 2, 2, "Left substring");
            Add("RIGHT", FunctionCategory.STRING, 2, 2, "Right substring");
            Add("REPEAT", FunctionCategory.STRING, 2, 2, "Repeat string");
            Add("REVERSE", FunctionCategory.STRING, 1, 1, "Reverse string");
            Add("CHAR_LENGTH", FunctionCategory.STRING, 1, 1, "Char length");
            Add("CHARACTER_LENGTH", FunctionCategory.STRING, 1, 1, "Character length");

            // DateTimeFunction
            Add("DATEPART", FunctionCategory.DATETIME, 2, 2, "Get date part");
            Add("GETDATE", FunctionCategory.DATETIME, 0, 0, "Current date");
            Add("NOW", FunctionCategory.DATETIME, 0, 0, "Current timestamp");
            Add("CURRENT_DATE", FunctionCategory.DATETIME, 0, 0, "Current date");
            Add("CURRENT_TIME", FunctionCategory.DATETIME, 0, 0, "Current time");
            Add("CURRENT_TIMESTAMP", FunctionCategory.DATETIME, 0, 0, "Current timestamp");
            Add("LOCALTIME", FunctionCategory.DATETIME, 0, 0, "Local time");
            Add("LOCALTIMESTAMP", FunctionCategory.DATETIME, 0, 0, "Local timestamp");
            Add("AGE", FunctionCategory.DATETIME, 1, 2, "Age");
            Add("TO_DATE", FunctionCategory.DATETIME, 1, 2, "To date");
            Add("TO_TIMESTAMP", FunctionCategory.DATETIME, 1, 2, "To timestamp");

            // MathFunction
            Add("ABS", FunctionCategory.MATHEMATICAL, 1, 1, "Absolute value");
            Add("CEILING", FunctionCategory.MATHEMATICAL, 1, 1, "Ceiling");
            Add("FLOOR", FunctionCategory.MATHEMATICAL, 1, 1, "Floor");
            Add("ROUND", FunctionCategory.MATHEMATICAL, 1, 2, "Round");
            Add("POWER", FunctionCategory.MATHEMATICAL, 2, 2, "Power");
            Add("SQRT", FunctionCategory.MATHEMATICAL, 1, 1, "Square root");
            Add("EXP", FunctionCategory.MATHEMATICAL, 1, 1, "Exponent");
            Add("LN", FunctionCategory.MATHEMATICAL, 1, 1, "Natural log");
            Add("LOG", FunctionCategory.MATHEMATICAL, 1, 1, "Logarithm");
            Add("MOD", FunctionCategory.MATHEMATICAL, 2, 2, "Modulo");
            Add("SIGN", FunctionCategory.MATHEMATICAL, 1, 1, "Sign");
            Add("TRUNC", FunctionCategory.MATHEMATICAL, 1, 2, "Truncate");
            Add("RANDOM", FunctionCategory.MATHEMATICAL, 0, 0, "Random number");
            Add("PI", FunctionCategory.MATHEMATICAL, 0, 0, "PI");
            Add("SIN", FunctionCategory.MATHEMATICAL, 1, 1, "Sine");
            Add("COS", FunctionCategory.MATHEMATICAL, 1, 1, "Cosine");
            Add("TAN", FunctionCategory.MATHEMATICAL, 1, 1, "Tangent");
            Add("ASIN", FunctionCategory.MATHEMATICAL, 1, 1, "Arc sine");
            Add("ACOS", FunctionCategory.MATHEMATICAL, 1, 1, "Arc cosine");
            Add("ATAN", FunctionCategory.MATHEMATICAL, 1, 1, "Arc tangent");
            Add("DEGREES", FunctionCategory.MATHEMATICAL, 1, 1, "Degrees");
            Add("RADIANS", FunctionCategory.MATHEMATICAL, 1, 1, "Radians");

            // SystemFunction
            Add("SUSER_SNAME", FunctionCategory.SYSTEM, 0, 0, "System user name");
            Add("CURRENT_USER", FunctionCategory.SYSTEM, 0, 0, "Current user");
            Add("SESSION_USER", FunctionCategory.SYSTEM, 0, 0, "Session user");
            Add("USER", FunctionCategory.SYSTEM, 0, 0, "User");
            Add("SYSTEM_USER", FunctionCategory.SYSTEM, 0, 0, "System user");
            Add("TRANCOUNT", FunctionCategory.SYSTEM, 0, 0, "Transaction count");
            Add("VERSION", FunctionCategory.SYSTEM, 0, 0, "DB version");
            Add("DATABASE", FunctionCategory.SYSTEM, 0, 0, "Database");
            Add("SCHEMA_NAME", FunctionCategory.SYSTEM, 0, 1, "Schema name");

            // WindowFunction
            Add("ROW_NUMBER", FunctionCategory.WINDOW, 0, 0, "Row number");
            Add("RANK", FunctionCategory.WINDOW, 0, 0, "Rank");
            Add("DENSE_RANK", FunctionCategory.WINDOW, 0, 0, "Dense rank");
            Add("NTILE", FunctionCategory.WINDOW, 1, 1, "N-tile");
            Add("PERCENT_RANK", FunctionCategory.WINDOW, 0, 0, "Percent rank");
            Add("CUME_DIST", FunctionCategory.WINDOW, 0, 0, "Cumulative distribution");
            Add("LEAD", FunctionCategory.WINDOW, 1, 3, "Lead value");
            Add("LAG", FunctionCategory.WINDOW, 1, 3, "Lag value");
            Add("FIRST_VALUE", FunctionCategory.WINDOW, 1, 1, "First value");
            Add("LAST_VALUE", FunctionCategory.WINDOW, 1, 1, "Last value");
            Add("NTH_VALUE", FunctionCategory.WINDOW, 2, 2, "Nth value");
        }

        private void Add(string name, FunctionCategory category, int minArgs = -1, int maxArgs = -1, string? description = null)
        {
            _map[name] = new FunctionInfo(name, category, minArgs, maxArgs, description);
        }

        public bool TryGet(string name, out FunctionInfo info) => _map.TryGetValue(name, out info);

        public void RegisterCustom(FunctionInfo info)
        {
            if (info is null) throw new ArgumentNullException(nameof(info));
            _map[info.Name] = info;
        }

        public IReadOnlyDictionary<string, FunctionInfo> AllFunctions() => new Dictionary<string, FunctionInfo>(_map);
    }
}