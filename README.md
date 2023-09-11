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
- [Complete documentation](#documentation)

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
<hr />

# Documentation

## Functional Documentation:

### Introduction:
The  Californian Health application allows users to book online appointments with consultants, through a dynamic booking calendar system.

### Use Cases and User Stories :

On user/client side, the application is a web application, composed of 3 pages :
Home page : it's the page that allows users to see the Doctors ang have general informations. It's composed of a list of doctors images. When you click on a doctor, the application will redirect you to the booking page.

Booking page : it's the page that allows users to see the available appointments slots of a doctor. It's composed of a form that allows to select a doctor. When the form is submitted, the application will display the appointments of the selected doctor in the calendar. You can click on an available slot to book an appointment. If the doctor doesn't have any appointment, the application will display a message that says that the doctor doesn't have any appointment.

Confirmation page : it's the page that confirms the booking of the appointment. It's composed of the date of the appointment and the name of the doctor.

### User Stories :
* As a user, I want to see the list of doctors, so that I can choose a doctor :

    - When I go to the home page, I can see the list of doctors images, when I click on a doctor, I'm redirected to the booking page :

<img src=".\CalifornianHealthFrontendUpdated\img\doctors.png" width="70%" alt="doctos images list">

* As a user, I want to see the appointments of a doctor, so that I can choose an appointment :
    
    - When I go to the booking page, I can see a form that allows to select a doctor, when I submit the form, I can see the appointments of the selected doctor in the calendar :

<img src=".\CalifornianHealthFrontendUpdated\img\bookings.png" width="70%" alt="booking page">

* As a user, I want to book an appointment, so that I can have an appointment :

    - When I go to the booking page, I can see a form that allows to select a doctor, when I submit the form, I can see the appointments of the selected doctor in the calendar, when I click on an available slot, I'm redirected to the confirmation page :

<img src=".\CalifornianHealthFrontendUpdated\img\confirm_1.png" width="70%" alt="confirmation page">

<img src=".\CalifornianHealthFrontendUpdated\img\confirm_2.png" width="70%" alt="confirmation page">


As a user, I can go to the “My appointments” page and see the confirmed appointment I have with some consultants : 

<img src=".\CalifornianHealthFrontendUpdated\img\myapt.png" width="70%" alt="confirmation page">


### System Components:
The application architecture consists of several interconnected Docker services, running within a custom Docker network named "ch-network". Here are the main components of the architecture and their relationships:

1. RabbitMQ service (rabbitmq):
        Uses the "rabbitmq:3-management" image to run a RabbitMQ server with a management interface.
        Defines a container name as "rabbitmq-container".
        Configures environment variables to set the default username and password.
        Exposes ports 5671 (AMQP 1.0), 5672 (default RabbitMQ port), and 15672 (management UI) to the host.
        Belongs to the "ch-network" network.

2. Booking Server container (californianhealthbookingserver):
        Uses the "californianhealthbookingserver" image to run the booking server.
        Defines a container name as "ch-booking-server-container".
        Builds the image from a Dockerfile in the "CalifornianHealthBookingServer" directory.
        Depends on the "rabbitmq" service to start.
        Exposes port 5001 from the container to port 80 on the host.
        Configures environment variables, including the RabbitMQ service hostname.
        Belongs to the "ch-network" network.

3. Mysql container (californian-health-db-container):
        Uses the "californian-health-db:latest" image to run a MySQL container.
        Defines a container name as "ch-db-container".
        Builds the image from a Dockerfile in the "CalifornianHealthDockerizedDB" directory.
        Exposes port 1433 from the container to port 1433 on the host.
        Configures environment variables for the MySQL database.
        Belongs to the "ch-network" network.

4. API container (californianhealthcalendarapi):
        Uses the "californianhealthcalendarapi" image to run an API.
        Defines a container name as "ch-api-container".
        Builds the image from a Dockerfile in the "CalifornianHealthCalendarApi" directory.
        Configures environment variables for the API.
        Exposes port 5000 from the container to port 80 on the host.
        Belongs to the "ch-network" network.

5. Californian Health Frontend Updated (californianhealthfrontendupdated):
        Uses the "californianhealthfrontendupdated" image to run the updated frontend UI.
        Defines a container name as "ch-frontend-container".
        Depends on the "californianhealthbookingserver" service to start.
        Builds the image from a Dockerfile in the "CalifornianHealthFrontendUpdated" directory.
        Configures environment variables, including the RabbitMQ service hostname.
        Exposes port 8080 from the container to port 80 on the host.
        Belongs to the "ch-network" network.

### Features:

- Home page : the home page detail the differents services offered by the application. It allow the user to navigate trought the app, and reach different pages. 

- Booking page : the booking page allow the user to select a doctor and see his appointments. The user can select an appointment and book it.

- Confirmation page : the confirmation page allow the user to see the confirmation of his appointment.

- My appointments page : the my appointments page allow the user to see his appointments.

### Architecture Overview:
<img src=".\CalifornianHealthFrontendUpdated\img\architecture.png" width="70%" alt="confirmation page">


## Technical Documentation:

### Technology Stack:
- .NET 7 to use MVC pattern
- ASP.NET Core to build the API, who allow the frontend to get datas about the doctors and the appointments from the database
- Entity Framework Core to interact with the database
- Docker to containerize the services
- RabbitMq to send and receive messages between/from the frontend and the booking service
- SQL Server to store the data

### Message Queue (RabbitMQ):
RabbitMq is used to send and receive messages between the frontend and the booking service. The frontend send a message to the booking service to book an appointment. The booking service try to update the database with the new appointment. If a concurrency error occurs and the booking service can't update the db because the appointment is already taken, the response message is not sent to the front, and the front will display a message to inform the user that the selected slot is already booked. If the appointment is available, the booking service send a message to the frontend to confirm the appointment.
By this way, we avoid concurrency errors and we ensure that the appointment is available before confirming it.

### Database Design:
<img src=".\CalifornianHealthFrontendUpdated\img\erm.png" width="70%" alt="confirmation page">


### API Documentation:
<a href="https://github.com/Yann-dv/CalifornianHealthApp/blob/main/CalifornianHealthCalendarApi/README.md">API Documentation</a>

### Error Handling:
If there is any concurrency error while booking, the frontend will display a message to inform the user that the selected slot is already booked. Also, if the API or the booking server is not running, the frontend will display a message to inform the user that the service is not available.

## Test Coverage Report:

<img src=".\CalifornianHealthFrontendUpdated\img\testsReport.png" width="70%" alt="tests report">

### Introduction:
The purpose of the test coverage is to test the API and the booking server. 

### Test Types:
The API is tested with unit tests that try to make several api call at the same time. The booking server is tested with unit tests to try a bunch of simultaneous booking tries. The frontend is not tested.

### Test Frameworks:
The tests are written with nUnit; 

### Test Results:
The test results are displayed in the test explorer in Visual Studio. The tests are runned with the command : dotnet test.
They show us that the API and the booking server are working as expected, even with a lot (up to 3000) of simultaneous requests or booking tries.

### Future Recommendations:

- Add a login system to allow users to create an account and book appointments with their account.
- Add a login system for doctors to allow them to add their availabilities and see their appointments.
- Add separate interfaces for doctors and patients.
- Add a system to allow users/doctors to cancel their appointments.
- Add a system to allow users/doctors to modify their appointments.
- Add a system to allow users/doctors to see their past appointments.
- Add a system to allow users/doctors to see their future appointments.
- Add a system to allow users/doctors to see their appointments history.
- Use a NoSQL database to store the data instead of a SQL database (MongoDB for example).


### Conclusion:

The Californian Health application is a booking application for patients to book appointments with doctors. It is a full-stack application built with .NET7. It was a monolithic application that was broken down into microservices. The application can be deployed on localhost and/or Docker.