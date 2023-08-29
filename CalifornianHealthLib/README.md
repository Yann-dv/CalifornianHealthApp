# Californian Health Library

## Description

This is the library shared by all Californian Health projects. It contains the following:

- **Contexts**: Contexts are used to provide a global state to the applications. They are used to store data used by the apps.
- **Models**: Models are used to store datas in the database. They are used by the contexts.
- **Services**: Services are used to communicate with the backend. They are used by the contexts. The Californian Health apps use RabbitMQ to communicate with the backend for bookings.

## Installation

Download the project and build it. Place the project in the same root folder than others CalifornianHealth projects to auto-reference it:

> An example of how is referenced the library in a CalifornianHealth project:

<img src=".\img\ref.png" width="70%" alt="reference example">

## Requirements

- .NET Framework 4.8.1
- NuGet packages :
    > Microsoft.EntityFrameworkCore
    > RabbitMQ.Client
    > Newtonsoft.Json
