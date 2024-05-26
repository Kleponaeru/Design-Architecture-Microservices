using System.Text.Json;
using CatalogServices.DTO;
using OrderServices.DAL;
using OrderServices.DAL.Interfaces;
using OrderServices.DTO;
using OrderServices.Models;
using OrderServices.Services;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//register services
builder.Services.AddScoped<ICustomer, CustomerDAL>();
builder.Services.AddScoped<IOrderDetail, OrderDetailDAL>();
builder.Services.AddScoped<IOrderHeader, OrderHeaderDAL>();

//register product services
builder.Services.AddHttpClient<IProductServices, ProductServices>()
.AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(10000)));

builder.Services.AddHttpClient<IWalletServices, WalletService>()
.AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(10000)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//customer

app.MapGet("/api/customer/", (ICustomer customerDAL) =>
{
    List<CustomerDTO> customerDTO = new List<CustomerDTO>();
    var customers = customerDAL.GetAll();
    foreach (var customer in customers)
    {

        customerDTO.Add(new CustomerDTO
        {
            CustomerId = customer.CustomerId,
            CustomerName = customer.CustomerName
        });
    }
    return Results.Ok(customerDTO);
});

app.MapGet("/api/customer/{id}", (ICustomer customerDAL, int id) =>
{
    CustomerDTO customerDTO = new CustomerDTO();
    var customer = customerDAL.GetById(id);
    if (customer == null)
    {
        return Results.NotFound();
    }
    customerDTO.CustomerName = customer.CustomerName;
    return Results.Ok(customer);
});

app.MapPost("/api/customer", (ICustomer customerDAL, CustomerCreateDTO customerCreateDTO) =>
{
    try
    {
        Customer customer = new Customer
        {
            CustomerName = customerCreateDTO.CustomerName,
        };

        var cust = customerDAL.Insert(customer);

        // Return 201 Created with the created product
        return Results.Created($"/api/customer/{customer.CustomerId}", cust);
    }
    catch (Exception ex)
    {
        // Return 400 Bad Request if there's an error
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/api/customer", (ICustomer customerDAL, CustomerUpdateDTO customerUpdateDTO) =>
{
    try
    {
        var customer = new Customer
        {
            CustomerId = customerUpdateDTO.CustomerId,
            CustomerName = customerUpdateDTO.CustomerName,
        };

        customerDAL.Update(customer); // Call the Update method with the created Product object

        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
app.MapDelete("/api/customer/{id}", (ICustomer customerDAL, int id) =>
{
    try
    {
        customerDAL.Delete(id);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

//OrderHeader
app.MapGet("/api/orderHeader/", (IOrderHeader orderHeaderDAL) =>
{
    List<OrderHeaderDTO> orderHeaderDTO = new List<OrderHeaderDTO>();
    var orderHeaders = orderHeaderDAL.GetAll();
    foreach (var orderHeader in orderHeaders)
    {
        orderHeaderDTO.Add(new OrderHeaderDTO
        {
            // Populate DTO properties based on Product properties
            OrderHeaderId = orderHeader.OrderHeaderId,
            CustomerId = orderHeader.CustomerId,
            OrderDate = orderHeader.OrderDate,
            CustomerName = orderHeader.CustomerName,
        });
    }
    return Results.Ok(orderHeaderDTO);
});

app.MapGet("/api/orderHeader/{id}", (IOrderHeader orderHeaderDAL, int id) =>
{
    OrderHeaderDTO orderHeaderDTO = new OrderHeaderDTO();
    var orderHeader = orderHeaderDAL.GetById(id);
    if (orderHeader == null)
    {
        return Results.NotFound();
    }
    var orderHead = new OrderHeaderDTO
    {
        OrderHeaderId = orderHeader.OrderHeaderId,
        CustomerId = orderHeader.CustomerId,
        OrderDate = orderHeader.OrderDate,
        CustomerName = orderHeader.CustomerName,
    };
    return Results.Ok(orderHead);
});

app.MapPost("/api/orderHeader", async (IOrderHeader orderHeaderDAL, IWalletServices walletServices, OrderHeaderCreateDTO orderHeaderCreateDTO) =>
{
    try
    {
        if (string.IsNullOrWhiteSpace(orderHeaderCreateDTO.Username))
        {
            return Results.BadRequest("Username must not be empty");
        }
        var wallets = await walletServices.GetSaldo(orderHeaderCreateDTO.Username);
        if (wallets == null)
        {
            return Results.BadRequest("Username not exist");
        }


        OrderHeader orderHeader = new OrderHeader
        {
            CustomerId = orderHeaderCreateDTO.CustomerId,
            Username = orderHeaderCreateDTO.Username,
            OrderDate = orderHeaderCreateDTO.OrderDate
        };

        // Insert the order header into the database
        var order = orderHeaderDAL.Insert(orderHeader);

        // Create the response object
        var responseObject = new
        {
            customerId = orderHeader.CustomerId,
            Username = orderHeaderCreateDTO.Username,
            orderDate = orderHeader.OrderDate
        };

        // Return 201 Created with the created order header
        return Results.Created($"/api/orderHeader/{order.OrderHeaderId}", responseObject);
    }
    catch (Exception ex)
    {
        // Return 400 Bad Request if there's an error
        return Results.BadRequest(ex.Message);
    }
});


app.MapPut("/api/orderHeader", (IOrderHeader orderHeaderDAL, OrderHeaderUpdateDTO orderHeaderUpdateDTO) =>
{
    try
    {
        var orderHeader = new OrderHeader
        {
            OrderHeaderId = orderHeaderUpdateDTO.OrderHeaderId,
            CustomerId = orderHeaderUpdateDTO.CustomerId,
            OrderDate = orderHeaderUpdateDTO.OrderDate
        };

        orderHeaderDAL.Update(orderHeader); // Call the Update method with the created Product object

        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
app.MapDelete("/api/orderHeader/{id}", (IOrderHeader orderHeaderDAL, int id) =>
{
    try
    {
        orderHeaderDAL.Delete(id);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});


//orderDetails
app.MapGet("/api/orderDetails/", (IOrderDetail orderDetailDAL) =>
{
    List<OrderDetailDTO> orderDetailDTO = new List<OrderDetailDTO>();
    var orderDetails = orderDetailDAL.GetAll();
    foreach (var orderDetail in orderDetails)
    {
        orderDetailDTO.Add(new OrderDetailDTO
        {
            // Populate DTO properties based on Product properties
            OrderDetailId = orderDetail.OrderDetailId,
            OrderHeaderId = orderDetail.OrderHeaderId,
            ProductId = orderDetail.ProductId,
            Price = orderDetail.Price,
            Quantity = orderDetail.Quantity,
            CustomerName = orderDetail.CustomerName,
            OrderDate = orderDetail.OrderDate,
        });
    }
    return Results.Ok(orderDetailDTO);
});

app.MapGet("/api/orderDetails/{id}", (IOrderDetail orderDetailDAL, int id) =>
{
    OrderHeaderDTO orderHeaderDTO = new OrderHeaderDTO();
    var orderDetail = orderDetailDAL.GetById(id);
    if (orderDetail == null)
    {
        return Results.NotFound();
    }
    var orderHead = new OrderDetailDTO
    {
        OrderDetailId = orderDetail.OrderDetailId,
        OrderHeaderId = orderDetail.OrderHeaderId,
        ProductId = orderDetail.ProductId,
        Price = orderDetail.Price,
        Quantity = orderDetail.Quantity,
        CustomerName = orderDetail.CustomerName,
        OrderDate = orderDetail.OrderDate,
    };
    return Results.Ok(orderHead);
});

app.MapPost("/api/orderDetails", async (IOrderDetail orderDetailDAL, IWalletServices walletServices, IProductServices productServices, OrderDetailCreateDTO orderDetailCreateDTO) =>
{
    try
    {
        // Log start of request processing
        Console.WriteLine("Processing new order detail request...");

        // Check if the product exists
        var product = await productServices.GetProductById(orderDetailCreateDTO.ProductId);
        if (product == null)
        {
            Console.WriteLine("Product not found.");
            return Results.BadRequest("Product not found");
        }

        // Check if there is enough stock
        if (product.quantity < orderDetailCreateDTO.Quantity)
        {
            Console.WriteLine("Stocks not enough.");
            return Results.BadRequest("Stocks not enough");
        }

        // Calculate the total price
        orderDetailCreateDTO.Price = product.price;
        var totalPrice = orderDetailCreateDTO.Price * orderDetailCreateDTO.Quantity;
        Console.WriteLine($"Total price calculated: {totalPrice}");

        // Check if the user's wallet exists
        var wallet = await walletServices.GetSaldo(orderDetailCreateDTO.Username);
        if (wallet == null)
        {
            Console.WriteLine("Username does not exist.");
            return Results.BadRequest("Username does not exist");
        }

        // Log the retrieved saldo for debugging
        Console.WriteLine($"Retrieved wallet saldo for user {orderDetailCreateDTO.Username}: {wallet.saldo}");

        // Check if there is sufficient saldo in the wallet
        if (wallet.saldo < totalPrice)
        {
            // Log insufficient saldo for debugging
            Console.WriteLine($"Insufficient saldo: {wallet.saldo} < {totalPrice}");
            return Results.BadRequest("Insufficient saldo");
        }

        // Deduct the saldo from the wallet
        wallet.saldo -= totalPrice; // Deduct the total price from the current saldo
        Console.WriteLine($"New wallet saldo after deduction: {wallet.saldo}");

        // Prepare data for updating wallet saldo
        var walletUpdateSaldoDTO = new WalletUpdateSaldoDTO
        {
            Username = orderDetailCreateDTO.Username,
            Saldo = wallet.saldo, // Updated saldo
        };

        // Create the order detail
        OrderDetail orderDetail = new OrderDetail
        {
            OrderHeaderId = orderDetailCreateDTO.OrderHeaderId,
            ProductId = orderDetailCreateDTO.ProductId,
            Price = totalPrice,
            Quantity = orderDetailCreateDTO.Quantity,
            Username = orderDetailCreateDTO.Username,
        };

        // Insert the order detail into the database
        var details = orderDetailDAL.Insert(orderDetail); // Ensure the insertion is awaited
        Console.WriteLine($"Order detail inserted with ID: {details.OrderDetailId}");

        // Update product stock and wallet saldo
        await productServices.UpdateProductByStock(new ProductUpdateStockDTO
        {
            ProductID = orderDetailCreateDTO.ProductId,
            Quantity = orderDetailCreateDTO.Quantity,
        });

        await walletServices.UpdateWalletBySaldo(walletUpdateSaldoDTO);
        Console.WriteLine("Product stock and wallet saldo updated.");
        var detailResponse = new
        {
            OrderDetailId = details.OrderDetailId,
            OrderHeaderId = details.OrderHeaderId,
            ProductId = details.ProductId,
            Quantity = details.Quantity,
            Price = details.Price,
            OrderDate = details.OrderDate,
            Username = details.Username,
        };

        // Return 201 Created with the created product
        return Results.Created($"/api/orderDetails/{details.OrderDetailId}", detailResponse);
    }
    catch (Exception ex)
    {
        // Return 400 Bad Request if there's an error
        Console.WriteLine($"Error occurred: {ex.Message}");
        return Results.BadRequest(ex.Message);
    }
});



app.MapDelete("/api/orderDetails/{id}", (IOrderDetail orderDetailDAL, int id) =>
{
    try
    {
        orderDetailDAL.Delete(id);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});



app.Run();
