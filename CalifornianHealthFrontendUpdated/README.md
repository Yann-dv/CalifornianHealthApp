# Californian Health - Frontend

##  Description

This project is the frontend of the Californian Health project. It's a web application that allows to manage to book appointments for patients with a specific doctor, without any overlap.

## Installation

Download the project and build it with an IDE. Run the project to use the application.

## Requirements

- .NET 7.0
- NuGet packages :
    > System.Web.Mvc
    > Microsoft.EntityFrameworkCore
    > Microsoft.EntityFrameworkCore.SqlServer
    > Newtonsoft.Json
- Libraries :
    > CalifornianHealthLib
  
## Usage

The application is a web application. It's composed of 3 pages :

- Home page : it's the page that allows to see the Doctors. It's composed of a list of doctors images. When you click on a doctor, the application will redirect you to the booking page.

- Booking page : it's the page that allows to see the appointments of a doctor. It's composed of a form that allows to select a doctor. When the form is submitted, the application will display the appointments of the selected doctor in the calendar. You can click on an available slot to book an appointment. If the doctor doesn't have any appointment, the application will display a message that says that the doctor doesn't have any appointment.

- Confirmation page : it's the page that confirms the booking of the appointment. It's composed of the date of the appointment and the name of the doctor.

## User Stories

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
