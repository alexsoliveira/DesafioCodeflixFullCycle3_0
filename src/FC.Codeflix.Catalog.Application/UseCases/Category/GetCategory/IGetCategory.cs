using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;
public interface IGetCategory
: IRequestHandler<GetCategoryInput, GetCategoryOutput>
{
    //Também usado Execute ou Process
    public Task<GetCategoryOutput> Handle(GetCategoryInput input, CancellationToken cancellationToken);
}
