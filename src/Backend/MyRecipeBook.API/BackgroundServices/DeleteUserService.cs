using Azure.Messaging.ServiceBus;
using MyRecipeBook.Application.UseCases.User.Delete.Delete;
using MyRecipeBook.Infrastructure.Services.ServiceBus;

namespace MyRecipeBook.API.BackgroundServices;

public class DeleteUserService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ServiceBusProcessor _processor;

    public DeleteUserService(IServiceProvider serviceProvider, DeleteUserProcessor processor)
    {
        _serviceProvider = serviceProvider;
        _processor = processor.GetProcessor();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _processor.ProcessMessageAsync += ProcessMessageAsync;

        _processor.ProcessErrorAsync += ExceptionReceivedHandler;
        
        await _processor.StartProcessingAsync(stoppingToken);
    }
    
    private async Task ProcessMessageAsync(ProcessMessageEventArgs args)
    {
        var message = args.Message.Body.ToString();
        var userIdentifier = Guid.Parse(message);
        
        var scope = _serviceProvider.CreateScope();
        var deleteUserUseCase = scope.ServiceProvider.GetRequiredService<IDeleteUserAccountUseCase>();
        
        await deleteUserUseCase.Execute(userIdentifier);
    }
    
    private Task ExceptionReceivedHandler(ProcessErrorEventArgs args)
    {
        // Adicionar logs
        // Adicionar uma func do azure para enviar e-mail para o usuário
        Console.WriteLine($"Erro ao processar a mensagem: {args.Exception.Message}");
        return Task.CompletedTask;
    }
    
    // Libera os recursos não gerenciados quando o serviço for finalizado
    ~DeleteUserService() => Dispose();

    public override void Dispose()
    {
        base.Dispose();
        
        GC.SuppressFinalize(this);
    }
}