# F&MD .NET Mediator 🤘

Uma implementação simples e gratuita do Mediator para nossos maravilhosos projetos .NET da F&MD

| Pacote 📦|
| ------- |
| `Fmd.Net.Mediator` |

## Como usar?? 🤔

### Instalação ⬇️

Pode-se instalar o Mediator da F&MD via NuGet Package Manager ou através de linha de comando do .NET CLI

```bash
dotnet add package Fmd.Net.Mediator
```

### Uso com 📢 Request + Notification 🔔

Esse exemplo demonstra como combar uma `Request` (command/query) e uma `Notification` (event) em um caso do mundo real

---

#### 1️⃣ Defina a Request e a Notification 📢🔔

```csharp
public class CriarClienteCommand : IRequest<string>
{
    public string Nome { get; set; }
}

public class ClienteCriadoEvent : INotification
{
    public Guid ClienteId { get; }

    public ClienteCriadoEvent(Guid clienteId)
    {
        ClienteId = clienteId;
    }
}
```

---

#### 2️⃣ Implemente os Handlers 💡

```csharp
public class CriarClienteHandler(IMediator mediator) : IRequestHandler<CriarClienteCommand, string>
{
    public async Task<string> Handle(CriarClienteCommand request, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();

        // Simulando persistência no BD...

        // Publicando evento
        await mediator.Publish(new ClienteCriadoEvent(id), cancellationToken);

        return $"Cliente '{request.Nome}' criado com ID {id}";
    }
}

public class EnviarEmailBemVindoHandler : INotificationHandler<ClienteCriadoEvent>
{
    public Task Handle(ClienteCriadoEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Enviando email de boas-vindas ao cliente {notification.ClienteId}");
        return Task.CompletedTask;
    }
}
```

---

#### 3️⃣ Registrar os Handlers (Dependency Injection) 📝

É possível registrar manualmente:

```csharp
services.AddSingleton<IMediator, Mediator>();

services.AddTransient<IRequestHandler<CriarClienteCommand, string>, CriarClienteHandler>();
services.AddTransient<INotificationHandler<ClienteCriadoEvent>, EnviarEmailBemVindoHandler>();
```

Ou usar a varredura completa dos assemblies desejados da aplicação

```csharp
services.AddMediator(Assembly.Load("Demo.Application"), Assembly.Load("Demo.Domain"));
```

---

#### 4️⃣ Execução ⚙️

```csharp
public class ClienteService(IMediator mediator)
{
    public async Task<string> CriarCliente(string nome)
    {
        return await mediator.Send(new CriarClienteCommand { Nome = nome });
    }
}
```

---

Quando o método `CriarCliente` é chamado:

1. `CriarClienteHandler` faz o handle da request
2. Persiste o cliente no banco (simulação)
3. Publica o evento `ClienteCriadoEvent`
4. `EnviarEmailBemVindoHandler` faz o handle do evento

Essa estrutura separa de maneira limpa os **commands** (que alteram o estado e retornam resultado) das **notifications** (que comunicam o resto do sistema que algo aconteceu)

## Compatibilidade 🖇️

- Fmd.Net.Mediator utiliza o .NET 9 e exige no mínimo isso no projeto para sua utilização 🔥
- uma versão alta a fim de estimular a excelente prática da constante atualização de nossos projetos para as versões mais recentes do framework 🌟

## Sobre 💬

Fmd.Net.Mediator foi desenvolvido por [Vinícius Fumagalli](https://www.linkedin.com/in/vini-fumagalli/) a.k.a. Fumas sob a licença do [MIT](https://opensource.org/license/MIT).