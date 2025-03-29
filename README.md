# snacktech-vidsnap-worker-video


## Descrição

Aplicação que executa um processo em background, consumindo mensagens para baixar vídeos, gerar imagens, criar um arquivo zip das imagens e enviar
os itens criados a um Bucket S3. E depois disso gera uma mensagem indicando sucesso do processamento

## Tecnologias

- **C#**: Linguagem de programação usada no desenvolvimento do projeto
- **.NET 8**: Framework como base em que a API é executada
- **SQS**: Tecnologia de mensageria que permite comunicação assíncrona através do envio de mensagens a uma fila específica.
- **S3**: Tecnologia de armazenamento de arquivos em Cloud
- **FFmpeg**: Programa que permite processamento de vídeo e imagem

## Como Utilizar

### Pré-requisitos

Antes de rodar o projeto SnackTech, certifique-se de que você possui os seguintes pré-requisitos:

- **.NET SDK**: O projeto foi desenvolvido com o .NET SDK 8. Instale a versão necessária para garantir a compatibilidade com o código.
- **Docker**: O projeto utiliza Docker para contêinerizar a aplicação. Instale o Docker Desktop para Windows ou Mac, ou configure o Docker Engine para Linux. O Docker Compose também é necessário para orquestrar os containers.
- **AWS SQS (Opcional)**: A aplicação faz uma comunicação assíncrona através do AWS SQS consumindo e publicando notificações. O arquivo de docker-compose sobe um serviço de LocalStack onde é possível criar as filas SQS para serem usadas no lugar das filas da AWS se desejado.
- **AWS S3 (Opcional)**: A aplicação faz o download e upload de arquivos se comunicando com o S3. O serviço de Localstack que o docker-compose constrói possibilita o uso do serviço S3 localmente ao invés de utilizar recursos na cloud AWS se dejado.

### Preparando o ambiente

O repositório tem um arquivo de Docker Compose que sobe toda a estrutura necessária para a API:

- Um Localstack onde é possível ter os serviços de SQS e S3 localmente
- A aplicação em si com acesso ao Localstack

Depois de rodar o comando docker-compose up, rode o script desejado de acordo com o local da infraestrutura:

- Localstack:

```
$./scripts/script-vidsnap-localstack.sh
```

- AWS:

```
$./scripts/script-vidsnap-aws.sh
```

Cada script constrói os mesmos recursos de SQS e S3 em suas respectivas infraestruturas. Além disso configura uma policy que permite o S3 publicar mensagens em uma fila SQS especificada e uma configuração onde, quando um evento 's3:ObjectCreated:*' ocorre onde o arquivo possui um formato de vídeo válido, é enviado a mensagem para a fila.


### Uso

A aplicação no momento foca somente no processo necessário, fazendo o consumo da fila especificada, baixando o vídeo localmente, extraindo imagens, criando um arquivo zip das imagens e fazendo o upload para o S3 do arquivo zip e da primeira imagem extraída, ambos no mesmo lugar onde ficou o vídeo. É possível acompanhar os logs da aplicação fazendo esses processamentos.

## Desenvolvimento

### Estrutura do Código

Todo o código fonte da aplicação fica dentro da pasta `src` e os projetos de testes dentro da pasta `tests`.

O projeto se baseou na arquitetura hexagonal e cada módulo representa uma parte criada para atender o objetivo do serviço.

#### adapter.api

O módulo que sobe a aplicação de fato. No momento ainda não há rotas de API, mas a estrutura inicial está preparada, além de subir o Worker que faz o consumo das mensagens em segundo plano.

#### adapter.amazon.s3

Módulo focado em trabalhar com o S3, seja para baixar arquivos como para fazer o upload

#### adapter.amazon.sqs

Módulo que atua em conjunto ao SQS, para consumir, produzir e deletar mensagens

#### adapter.video

O módulo que trabalha com o FFmpeg para extrair os frames dos vídeos como imagens.

#### core.domain

Núcleo com todas as estruturas de dados necessárias para todo o conjunto de módulos da aplicação. Possui também as interfaces como portas aos adapters.

#### core.application

Módulo com serviços que trabalham as estruturas do núcleo e ou aciona os adapters ou é acionado pelos adapters através das portas

#### Tests

A aplicação conta com 1 projeto de teste unitário.

O `unit-tests` contem todo o conjunto de testes unitários construídos para garantir que cada unidade de código esteja operando como esperado dentro de cada respectivo contexto

### Modificabilidade

Para alterar o código da aplicação, recomendamos que faça um clone do repositório e crie uma branch própria partindo da branch development, para não perder últimas mudanças.

Após aplicar as alterações, abra um PR apontando para a branch development como destino e espere a aprovação. Uma vez aprovado a pipeline de CI/CD deve entrar em ação, passando o código pelo Sonar e gerando o artefato necessário para seguir com o deploy.