using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
public interface ICreateCategory 
    : IRequestHandler<CreateCategoryInput,CreateCategoryOutput>
{
    //Também usado Execute ou Process
    public Task<CreateCategoryOutput> Handle(CreateCategoryInput input, CancellationToken cancellationToken);
}
