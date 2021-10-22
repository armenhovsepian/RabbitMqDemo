# RabbitMqDemo

Here is a demo program that shows how RabbitMQ can be used as a message broker between two console applications.

In this demo, a RabbitMQ Management Docker image is used to run a rabbitmq container and have access to it via the browser locally.

```
docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.9-management
```
