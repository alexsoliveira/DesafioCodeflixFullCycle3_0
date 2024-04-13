using FC.Codeflix.Catalog.Domain.Repository;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;
public class GetCategory : IGetCategory
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategory(ICategoryRepository categoryRepository)
        => _categoryRepository = categoryRepository;

    public async Task<GetCategoryOutput> Handle(
        GetCategoryInput input,
        CancellationToken cancellationToken
    ){
        var category = await _categoryRepository.Get(input.Id,cancellationToken);
        return GetCategoryOutput.FromCategory(category);

    }
}
