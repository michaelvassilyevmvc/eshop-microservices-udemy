namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .WithMessage("Name is required");
        RuleFor(command => command.Category)
            .NotEmpty()
            .WithMessage("Category is required");
        RuleFor(command => command.Description)
            .NotEmpty()
            .WithMessage("Description is required");
        RuleFor(command => command.ImageFile)
            .NotEmpty()
            .WithMessage("ImageFile is required");
        RuleFor(command => command.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0")
            .NotEmpty()
            .WithMessage("Price is required");
    }
}

internal class CreateProductCommandHandler(
    IDocumentSession session,
    ILogger<CreateProductCommandHandler> logger)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // create Product entity from command object
        logger.LogInformation("CreateProductCommandHandler.Handle called with {@Command}", command);
        var product = new Product
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };

        //save to database
        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);

        // return CreateProductResult result
        return new CreateProductResult(product.Id);
    }
}