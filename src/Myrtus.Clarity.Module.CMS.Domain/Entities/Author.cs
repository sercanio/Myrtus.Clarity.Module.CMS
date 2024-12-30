using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.Clarity.Module.CMS.Domain.Entities;

public class Author : Entity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }

    private Author() { }

    public Author(Guid id, string firstName, string lastName, string email) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
}
