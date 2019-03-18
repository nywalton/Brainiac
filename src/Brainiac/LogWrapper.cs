using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Brainiac
{
    /// <summary>
    /// A LogWrapper singleton class.
    /// </summary>
    public sealed class LogWrapper
    {
        private readonly ServiceProvider _serviceProvider;

        /// <summary>
        /// Static initialization of the instance.
        /// </summary>
        public static LogWrapper Instance { get; } = new LogWrapper();

        /// <summary>
        /// Explicit static constructor tells compiler not to mark type as beforefieldinit
        /// </summary>
        static LogWrapper() { }

        /// <summary>
        /// Private constructor
        /// </summary>
        private LogWrapper()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// Retrieve a logger for the specified class type.
        /// </summary>
        /// <typeparam name="T">The Class type to get a logger for.</typeparam>
        /// <returns>A Logger.</returns>
        public ILogger GetLogger<T>() where T : class
        {
            return _serviceProvider.GetService<ILogger<T>>();
        }

        /// <summary>
        /// Clean up any Logger resources.
        /// </summary>
        public void Clean()
        {
            _serviceProvider.Dispose();
        }

        /// <summary>
        /// Configure services.
        /// </summary>
        private void ConfigureServices(IServiceCollection services)
        {
            NLogLoggerProvider provider = new NLogLoggerProvider();
            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Trace);
                builder.AddConsole();
                builder.AddDebug();
                builder.AddProvider(new NLogLoggerProvider());
            });
        }
    }
}
