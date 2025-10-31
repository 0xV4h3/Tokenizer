using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Win32;
using Tokenizer.Core.Models;
using Tokenizer.Core.Tokenizer;
using Tokenizer.Core.Utils;
using Tokenizer.UI.Commands;

namespace Tokenizer.UI.ViewModels
{
    /// <summary>
    /// Main view model for the tokenizer UI.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void Raise([CallerMemberName] string? n = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));

        private readonly TokenizerEngine _engine;
        private CancellationTokenSource? _cts;
        private bool _isRunning;
        private readonly List<Token> _lastTokens = new();

        public string SqlText { get; set; } = string.Empty;
        public ObservableCollection<TokenViewModel> Tokens { get; } = new();

        public ICommand TokenizeCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand ExportJsonCommand { get; }
        public ICommand ExportCsvCommand { get; }
        public ICommand ClearCommand { get; }

        public MainViewModel()
        {
            var keywords = new KeywordRegistry();
            var operators = new OperatorRegistry();
            var punctuators = new PunctuatorRegistry();
            var functions = new FunctionRegistry();
            _engine = new TokenizerEngine(keywords, operators, punctuators, functions);

            TokenizeCommand = new AsyncRelayCommand(TokenizeAsync, () => !_isRunning);
            CancelCommand = new AsyncRelayCommand(CancelAsync, () => _isRunning);
            ExportJsonCommand = new AsyncRelayCommand(ExportJsonAsync, () => !_isRunning && _lastTokens.Count > 0);
            ExportCsvCommand = new AsyncRelayCommand(ExportCsvAsync, () => !_isRunning && _lastTokens.Count > 0);
            ClearCommand = new AsyncRelayCommand(ClearAsync);
        }

        private void RaiseAll()
        {
            Raise(nameof(SqlText));
            Raise(nameof(Tokens));
            Raise(nameof(IsRunning));
        }

        private async Task TokenizeAsync()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            _isRunning = true;
            Raise(nameof(IsRunning));
            CommandManager.InvalidateRequerySuggested();

            Tokens.Clear();
            _lastTokens.Clear();

            try
            {
                await foreach (var t in _engine.TokenizeAsync(SqlText ?? string.Empty, _cts.Token))
                {
                    Tokens.Add(new TokenViewModel(t));
                    _lastTokens.Add(t);
                    if (t.Type == TokenType.END_OF_FILE) break;
                }
            }
            catch (OperationCanceledException) { }
            finally
            {
                _isRunning = false;
                RaiseAll();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private Task CancelAsync()
        {
            _cts?.Cancel();
            return Task.CompletedTask;
        }

        private Task ClearAsync()
        {
            Tokens.Clear();
            _lastTokens.Clear();
            RaiseAll();
            CommandManager.InvalidateRequerySuggested();
            return Task.CompletedTask;
        }

        private async Task ExportJsonAsync()
        {
            if (_lastTokens.Count == 0) return;
            var dlg = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                DefaultExt = "json",
                FileName = "tokens.json"
            };
            var res = dlg.ShowDialog();
            if (res != true) return;
            var json = TokenSerializer.ToJson(_lastTokens);
            await File.WriteAllTextAsync(dlg.FileName, json);
        }

        private async Task ExportCsvAsync()
        {
            if (_lastTokens.Count == 0) return;
            var dlg = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                DefaultExt = "csv",
                FileName = "tokens.csv"
            };
            var res = dlg.ShowDialog();
            if (res != true) return;
            var csv = TokenSerializer.ToCsv(_lastTokens);
            await File.WriteAllTextAsync(dlg.FileName, csv);
        }

        public bool IsRunning => _isRunning;
    }
}