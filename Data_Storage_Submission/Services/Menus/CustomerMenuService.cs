﻿using Data_Storage_Submission.Models.Entities;

namespace Data_Storage_Submission.Services.Menus;

internal class CustomerMenuService
{
    private readonly CustomerService _customerService = new();
    private readonly AddressService _addressService = new();

    public async Task<Guid> CreateCustomer()
    {
        var customer = new CustomerEntity();
        var address = new AddressEntity();
        Console.Clear();
        Console.WriteLine("Please enter the customer information to create a new customer.");
        Console.WriteLine("First name:");
        customer.FirstName = Console.ReadLine()!;
        Console.WriteLine("Last name:");
        customer.LastName = Console.ReadLine()!;
        Console.WriteLine("Email:");
        customer.Email = Console.ReadLine()!;
        Console.WriteLine("Phonenumber: ");
        customer.PhoneNumber = Console.ReadLine()!;
        Console.WriteLine("Address: ");
        address.Street = Console.ReadLine()!;
        Console.WriteLine("Zip code: ");
        address.PostalCode = Console.ReadLine()!;
        Console.WriteLine("City: ");
        address.City = Console.ReadLine()!;

        try
        {
            Console.WriteLine("Loading...");
            address = await _addressService.SaveAsync(address);
        }
        catch (ArgumentException)
        {
            address = await _addressService.GetAsync(x => x.City == address.City && x.PostalCode == address.PostalCode && x.Street == address.Street);
        }
        catch
        {
            Console.Clear();
            throw new Exception("Something went wrong when trying to save your address. Make sure that you entered valid information and try again.");
        }

        customer.AddressId = address.Id;

        try
        {
            customer = await _customerService.SaveAsync(customer);
            Console.Clear();
            Console.WriteLine("Successfully saved customer. Press enter to continue");
            Console.ReadLine();
        }
        catch
        {
            Console.Clear();
            throw new Exception("Something went wrong when trying to save the customer. Make sure that you entered valid information and try again.");
        }

        return customer.Id;
    }
}
