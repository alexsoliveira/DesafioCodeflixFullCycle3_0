using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Domain.Repository;

namespace FC.Codeflix.Catalog.Application.UseCases.Genre.DeleteGenre
{
    public class DeleteGenre
        : IDeleteGenre
    {
        private readonly IGenreRepository _genericRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteGenre(
            IGenreRepository genericRepository, 
            IUnitOfWork unitOfWork)
        {
            _genericRepository = genericRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteGenreInput request, CancellationToken cancellationToken)
        {
            var genre = await _genericRepository.Get(
                request.Id, 
                cancellationToken
            );
            await _genericRepository.Delete(genre, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);            
        }        
    }
}
