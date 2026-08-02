# Best Burger Management
Version: 1.0.0

Best Burger Management is an application built to manage the orders from several restaurant POS (point-of-sale) instances by routing them to the correct areas of the Matrix Kitchen.


## Supported OS Systems

The following systems are supported for hosting this application:

- Windows 7 (x64) and above;
- Linux Ubuntu 18.04 and above.

## Server Preparation
#### Windows
##### Server Packages

To install this app on a Windows machine, the server must have the **.NET Core 2.2** runtime installed. For download links and more information, see [.NET Core](https://dotnet.microsoft.com/download/dotnet-core/2.2) downloads page to get the Windows version of it.

#### Unix

To install this app on a Unix machine, the following packages must be installed:

- Linux Distribution Dependencies. Check which dependencies must be installed on this [link](https://docs.microsoft.com/pt-br/dotnet/core/linux-prerequisites?tabs=netcore2x);
- .NET Core 2.2 runtime. For download links and more information, see [.NET Core 2.2](https://dotnet.microsoft.com/download/dotnet-core/2.2) downloads page to get the Linux version of it.


## Running the Application
#### Windows 

From an administrative CMD prompt, navigate to the AppRelease folder and execute the BestBurgerManager program with the following command:

```
./BestBurgerManagerAPI.exe
```

You will see the following phrase indicating that the server is up:

> Application started. Press Ctrl+C to shut down.

Open your browser and access the application endpoint: [https://localhost:5001/swagger](https://localhost:5001/swagger). Accept the untrusted certificate if prompted. As this API runs with a built-in Kestrel instance, the certificate may not be valid in some machines.

#### Linux

On Linux, open a Terminal window and navigate to the AppRelease folder. Then, execute the following command to bring up the assembly of BestBurgerManager:

```
dotnet BestBurgerManagerAPI.dll
```

You will see the same phrase indicating that the server is up:

> Application started. Press Ctrl+C to shut down.

Now Open your browser and access the application endpoint: [https://localhost:5001/swagger](https://localhost:5001/swagger)

## API Documentation
#### HTTP Endpoints Test Suite

This API used Swagger package for .NET Core 2.2 in order to provide the endpoints documentation. It also provides a complete suite to test the endpoints using a built-in REST client.

When accessing the URL [https://localhost:5001/swagger](https://localhost:5001/swagger), you will see the main page containing a detailed description for all endpoints. 

#### API Users

As this API does not connect to any database, it has fixed users defined. Here is the list of users you can use:
1. **POS 1**:
    - Id: 1
    - Name: POS1
    - Type: 1 (code for Point of Sale user type)
2. **POS 2**:
    - Id: 2
    - Name: POS2
    - Type: 1 (code for Point of Sale user type)
3. **POS 3**:
    - Id: 3
    - Name: POS3
    - Type: 1 (code for Point of Sale user type)
4. **Kitchen**:
    - Id: 4
    - Name: Kitchen
    - Type: 1 (code for Kitchen user type)


#### Placing an Order

All POST methods requires a valid JSON text on body request in order to work proprely.
Here is an example of the JSON needed to post a single order:
```
{
  "PosId": 3,
  "Items": [
    {
      "Id": 1,
      "Name": "Burger",
      "Status": 1
    }
  ],
  "Status": 1 
}
```

The **Items** refers to the products being inserted. You can send any product with any ID, as they don`t have any relevant relationship in the API. The only detail is that a product always belongs to an order.

#### Placing Multiple Orders

To place multiple orders with a single request, you just have to group all orders in a Json Array string.

Here is an example of the JSON needed to post multiple orders:

```
[ 
  {
    "PosId": 1,
    "Items": [
      {
        "Id": 1,
        "Name": "Burger",
        "Status": 1
      }
    ],
    "Status": 1 
  },
  {
    "PosId": 1,
    "Items": [
      {
        "Id": 1,
        "Name": "Fries",
        "Status": 1
      },
      {
        "Id": 2,
        "Name": "Burger",
        "Status": 1
      },
      {
        "Id": 3,
        "Name": "Drink",
        "Status": 1
      }
    ],
    "Status": 1 
  }
]
```

##### Order Status

Find below the status details defined for placed orders:

1. Pending
2. In Progress
3. Ready
4. Cancelled
5. Removed

##### Order Placement Restrictions

Only a Point of Sale user type can post an order. The Kitchen user type is not allowed to post orders by convention. If you try to place an order with the Kitchen user, a Bad Request error will be returned with a friendly message.


#### Managing an Order

When an order is placed by some Point of Sale user, the Kitchen can start managing it by changing its status and the status of the order items after routing the items to the corresponding kitchen areas.

Here is the basic flow of an order:

1. Order Placed:
    - a POS user places an order containing one or more items. The order starts in **Order Pending** status
2. Kitchen starts working on the order:
    - When an item is routed to the correct area, the Kitchen must change item status to **Being Prepared**
    - As the products are being ready, the Kitchen must set their status to **Item Ready**
    - An order will be automatically has its status changed to **In Progress** when an item starts to be produced.
3. Kitchen finishes all products and set the order status to Ready
   - When the Kitchen finished all order items, it must set the order to **Ready to Customer** status. Then, the POS knows that the order is ready to be served.

An order can also be cancelled at anytime by the Kitchen or by any POS. There is a configured endpoint for that.   

#### Kitchen Features

The Kitchen user has a dedicated endpoint located at [https://localhost:5001/api/kitchen](https://localhost:5001/api/kitchen) address. There, the Kitchen user can do the following tasks:

   - Set a product to  **Being Prepard** status;
   - Set a product to **Item Ready** status;
   - Cancel a product;
   - Set an order to status **Ready to Customer**

## Licensing

This API is licensed under GPL 3.0 license. Check out the details [here](https://opensource.org/licenses/GPL-3.0).


> © Best Burger Manager API v1.0.0