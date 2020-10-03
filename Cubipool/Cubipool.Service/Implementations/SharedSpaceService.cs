using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cubipool.Common.Exceptions;
using Cubipool.Entity;
using Cubipool.Repository.Abstractions;
using Cubipool.Service.Abstractions;
using Cubipool.Service.Dtos.SharedSpaces;

namespace Cubipool.Service.Implementations
{
    public class SharedSpaceService : ISharedSpaceService
    {
        private readonly IPublicationRepository _publicationRepository;
        private readonly ISharedSpaceRepository _sharedSpaceRepository;

        public SharedSpaceService(
            ISharedSpaceRepository sharedSpaceRepository,
            IPublicationRepository publicationRepository
        )
        {
            _sharedSpaceRepository = sharedSpaceRepository;
            _publicationRepository = publicationRepository;
        }

        public async Task<IEnumerable<SharedSpaceResponseDto>> FindAllByPublicationIdAsync(int id)
        {
            // La publicacion debe existir
            var publication = await _publicationRepository.FindOneByIdAsync(id);
            if (publication == null)
                throw new NotFoundException($"No se enctontro publicacion con id={id}");

            var sharedSpaces = await _sharedSpaceRepository.FindAllByPublicationId(id);
            return sharedSpaces.Select(SharedSpaceResponseDto.FromEntity);
        }
    }
}