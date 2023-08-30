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
- [Complete doc](#documentation)

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
- Frontend can make calls to API to get Consultants calendars and available slots
- Frontend can send messages to RabbitMq queue to book an appointment
- The Booking Server can receive messages from RabbitMq queue and book an appointment (update DB)
- The Booking Server can send messages to RabbitMq queue to confirm an appointment
- Frontend can receive response message from RabbitMq queue to display confirmation message

## Contact

yh-dev@protonmail.com

# Documentation

## Functional Documentation:

<strong>Introduction</strong> :
The  Californian Health application allows users to book online appointments with consultants, through a dynamic booking calendar system.
Use Cases and User Stories:
The application is a web application. It's composed of 3 pages :
Home page : it's the page that allows to see the Doctors. It's composed of a list of doctors images. When you click on a doctor, the application will redirect you to the booking page.


Booking page : it's the page that allows to see the appointments of a doctor. It's composed of a form that allows to select a doctor. When the form is submitted, the application will display the appointments of the selected doctor in the calendar. You can click on an available slot to book an appointment. If the doctor doesn't have any appointment, the application will display a message that says that the doctor doesn't have any appointment.


Confirmation page : it's the page that confirms the booking of the appointment. It's composed of the date of the appointment and the name of the doctor.


### User Stories :
* As a user, I want to see the list of doctors, so that I can choose a doctor :

    - When I go to the home page, I can see the list of doctors images, when I click on a doctor, I'm redirected to the booking page :

<img src=".\img\doctors.png" width="70%" alt="doctos images list">

* As a user, I want to see the appointments of a doctor, so that I can choose an appointment :
    
    - When I go to the booking page, I can see a form that allows to select a doctor, when I submit the form, I can see the appointments of the selected doctor in the calendar :

<img src=".\img\bookings.png" width="70%" alt="booking page">

* As a user, I want to book an appointment, so that I can have an appointment :

    - When I go to the booking page, I can see a form that allows to select a doctor, when I submit the form, I can see the appointments of the selected doctor in the calendar, when I click on an available slot, I'm redirected to the confirmation page :

<img src=".\img\confirm_1.png" width="70%" alt="confirmation page">

<img src=".\img\confirm_2.png" width="70%" alt="confirmation page">


As a user, I can go to the “My appointments” page and see the confirmed appointment I have with some consultants : 

<img src=".\img\myapt.png" width="70%" alt="confirmation page">


### System Components:
Explain the major components of the system, such as frontend, backend services, RabbitMQ, and the database. Create diagrams illustrating the architecture and interactions between these components.

### Features:
Detail each feature of the application, explaining what it does and how it benefits users. For example, describe how patients can browse doctors availability and book appointments, and how doctors can manage their schedules.

### Data Flow:
Create diagrams showing how data flows through the application. Illustrate how data moves from the frontend to backend services, gets processed, and is stored in the database. Include interactions with RabbitMQ for asynchronous messaging.

### Architecture Overview:
Present a comprehensive architecture diagram that visualizes how microservices communicate with each other, how they interact with external services like RabbitMQ and the database, and how the frontend interfaces with the backend.
### Integration Points:
Explain in detail how different microservices and components communicate and exchange data. Describe API endpoints, message queues, and protocols used for communication.

### Deployment:
Provide step-by-step instructions for deploying the application locally and using Docker. Include any environment variables, configurations, or prerequisites needed for a successful deployment.
## Technical Documentation:

### Technology Stack:
List and explain the technologies used, including .NET 7, ASP.NET Core, Entity Framework Core, RabbitMQ, SQL Server, and any other relevant tools or libraries. Justify why each technology was chosen.

### Microservices Architecture:
For each microservice, describe its responsibilities, interactions, and role in the overall system. Include sequence diagrams showing how microservices collaborate to fulfill user requests.

### Message Queue (RabbitMQ):
Detail how RabbitMQ is used, explaining concepts like exchanges, queues, and bindings. Provide examples of message payloads and how they are structured.

### Database Design:
Provide an entity-relationship diagram (ERD) showcasing the database schema, table relationships, and data attributes. Explain the purpose of each table and its relationship to other tables.

### API Documentation:
Use tools like Swagger to generate comprehensive API documentation. Document all endpoints, request/response models, headers, query parameters, and authentication/authorization mechanisms.

### Security Measures:
Explain the security measures in place, such as token-based authentication, role-based access control, and any encryption or data protection techniques used.

### Error Handling:
Detail how errors and exceptions are handled across the application. Describe how error messages are generated, logged, and presented to users.

## Test Coverage Report:

### Introduction:
Explain the purpose of the test coverage report, why it's essential for maintaining code quality, and how it helps identify areas of improvement.

### Test Types:
Differentiate between various types of tests employed, such as unit tests (testing individual functions or methods), integration tests (testing interactions between components), and end-to-end tests (testing entire user flows).

### Test Frameworks:
Provide an overview of the testing frameworks used (NUnit, xUnit, etc.), why they were chosen, and how they facilitate writing and executing tests.

### Coverage Metrics:
Explain coverage metrics like line coverage, branch coverage, and statement coverage. Elaborate on why achieving a certain coverage percentage is important for code maintainability and quality.

### Test Results:
Present a summary of test results for each component or service, including the total number of tests, number of passing tests, and coverage percentages. Highlight any critical areas with low coverage.

### Code Examples:
Include examples of unit tests, integration tests, and possibly end-to-end tests. Showcase how these tests are structured, what they're testing, and how they're written.

### Future Recommendations:
Suggest strategies for improving test coverage in areas where coverage is lacking. Explain how to identify and prioritize areas for improvement as the application evolves.
Remember that each of these sections should be tailored to your specific application and its architecture. It's advisable to collaborate with developers, testers, and other stakeholders to ensure accuracy and completeness in the documentation.

