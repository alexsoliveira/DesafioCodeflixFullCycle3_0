﻿using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
public interface ICreateCategory 
    : IRequestHandler<CreateCategoryInput, CategoryModelOutput>
{
    //Também usado Execute ou Process
    //public Task<CreateCategoryOutput> Handle(CreateCategoryInput input, CancellationToken cancellationToken);
}
