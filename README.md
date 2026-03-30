# Bookly - digital book goal tracker
## Overview
Bookly is a web application designed to help readers organize, track, and achieve their reading goals. The platform allows users to manage books, monitor their reading progress, and discover new titles through interactive features.

The system supports both reader functionality for managing reading goals and manager functionality for maintaining the book catalog.

The goal of Bookly is to provide a centralized digital solution for tracking reading habits and discovering books, replacing traditional methods such as handwritten lists or basic bookmarking systems.

## The problem
Many readers struggle to organize their reading habits and track their progress toward reading goals. Traditional solutions such as notebooks, bookmarks, or simple e-reader features often lack interactivity and motivation.

Bookly addresses this problem by providing a digital platform that helps users manage books, set reading goals, and explore new reading opportunities through interactive features.

## Features
### Reader Features
Set and track reading goals
Organize books into shelves such as:
 - Currently Reading
 - Have Read
Discover books through interactive features:
 - Spin the Wheel – randomly selects the next book
 - Random Date with a Book – suggests a book based on genre and rating
View book information including:
 - author
 - summary
 - ratings
 - reviews
Rate and review books

### Manager Features

The manager interface allows administrators to manage the book catalog.

 - Create books
 - Edit book information
 - View book details
 - Archive books

## System Architecture

Bookly follows a layered architecture with separation between business logic, data access, and presentation layers.

Key architectural principles used in the project include:

 - SOLID principles
 - Clean architecture
 - Entity mapping

The application also includes structured logging and error monitoring.

The original data layer was later replaced with Entity Framework using a database-first approach, allowing the domain model to be generated directly from the database schema.

Bookly also integrates with a [.NET MAUI Planner](https://github.com/KaterinaSSlavova/BooklyPlanner.git) application through REST APIs. This integration enables synchronization between the reading platform and the planner:

When a book is added to the "Want to Read" shelf in Bookly, it is automatically sent to the planner as a new reading task.
When a book is marked as completed in the planner, the system updates Bookly and moves the book to the "Have Read" shelf.

This communication enables users to manage their reading goals across both applications.

## Technology Stack
 - Backend - C#, ASP.NET Core, Entity Framework
 - Frontend - Razor Pages, HTML, CSS, Bootstrap
 - Database - SQL Server
- Additional Tools - REST APIs, Seq (structured logging)
