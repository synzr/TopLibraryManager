using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TopLibraryManager.Services.Interfaces;
using TopLibraryManager.Services.Implementations;
using TopLibraryManager.Data;
using TopLibraryManager.Commands;
using TopLibraryManager.Commands.Books;
using TopLibraryManager.Commands.Readers;
using TopLibraryManager.Commands.Librarians;
using TopLibraryManager.Commands.Loans;
using TopLibraryManager.Commands.System;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.SetBasePath(Directory.GetCurrentDirectory());
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
    })
    .ConfigureServices((context, services) =>
    {
        // настраиваем контекст базы данных библиотеки
        services.AddDbContext<LibraryDbContext>(options =>
        {
            var connectionString = context.Configuration.GetConnectionString("Library");
            options.UseSqlite(connectionString);
        });

        // регистрируем сервисы
        services.AddSingleton<IConsoleUIService, ConsoleUIService>();
        services.AddTransient<ILibrarianService, LibrarianService>();
        services.AddTransient<IBookService, BookService>();
        services.AddTransient<IReaderService, ReaderService>();
        services.AddTransient<ILoanService, LoanService>();
        services.AddTransient<IFineService, FineService>();
        services.AddTransient<ICommandProcessorService, CommandProcessorService>();
        services.AddTransient<IAuthService, AuthService>();

        // регистрируем фабрику команд и реестр
        services.AddSingleton<ICommandFactory, CommandFactory>();
        services.AddSingleton<CommandRegistry>();

        // регистрируем все команды
        services.AddTransient<CreateBookCommand>();
        services.AddTransient<DeleteBookCommand>();
        services.AddTransient<SearchBooksCommand>();
        services.AddTransient<UpdateBookCommand>();

        services.AddTransient<DeleteLibrarianCommand>();
        services.AddTransient<GetLibrarianCommand>();
        services.AddTransient<RegisterLibrarianCommand>();

        services.AddTransient<CreateReaderCommand>();
        services.AddTransient<DeleteReaderCommand>();
        services.AddTransient<GetReaderCommand>();
        services.AddTransient<SearchReadersCommand>();
        services.AddTransient<UpdateReaderCommand>();

        services.AddTransient<CreateLoanCommand>();
        services.AddTransient<ReturnBookCommand>();
        services.AddTransient<ListActiveLoansCommand>();
        services.AddTransient<ViewLoanDetailsCommand>();
        services.AddTransient<PayFineCommand>();

        services.AddTransient<ExitCommand>();
        services.AddTransient<HelloCommand>();
        services.AddTransient<HelpCommand>();
        services.AddTransient<UnknownCommand>();

        // регистрируем цикл приложения
        services.AddHostedService<AppHostedService>();
    })
    .Build();

await host.RunAsync();
