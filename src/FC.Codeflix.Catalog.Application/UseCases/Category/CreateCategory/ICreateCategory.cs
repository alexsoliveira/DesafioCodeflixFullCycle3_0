namespace FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
public interface ICreateCategory
{
    //Também usado Execute ou Process
    public Task<CreateCategoryOutput> Handle(CreateCategoryInput input, CancellationToken cancellationToken);
}
