using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cubipool.Common.Exceptions;
using Cubipool.Entity;
using Cubipool.Repository.Abstractions;
using Cubipool.Repository.Context;
using Cubipool.Service.Abstractions;
using Cubipool.Service.Dtos.Resources;
using Microsoft.EntityFrameworkCore;

namespace Cubipool.Service.Implementations
{
    public class ResourceService : IResourceService
    {
        private readonly EFDbContext _context;
        private readonly IResourceRepository _resourceRepository;
        private readonly IResourceTypeRepository _resourceTypeRepository;
        private readonly ICubicleRepository cubicleRepository;

        public ResourceService(
            EFDbContext context,
            IResourceRepository resourceRepository,
            IResourceTypeRepository resourceTypeRepository,
			ICubicleRepository cubicleRepository)
        {
            _context = context;
            _resourceRepository = resourceRepository;
            _resourceTypeRepository = resourceTypeRepository;
            this.cubicleRepository = cubicleRepository;
        }


        public async Task<IEnumerable<ResourceResponseDto>> FindAllAsync()
        {
            var items = await _resourceRepository
                .FindAllAsync();

            return items.Select(ResourceResponseDto.FromResource);
        }


        public async Task<ResourceResponseDto> FindOneByIdAsync(int id)
        {
            var item = await _resourceRepository
                .FindOneByIdAsync(id);
            return ResourceResponseDto.FromResource(item);
        }


        public async Task<ResourceResponseDto> CreateOneAsync(Resource resource)
        {
            // Validando que no exista un recurso con el codigo ingresado
            var foundByCode = await _resourceRepository.FindOneByCodeAsync(resource.Code);
            if (foundByCode != null)
                throw new BadRequestException($"Resource with code {resource.Code} already exists");


            // Validando que el tipo de recurso exista
            var resourceType = await _resourceTypeRepository.GetOneByIdAsync(resource.ResourceTypeId);
            if (resourceType == null)
                throw new BadRequestException($"Resource Type with id {resource.Id} was not found");

            resource.CreatedAt = new DateTime();
            resource.UpdatedAt = new DateTime();
            resource.IsActive = true;

            await _resourceRepository.CreateOneAsync(resource);

            return ResourceResponseDto.FromResource(resource);
        }


        public async Task<ResourceResponseDto> UpdateOneByIdAsync(int id, Resource resource)
        {
            // Validando existencia
            var foundById = await _resourceRepository.FindOneByIdAsync(id);
            if (foundById == null)
                throw new NotFoundException($"Resource with id {id} was not found");
            _context.Entry(foundById).State = EntityState.Detached;

            if (foundById.Code != resource.Code)
            {
                // Validando que no se cruce con otro
                var foundByCode = await _resourceRepository.FindOneByCodeAsync(resource.Code);
                if (foundByCode != null)
                    throw new BadRequestException($"Resource with code {resource.Code} already exists");
            }


            resource.Id = id;
            resource.UpdatedAt = new DateTime();
            await _resourceRepository.UpdateOneByIdAsync(id, resource);

            return ResourceResponseDto.FromResource(resource);
        }


        public async Task<ResourceResponseDto> DeleteOneByIdAsync(int id)
        {
            var foundById = await _resourceRepository.FindOneByIdAsync(id);
            if (foundById == null)
                throw new NotFoundException($"Resource with id {id} was not found");

            foundById = await _resourceRepository.DeleteOneAsync(foundById);
            return ResourceResponseDto.FromResource(foundById);
        }

        public async Task<IEnumerable<GetAllByCubicleIdResponse>> GetAllByCubicleIdAsync(int id)
        {
            var cubicle = await cubicleRepository.FindOneByIdAsync(id);
            if (cubicle == null)
                throw new NotFoundException($"El cubiculo con el id {id} no existe");

            var resources = _resourceRepository.GetAllByCubicleId(id);
            var response = resources.Select(r => GetAllByCubicleIdResponse.FromResource(r));
            return response;
        }
    }
}