using Demo.BuildingBlocks;
using Demo.SharedKernel.Customers;

namespace Demo.Api.Customers.Domain;

public sealed class Customer : Entity
{
    public string Name { get; private set; } = default!;
    public string Address { get; private set; } = default!;
    public string PhotoUrl { get; private set; } = default!;

    private Customer() { }


    public void Update(string name, string address)
    {
        if(string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("The name cannot be null or have only white spaces");
        }

        Name = name;
        Address = address;

        AddDomainEvent(new CustomerUpdatedDomainEvent
        {
            Id = Id,
            Name = Name,
            Address = Address
        });
    }

    public void UpdatePhoto(string photoUrl)
    {
        PhotoUrl = photoUrl;

        AddDomainEvent(new CustomerPhotoUpdatedDomainEvent
        {
            Id = Id,
            PhotoUrl = PhotoUrl
        });
    }

    public void Delete()
        => AddDomainEvent(new CustomerDeletedDomainEvent { Id = Id });

    public static Customer Create(string name, string address)
    {
        if(string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("The name cannot be null or have only white spaces");
        }

        var customer = new Customer
        {
            Name = name,
            Address = address,
        };

        customer.AddDomainEvent(new CustomerCreatedDomainEvent
        {
            Id = customer.Id,
            Name = customer.Name,
            Address = customer.Address
        });

        return customer;
    }
}
