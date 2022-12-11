# Demo .NET RabbitMQ resiliently

Demo using RabbitMQ resiliently


## Run project


### Run RabbitMQ

```bash
cd src/Demo.MessageBus/
docker-compose up
```

### Run WebApi Customers
```bash
cd src/Demo.Api.Customers/
dotnet watch
```

### Run WebApi Payments
```bash
cd src/Demo.Api.Payments/
dotnet watch
```