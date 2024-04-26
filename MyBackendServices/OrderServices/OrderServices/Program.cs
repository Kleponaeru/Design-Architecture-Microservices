using OrderServices.DAL;
using OrderServices.DAL.Interfaces;
using OrderServices.DTO;
using OrderServices.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICustomer, CustomerDAL>();
builder.Services.AddScoped<IOrderDetail, OrderDetailDAL>();
builder.Services.AddScoped<IOrderHeader, OrderHeaderDAL>();
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

        customerDAL.Insert(customer);

        // Return 201 Created with the created product
        return Results.Created($"/api/customer/{customer.CustomerId}", customer);
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

app.MapPost("/api/orderHeader", (IOrderHeader orderHeaderDAL, OrderHeaderCreateDTO orderHeaderCreateDTO) =>
{
    try
    {
        OrderHeader orderHeader = new OrderHeader
        {

            CustomerId = orderHeaderCreateDTO.CustomerId,
            OrderDate = orderHeaderCreateDTO.OrderDate
        };

        orderHeaderDAL.Insert(orderHeader);

        var responseObject = new
        {
            customerId = orderHeader.CustomerId,
            orderDate = orderHeader.OrderDate
        };

        // Return 201 Created with the created product
        return Results.Created($"/api/orderHeader/{orderHeader.CustomerId}", responseObject);
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

app.MapPost("/api/orderDetails", (IOrderDetail orderDetailDAL, OrderDetailCreateDTO orderDetailCreateDTO) =>
{
    try
    {
        OrderDetail orderDetail = new OrderDetail
        {

            OrderHeaderId = orderDetailCreateDTO.OrderHeaderId,
            ProductId = orderDetailCreateDTO.ProductId,
            Price = orderDetailCreateDTO.Price,
            Quantity = orderDetailCreateDTO.Quantity,
        };

        orderDetailDAL.Insert(orderDetail);

        var responseObject = new
        {
            OrderHeaderId = orderDetailCreateDTO.OrderHeaderId,
            ProductId = orderDetailCreateDTO.ProductId,
            Price = orderDetailCreateDTO.Price,
            Quantity = orderDetailCreateDTO.Quantity,
        };

        // Return 201 Created with the created product
        return Results.Created($"/api/orderDetails/{orderDetail.OrderDetailId}", responseObject);
    }
    catch (Exception ex)
    {
        // Return 400 Bad Request if there's an error
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/api/orderDetails", (IOrderDetail orderDetailDAL, OrderDetailUpdateDTO orderDetailUpdateDTO) =>
{
    try
    {
        var orderDetail = new OrderDetail
        {
            OrderDetailId = orderDetailUpdateDTO.OrderDetailId,
            OrderHeaderId = orderDetailUpdateDTO.OrderHeaderId,
            ProductId = orderDetailUpdateDTO.ProductId,
            Price = orderDetailUpdateDTO.Price,
            Quantity = orderDetailUpdateDTO.Quantity,
        };

        orderDetailDAL.Update(orderDetail); // Call the Update method with the created Product object

        return Results.Ok();
    }
    catch (Exception ex)
    {
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
