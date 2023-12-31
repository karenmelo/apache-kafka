﻿using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using desenvolvedorIO;


var schemaConfig = new SchemaRegistryConfig{
    Url = "http://localhost:8081"
};

var schemaRegistry = new CachedSchemaRegistryClient(schemaConfig);

var config = new ConsumerConfig { 
    GroupId = "devIO",
    BootstrapServers = "localhost:9092" };

using var consumer = new ConsumerBuilder<string, Curso>(config)
                         .SetValueDeserializer(new AvroDeserializer<Curso>(schemaRegistry).AsSyncOverAsync())
                         .Build();

consumer.Subscribe("cursos");

while (true)
{
    var result = consumer.Consume();    
    Console.WriteLine($"Mensagem: {result.Message.Key}---{result.Message.Value.Descricao}");
}