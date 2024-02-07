# IHiveManager

The `IHiveManager` interface serves as the gateway to the hive system, providing methods to manage hives and their associated tasks. As the primary interface for interacting with the hive system, `IHiveManager` offers functionalities for adding, retrieving, and managing hives, as well as accessing hive-related information.

By default, `IHiveManager` is registered as a singleton within the system, ensuring that there is only one instance available for managing hives across the application. This singleton pattern allows for centralized control and coordination of hive-related operations, promoting consistency and efficiency in task management.

## Functionalities:

1. **AddHive:**
   - Allows users to add a new hive to the system, associating it with a unique identifier (`Guid`) and an optional `Hive` instance. This method facilitates the creation of new hive entities within the system.

2. **GetHive (by Guid):**
   - Enables users to retrieve a hive from the system based on its unique identifier (`Guid`). This method provides access to the specified hive, allowing users to interact with its associated tasks and properties.

3. **GetHive (by name):**
   - Provides functionality to retrieve a hive from the system based on its name. This method allows users to locate hives by their descriptive names, enhancing ease of use and navigation within the system.
