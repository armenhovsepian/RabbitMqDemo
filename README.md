### Implementing Producer/Consumer with .NET Core console applications and RabbitMQ

Here is a demo program that shows how RabbitMQ can be used as a message broker between two .NET Core console applications.

In this demo, a RabbitMQ Management Docker image is used to run a rabbitmq container and have access to it via the browser locally.

```
docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.9-management
```


#### Follow these steps to run Producer and Concumer applications as Docker containers

In the RabbitMqDemo.Producer.csproj directory, create an image of the producer
```
docker build -t rabbitmqdemo.producer .
```

In the RabbitMqDemo.Consumer.csproj directory, create an image of the producer
```
docker build -t rabbitmqdemo.consumer .
```

In the RabbitMqDemo.sln directory, run containers with docker-compose
```
docker-compose run
```

