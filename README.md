# Californian Health

## Introduction

Californian Health is a booking application for patients to book appointments with doctors. It is a full-stack application built with .NET7. It was a monolithic application that was broken down into microservices. The application can be deployed on localhost and/or Docker.

## Table of Contents

- [Introduction](#introduction)
- [Table of Contents](#table-of-contents)
- [Technologies](#technologies)
- [Setup](#setup)
- [Features](#features)
- [Contact](#contact)

## Technologies

- .NET 7
- ASP.NET Core
- Entity Framework Core
- Docker
- RabbitMq
- SQL Server

## Setup

### Prerequisites

#### For Windows local deployment

- .NET 7
- Docker
- RabbitMq
- SQL Server

1.From Github, clone the repository : git clone https://github.com/Yann-dv/CalifornianHealthApp.git
2.Open all solutions for each services in Visual Studio 2019 (or other IDE) and run the service.

Here are the differents urls for each services :
- CalifornianHealthDatabase : (LocalDB)\. in SQL Server (windows authentication)
- CalifornianHealthApi : https://localhost:44366/swagger/index.html
- CalifornianHealthBookingServer: default RabbitMq url
- CalifornianHealthApp : https://localhost:44328/
- RabbitMq : http://localhost:15672/ (user:guest, pwd:guest)

#### For Docker deployment

Ensure that Docker is installed and running on your machine. If not, you can download it here : https://www.docker.com/products/docker-desktop

From Github, clone the repository : git clone https://github.com/Yann-dv/CalifornianHealthApp.git
Open a terminal and go to the CalifornianHealthApp folder. 
Run the following command : docker-compose up -d --build
Ensure that all containers are running with the following command : docker ps -a

Here are the differents urls for each services :
- CalifornianHealthDatabase : localhost, 1433 in SQL Server (user:sa, pwd:Password+123)
- CalifornianHealthApi : http://localhost:5000/
- CalifornianHealthBookingServer: http://localhost:5001/
- CalifornianHealthApp : http://localhost:8080/
- RabbitMq : http://localhost:15672/ (user:ch_queue_user, pwd:ch_pwd_123)

Trouble shooting : sometime the db creation and or the rabbitmq service is not ready when the other services are trying to connect to it (despite the retry logic included). In this case, you can just restart the concerned service.

You can also dockerize each service individually by going to the service folder and run the following command : docker build -t <service_name> . 
Then run the container with the following command : docker run -d -p <port>:<port> --name <service_name> <service_name>

Or also by using the docker-compose.yml file in the each service folder.

## Features

- Frontend can be accessed on https://localhost:44328/
- Frontend 

## Contact

yh-dev@protonmail.com

