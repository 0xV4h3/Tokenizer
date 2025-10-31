using System;
using System.Collections.Generic;
using Tokenizer.Core.Models;

namespace Tokenizer.Core.Tokenizer
{
    /// <summary>
    /// Full registry of SQL operators (symbol/word -> OperatorInfo). Case-insensitive.
    /// </summary>
    public class OperatorRegistry
    {
        private readonly Dictionary<string, OperatorInfo> _map = new(StringComparer.OrdinalIgnoreCase);

        public OperatorRegistry()
        {
            // ArithmeticOp
            Add("+", OperatorCategory.ARITHMETIC, SQLOperatorPrecedence.ADDITIVE);
            Add("-", OperatorCategory.ARITHMETIC, SQLOperatorPrecedence.ADDITIVE);
            Add("*", OperatorCategory.ARITHMETIC, SQLOperatorPrecedence.MULTIPLICATIVE);
            Add("/", OperatorCategory.ARITHMETIC, SQLOperatorPrecedence.MULTIPLICATIVE);
            Add("%", OperatorCategory.ARITHMETIC, SQLOperatorPrecedence.MULTIPLICATIVE);

            // AssignOp
            Add(":=", OperatorCategory.ASSIGN, SQLOperatorPrecedence.ASSIGNMENT);
            Add("=", OperatorCategory.ASSIGN, SQLOperatorPrecedence.ASSIGNMENT);

            // ComparisonOp (single/multi-word)
            Add("=", OperatorCategory.COMPARISON, SQLOperatorPrecedence.COMPARISON);
            Add("<", OperatorCategory.COMPARISON, SQLOperatorPrecedence.COMPARISON);
            Add(">", OperatorCategory.COMPARISON, SQLOperatorPrecedence.COMPARISON);
            Add("<=", OperatorCategory.COMPARISON, SQLOperatorPrecedence.COMPARISON);
            Add(">=", OperatorCategory.COMPARISON, SQLOperatorPrecedence.COMPARISON);
            Add("<>", OperatorCategory.COMPARISON, SQLOperatorPrecedence.COMPARISON);
            Add("!=", OperatorCategory.COMPARISON, SQLOperatorPrecedence.COMPARISON);
            Add("IS DISTINCT FROM", OperatorCategory.COMPARISON, SQLOperatorPrecedence.COMPARISON);
            Add("IS NOT DISTINCT FROM", OperatorCategory.COMPARISON, SQLOperatorPrecedence.COMPARISON);
            Add("LIKE", OperatorCategory.COMPARISON, SQLOperatorPrecedence.PATTERN);
            Add("NOT LIKE", OperatorCategory.COMPARISON, SQLOperatorPrecedence.PATTERN);
            Add("ILIKE", OperatorCategory.COMPARISON, SQLOperatorPrecedence.PATTERN);
            Add("NOT ILIKE", OperatorCategory.COMPARISON, SQLOperatorPrecedence.PATTERN);
            Add("SIMILAR TO", OperatorCategory.COMPARISON, SQLOperatorPrecedence.PATTERN);
            Add("NOT SIMILAR TO", OperatorCategory.COMPARISON, SQLOperatorPrecedence.PATTERN);

            // LogicalOp
            Add("AND", OperatorCategory.LOGICAL, SQLOperatorPrecedence.AND);
            Add("OR", OperatorCategory.LOGICAL, SQLOperatorPrecedence.OR);
            Add("NOT", OperatorCategory.LOGICAL, SQLOperatorPrecedence.UNARY, unary: true);
            Add("XOR", OperatorCategory.LOGICAL, SQLOperatorPrecedence.AND);
            Add("IMPLIES", OperatorCategory.LOGICAL, SQLOperatorPrecedence.OR);

            // BitwiseOp and shifts
            Add("&", OperatorCategory.BITWISE, SQLOperatorPrecedence.BITWISE_AND);
            Add("|", OperatorCategory.BITWISE, SQLOperatorPrecedence.BITWISE_OR);
            Add("^", OperatorCategory.BITWISE, SQLOperatorPrecedence.BITWISE_XOR);
            Add("~", OperatorCategory.BITWISE, SQLOperatorPrecedence.UNARY, unary: true);
            Add("<<", OperatorCategory.BITWISE, SQLOperatorPrecedence.SHIFT);
            Add(">>", OperatorCategory.BITWISE, SQLOperatorPrecedence.SHIFT);

            // ConcatOp
            Add("||", OperatorCategory.CONCAT, SQLOperatorPrecedence.ADDITIVE);

            // JSON/Array/Path
            Add("->", OperatorCategory.JSON, SQLOperatorPrecedence.MEMBER);
            Add("->>", OperatorCategory.JSON, SQLOperatorPrecedence.MEMBER);
            Add("#>", OperatorCategory.JSON, SQLOperatorPrecedence.MEMBER);
            Add("#>>", OperatorCategory.JSON, SQLOperatorPrecedence.MEMBER);
            Add("@>", OperatorCategory.JSON, SQLOperatorPrecedence.COMPARISON);
            Add("<@", OperatorCategory.JSON, SQLOperatorPrecedence.COMPARISON);
            Add("?", OperatorCategory.JSON, SQLOperatorPrecedence.COMPARISON);
            Add("?|", OperatorCategory.JSON, SQLOperatorPrecedence.COMPARISON);
            Add("?&", OperatorCategory.JSON, SQLOperatorPrecedence.COMPARISON);
            Add("#-", OperatorCategory.JSON, SQLOperatorPrecedence.MEMBER);

            // RegexOp
            Add("~", OperatorCategory.REGEX, SQLOperatorPrecedence.PATTERN);
            Add("!~", OperatorCategory.REGEX, SQLOperatorPrecedence.PATTERN);
            Add("~*", OperatorCategory.REGEX, SQLOperatorPrecedence.PATTERN);
            Add("!~*", OperatorCategory.REGEX, SQLOperatorPrecedence.PATTERN);

            // ArrayOp (Postgres)
            Add("&&", OperatorCategory.ARRAY, SQLOperatorPrecedence.AND);
            Add("@>", OperatorCategory.ARRAY, SQLOperatorPrecedence.COMPARISON);
            Add("<@", OperatorCategory.ARRAY, SQLOperatorPrecedence.COMPARISON);

            // TypecastOp and misc
            Add("::", OperatorCategory.TYPECAST, SQLOperatorPrecedence.TYPECAST, leftAssoc: false);
            Add(":", OperatorCategory.TYPECAST, SQLOperatorPrecedence.TYPECAST);

            // Member/dot (for completeness)
            Add(".", OperatorCategory.ARITHMETIC, SQLOperatorPrecedence.MEMBER);

            // Exponentiation (some dialects)
            Add("**", OperatorCategory.ARITHMETIC, SQLOperatorPrecedence.MULTIPLICATIVE);
        }

        private void Add(string symbol, OperatorCategory category, int precedence, bool leftAssoc = true, bool unary = false)
        {
            _map[symbol] = new OperatorInfo(symbol, category, precedence, leftAssoc, unary);
        }

        public bool TryGet(string symbol, out OperatorInfo info) => _map.TryGetValue(symbol, out info);

        public void RegisterCustom(OperatorInfo info)
        {
            if (info is null) throw new ArgumentNullException(nameof(info));
            _map[info.Symbol] = info;
        }

        public IReadOnlyDictionary<string, OperatorInfo> AllOperators() => new Dictionary<string, OperatorInfo>(_map);
    }
}