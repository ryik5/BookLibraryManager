# Library Manager

Library Manager is a WPF-based desktop application designed to manage a library's collection of books, movies, and games. It provides a user-friendly interface for both patrons and administrators, allowing them to handle reservations, and inventory management seamlessly.

## Features

- Add new books to the library
- Edit existing book details
- Remove books from the library
- Search for books by title, author, or genre
- Store content of the book within the library
- View detailed information about each book

## Technologies Used

- **C#** – Core programming language.
- **.NET 9.0** – Framework for application development.
- **WPF (Windows Presentation Foundation)** – UI framework for building a modern desktop experience.
- **MVVM (Model-View-ViewModel)** – Architectural pattern for code separation and maintainability.
- **XAML** – Used for defining UI layouts.
- **Git** – Version control for tracking changes and collaboration.

## Installation

To set up the Library Manager application locally, follow these steps:

1. **Clone the entire solution**:
   ```bash
   git clone https://github.com/ryik5/BookLibraryManager.git
   cd BookLibraryManager/LibraryManager
   ```

2. **Build and Run the .dll libraries**:
   - Open the projects in Visual Studio.
   - Restore NuGet packages.
   - Replace icons/logos links to yours.
   - Build BookLibraryManager.Common to provide general objects
   - Build either BookLibraryManager.XmlFileLibraryOperator if you are going to work with .xml library form or create your own provider.
     
3. **Build and Run the Application**:
   - Open the project in Visual Studio.
   - Restore NuGet packages.
   - Replace icons/logos links to yours.
   - Copy dll or add links into applicatio's project for .dll
   - Build and run the application.

## Requirements

- .NET 9 SDK
- Visual Studio 2022 or later

## Contributing

Contributions to the Library Manager project are welcome. To contribute:

1. Fork the repository.
2. Create a new branch for your feature or bug fix.
3. Commit your changes with clear and descriptive messages.
4. Push your changes to your forked repository.
5. Submit a pull request detailing your changes and their purpose.
