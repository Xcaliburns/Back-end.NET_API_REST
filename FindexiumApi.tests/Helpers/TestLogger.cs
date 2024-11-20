using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindexiumApi.tests.Helpers
{
    // Classe générique TestLogger<T> qui implémente l'interface ILogger<T>
    public class TestLogger<T> : ILogger<T>
    {
        // Liste pour stocker les entrées de journalisation capturées
        public List<LogEntry> Logs { get; } = new List<LogEntry>();

        // Implémentation de la méthode BeginScope qui n'est pas utilisée ici, donc elle retourne null
        public IDisposable BeginScope<TState>(TState state) => null;

        // Indique que tous les niveaux de journalisation sont activés
        public bool IsEnabled(LogLevel logLevel) => true;

        // Implémentation de la méthode Log qui capture les messages de journalisation et les ajoute à la liste Logs
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception?, string> formatter)
        {
            // Ajoute une nouvelle entrée de journalisation à la liste Logs
            Logs.Add(new LogEntry
            {
                LogLevel = logLevel, // Niveau de journalisation (Information, Warning, Error, etc.)
                Message = formatter(state, exception), // Message de journalisation
                Exception = exception // Exception associée, s'il y en a une
            });
        }
    }

    // Classe LogEntry pour représenter une entrée de journalisation
    public class LogEntry
    {
        public LogLevel LogLevel { get; set; } // Niveau de journalisation
        public string Message { get; set; } // Message de journalisation
        public Exception Exception { get; set; } // Exception associée, s'il y en a une
    }
}
