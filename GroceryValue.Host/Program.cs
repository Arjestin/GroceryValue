using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Security;
using System.ServiceModel;
using GroceryValue.Library;

namespace GroceryValue.Host
{
    internal static class Program
    {
        private static ServiceHost _host;

        private static void Main()
        {
            try
            {
                StartProgram();
                InitializeDatabase();
                RunServiceHost();
            }
            catch (DbEntityValidationException exception)
            {
                exception.Display();
            }
            catch (Exception exception) when
            (
                exception is GroceryValueException ||
                exception is SecurityException ||
                exception is UnauthorizedAccessException ||
                exception is IOException ||
                exception is InvalidOperationException ||
                exception is ArgumentException ||
                exception is CommunicationException ||
                exception is TimeoutException
            )
            {
                exception.Display();
            }
            finally
            {
                FinishProgram();
            }
        }

        private static void StartProgram()
        {
            Console.Title = "GroceryValue";
            Console.ForegroundColor = ConsoleColor.Green;
        }

        private static void FinishProgram()
        {
            if (_host != null && _host.State != CommunicationState.Closed)
            {
                _host.Abort();
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey(true);
        }

        private static void InitializeDatabase()
        {
            Database.SetInitializer(new Initializer());
            using (var context = new Context())
            {
                context.Database.Log += Logger.LogHandler;
                Console.WriteLine("Database Initializing...");
                context.Database.Initialize(true); // Removing this command should cause the database to initialize itself lazily (untested).
                Console.WriteLine("Database Initialized.");
            }
        }

        private static void RunServiceHost()
        {
            _host = new ServiceHost(typeof(Service));
            Console.WriteLine();
            Console.WriteLine("Service Host Opening...");
            _host.Open();
            Console.WriteLine("Service Host Opened.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine("Service Host Endpoints:");
            Console.ForegroundColor = ConsoleColor.Magenta;
            foreach (var endpoint in _host.Description.Endpoints)
            {
                Console.WriteLine(endpoint.Address);
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
            Console.WriteLine("Press any key to close service host");
            Console.ReadKey(true);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine("Service Host Closing...");
            _host.Close();
            Console.WriteLine("Service Host Closed.");
        }

        private static void Display(this DbEntityValidationException exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine(exception.Message);
            foreach (var entityError in exception.EntityValidationErrors)
            {
                Console.WriteLine();
                Console.WriteLine(entityError.Entry.Entity.GetType().Name);
                foreach (var validationError in entityError.ValidationErrors)
                {
                    Console.WriteLine($"{validationError.PropertyName}: {validationError.ErrorMessage}");
                }
            }
        }

        private static void Display(this Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine(exception.Message);
        }
    }
}
