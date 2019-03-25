## Table of Contents
* [User web api project](#user-web-api-project)
  * [Running User.WebApi](#user-web-api-project)
* [WCF web project](#wcf-web-project)
  * [Running WcfWebHost](#running-wcfwebhost)
* [Client MVC project](#client-mvc-project)
  * [Running TicketManagementMVC](#running-ticketmanagementmvc)
* [Client WPF project](#client-wpf-project)
  * [Running TicketManagementWPF](#running-ticketmanagementwpf)
* [Integration tests](#integration-tests)
  * [Running BusinessLogic.Services.Tests](#running-integration-tests)
* [Automated tests](#automated-tests)
  * [Running AutomatedTests](#running-automated-tests)

## User RESTful web api project

### Running User.WebApi

1. Publish TicketManagement database (if it isn't done)
2. Set up connection string to the database

## WCF web project

## Running WcfWebHost

1. Publish TicketManagement database (if it isn't done)
2. Set up connection string to the database
3. Set up special pickup directory in Web.config to keep into it emails
4. Set up email credentials and post server settings in Web.config
5. Install server certificate
   - Install it to a local machine of the Trusted Root Certificate store  
   - Add access right for a host server to a private key

## Client MVC project

### Running TicketManagementMVC

1. Make sure that wcf server and user web api server are running
2. Set up user web api host url in Web.config
3. Make sure that wcf endpoint address ports in Web.config and Wcf Web Host  port are same

## Client WPF project

### Running TicketManagementWPF

1. Make sure that wcf server and user web api server are running
2. Set up user web api host url in Web.config

## Integration tests

### Running BusinessLogic.Services.Tests

1. Build TicketManagement database. *dacpac file is being copied automatically to a needed directory or do it  manually to a directory (ProjectFolder)\src\tests\BusinessLogic.Services.Tests
2. Set up connection string to a database in App.config

## Automated tests

### Running AutomatedTests

1. Set up user mvc host url in App.config
