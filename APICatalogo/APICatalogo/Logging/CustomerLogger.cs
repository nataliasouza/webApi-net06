namespace APICatalogo.Logging
{
    public class CustomerLogger : ILogger
    {
        public readonly string loggerNome;
        readonly CustomLoggerProviderConfiguration loggerConfig;

        public CustomerLogger(string name, CustomLoggerProviderConfiguration config)
        {
            loggerNome = name;
            loggerConfig = config;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == loggerConfig.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string mensagem = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";

            EscreverAquivoLog(mensagem);
        }

        private void EscreverAquivoLog(string mensagem)
        {
            string enderecoArquivoLog = @"C:\Users\natal\Desktop\LOGS\Registro_Log.txt";

            try
            {
                File.AppendAllText(enderecoArquivoLog, mensagem + Environment.NewLine);
            }
            catch (Exception ex)
            {
                File.AppendAllText(enderecoArquivoLog, $"Erro ao escrever no arquivo de log: {ex}" + Environment.NewLine);
                throw;
            }
        }
    }
}
