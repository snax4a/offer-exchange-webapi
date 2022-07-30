using FSH.WebApi.Application.Admin.DTOs;
using FSH.WebApi.Application.Billing.Stripe;
using FSH.WebApi.Application.Identity.Users;
using FSH.WebApi.Domain.Billing;

namespace FSH.WebApi.Application.Admin;

public class CreateCustomersRequest : IRequest<CreateCustomersResponse>
{
}

public class CreateCustomersRequestHandler : IRequestHandler<CreateCustomersRequest, CreateCustomersResponse>
{
    private readonly IStripeService _stripeService;
    private readonly IDapperRepository _dapperRepository;
    private readonly IRepositoryWithEvents<Customer> _customerRepository;

    public CreateCustomersRequestHandler(
        IStripeService stripeService,
        IDapperRepository dapperRepository,
        IRepositoryWithEvents<Customer> customerRepository)
    {
        _stripeService = stripeService;
        _dapperRepository = dapperRepository;
        _customerRepository = customerRepository;
    }

    public async Task<CreateCustomersResponse> Handle(CreateCustomersRequest request, CancellationToken ct)
    {
        int customersCreated = 0;

        // Get all users who do not have related Customer record.
        const string sql = @"
            SELECT u.""Id""::uuid, u.""FirstName"", u.""LastName"", u.""CompanyName"", u.""Email""
            FROM ""Identity"".""Users"" AS u
            LEFT OUTER JOIN ""Billing"".""Customers"" c
            ON u.""Id""::uuid = c.""UserId"" WHERE c.""UserId"" IS NULL;
        ";

        var users = await _dapperRepository.QueryAsync<UserDetailsDto>(sql);

        foreach (var user in users)
        {
            var stripeCustomer = await _stripeService.CreateCustomer(user.Email, user.CompanyName, user.Id.ToString(), ct);
            var customer = new Customer(user.Id, stripeCustomer.Id);
            await _customerRepository.AddAsync(customer, ct);
            customersCreated++;
        }

        return new CreateCustomersResponse() { CustomersCreated = customersCreated };
    }
}