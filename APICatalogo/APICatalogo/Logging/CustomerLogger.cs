namespace APICatalogo.Logging
{
    public class CustomerLogger : ILogger
    {
        readonly string loggerNome;
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
            using (StreamWriter sw = new StreamWriter(enderecoArquivoLog, true))
            {
                try
                {
                    sw.WriteLine(mensagem);
                    sw.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
