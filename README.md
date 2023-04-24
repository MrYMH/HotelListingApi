# Hotel Listing API

This project is a hotel listing API that allows users to search and retrieve information about hotels and countries. The API is built using C# and .NET Core 7.

## Installation

To install and run the project, follow these steps:

1. Clone the repository to your local machine.
2. Open the project in your preferred IDE (e.g. Visual Studio).
3. Restore the NuGet packages by running the **dotnet restore** command.
4. Build the project by running the **dotnet build** command.
5. Run the project by running the **dotnet run** command.

## Usage
The API provides the following endpoints:

* **'GET /hotels'**: Retrieves a list of all hotels.
* **'GET /hotels/{id}'**: Retrieves information about a specific hotel.
* **'POST /hotels'**: Creates a new hotel.
* **'PUT /hotels/{id}'**: Updates an existing hotel.
* **'DELETE /hotels/{id}'**: Deletes a hotel.
* **'GET /countries'**: Retrieves a list of all countries.
* **'GET /countries/{id}'**: Retrieves information about a specific country.
* **'POST /countries'**: Creates a new country.
* **'PUT /countries/{id}'**: Updates an existing country.
* **'DELETE /countries/{id}'**: Deletes a country.

To use the API, send a request to one of the endpoints using your preferred HTTP client (e.g. Postman). The request and response formats are in JSON.

## Authentication
The API requires an API key to access. To obtain an API key, please contact the project maintainers.

## Future Plans
* API Documentation
* Enhance Search Functionality, so The API should allow you to search for hotels based on your preferred location, date, and price range.
* Provie more Accurate Information including room types, amenities, pricing, and availability.
* Allow Reviews and Ratings Functionality.

## Contributing

We welcome contributions from the community! If you find a bug or have an idea for a new feature, please open an issue on the Github repository.

## Contact Information
If you have any questions or issues, please contact the project maintainers at **'youssefhemdan02@gmail.com'**.
