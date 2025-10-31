# Tokenizer

**Tokenizer** is a high-performance, extensible .NET desktop application for SQL tokenization, syntax analysis, and visualization. Designed for developers and database professionals, Tokenizer enables fast and accurate parsing of SQL code across multiple dialects, providing deep insights into SQL structure and assisting with debugging, language tooling, or educational use.

---

## Features

- **Desktop Application**: Intuitive and user-friendly WPF GUI for visualizing SQL token streams.
- **Multi-dialect Support**: Registry-driven architecture easily adapts to various SQL dialects (PostgreSQL, MySQL, SQL Server, etc.).
- **Comprehensive Tokenization**: Accurately recognizes keywords, operators, functions, identifiers, literals, comments, punctuators, and more.
- **Customizable Registries**: Easily extend or modify keyword, operator, and function registries to suit your project or dialect.
- **Advanced Visualization**: View detailed token streams, their categories, positions, and relationships.
- **Export Options**: Export tokenized results as JSON or CSV for further processing or integration.
- **Robust Parsing Engine**: Handles edge cases such as multi-word operators, user/system variables, quoted identifiers, and various literal types.
- **Extensive Unit Tests**: Ensures correctness and reliability across a wide range of SQL inputs.

---

## Project Structure

```
TokenizerSolution/
  src/
    Tokenizer.Core/
      Models/
        TokenEnums.cs           # Token type/category enums
        Token.cs                # Token class definition
        TokenInfo.cs            # Metadata for tokens
      Tokenizer/
        ITokenizer.cs           # ITokenizer interface
        TokenizerEngine.cs      # Core tokenization logic
        KeywordRegistry.cs      # Keyword registry
        OperatorRegistry.cs     # Operator registry
        PunctuatorRegistry.cs   # Punctuator registry
        FunctionRegistry.cs     # Function registry
      Utils/
        TokenColoring.cs        # Token color mapping
    Tokenizer.UI/
      Tokenizer.UI.csproj
      App.xaml / App.xaml.cs
      Views/
        MainWindow.xaml / MainWindow.xaml.cs
      ViewModels/
        MainViewModel.cs
        TokenViewModel.cs
      Commands/
        AsyncRelayCommand.cs
      Utils/
        TokenColoring.cs
    Tokenizer.Tests/
      TokenizerEngineTests.cs
      FunctionRegistryTests.cs
      KeywordRegistryTests.cs
      OperatorRegistryTests.cs
      PunctuatorRegistryTests.cs
      SerializationTests.cs
      TokenEnumsTests.cs
  README.md
  MIT LICENSE
```

---

## Getting Started

### 1. **Build and Run**
- **Clone the repository:**
  ```sh
  git clone https://github.com/your-org/Tokenizer.git
  ```
- **Open the solution** (`TokenizerSolution.sln`) in Visual Studio or Rider.
- **Build the project**.
- **Set `Tokenizer.UI` as the startup project** and run to launch the desktop application.

### 2. **Using the Application**
- Paste or type your SQL code in the editor.
- Click **Tokenize** to process the SQL and view token details.
- Use export options (JSON/CSV) to save results.

---

## Customization

- **Dialect Extensions:**  
  Add or modify entries in `KeywordRegistry`, `OperatorRegistry`, or `FunctionRegistry` to support your custom SQL dialects.

- **Integration:**  
  Use the core engine (`TokenizerEngine`) as a library in your own .NET projects for programmatic SQL analysis.

---

## Example

```sql
SELECT SUM(a), LOWER(b) FROM users WHERE a >= 10 AND b LIKE 'X%';
```

| Type      | Lexeme   | Category     | Line | Col |
|-----------|----------|--------------|------|-----|
| KEYWORD   | SELECT   | DML          | 1    | 1   |
| FUNCTION  | SUM      | AGGREGATE    | 1    | 8   |
| PUNCTUATOR| (        | LPAREN       | 1    | 11  |
| IDENTIFIER| a        |              | 1    | 12  |
| ...       | ...      | ...          | ...  | ... |

---

## Project Purpose

This project is aimed at developers, students, and engineers who require a fast, safe, and modern way to analyze and visualize SQL syntax in .NET desktop environments.  
Tokenizer provides reusable core libraries for SQL tokenization and comprehensive desktop tools for hands-on exploration, debugging, and educational scenarios involving SQL parsing and language tooling.
